using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


enum CameraState
{
    NormalState,
    FocusUnitState,
}

public class CameraManager : MonoSingleton<CameraManager>, IManager
{
    private Camera _mCamera;
    private GameObject _cameraCarrier;
    private FSM _cameraFSM;
    private BattleSession _curSession;
    private float _cameraHeight = 17f;
    private bool bIsMoving = false;
    private bool bIsRotating = false;
    private float pitchAngle = 0f;
    #region Properties
    public GameObject CameraCarrier
    {
        get
        {
            return _cameraCarrier;
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init()
    {
        _cameraCarrier = GameObject.Find("Camera");
        GameObject.DontDestroyOnLoad(_cameraCarrier);
        _mCamera = _cameraCarrier.GetComponentInChildren<Camera>();
        _cameraFSM = new FSM();
        _cameraFSM.RegisterState((int)CameraState.NormalState, new CameraNormalState());
        _cameraFSM.RegisterState((int)CameraState.FocusUnitState, new CameraFocusUnitState());
        _cameraFSM.Context.SetVariable("Camera", _mCamera);
        _cameraFSM.Context.SetVariable("Carrier", _cameraCarrier);
        _cameraFSM.SwitchToState((int)CameraState.NormalState);
    }

    public void Release()
    {

    }

    public void OnEnterBattle(BattleSession session)
    {
        _curSession = session;

        int width = BattleProcedure.CurSessionVO.MapVO.Width;
        int length = BattleProcedure.CurSessionVO.MapVO.Length;
        SetCameraRotation(new Vector3(50, 0, 0));
        SetCameraFocusPosition(new Vector2(width / 2 * BattleConst.MAP_GRID_SIDE_LENGTH, length / 2 * BattleConst.MAP_GRID_SIDE_LENGTH));

        GestureManager.Instance.SwipeAction += OnSwipe;
    }

    public void OnLeaveBattle()
    {

    }

    #region BattleCamera
    private void OnSwipe(GestureData gData)
    {
        if (bIsMoving || bIsRotating) return;
        _cameraCarrier.transform.position -= new Vector3(gData.deltaVal.x * 0.005f, 0, gData.deltaVal.y * 0.005f);
        _cameraCarrier.transform.position = ConstraintCameraPos(_cameraCarrier.transform.position);
    }

    private Vector3 ConstraintCameraPos(Vector3 pos)
    {
        int width = BattleProcedure.CurSessionVO.MapVO.Width;
        int length = BattleProcedure.CurSessionVO.MapVO.Length;
        float cameraXOffset = -_cameraHeight * Mathf.Cos(Mathf.Deg2Rad * pitchAngle);
        Vector2 _cameraXLimit = new Vector2(Mathf.Min(width, 2) * BattleConst.MAP_GRID_SIDE_LENGTH, Mathf.Max(0, width - 2) * BattleConst.MAP_GRID_SIDE_LENGTH);
        Vector2 _cameraYLimit = new Vector2(Mathf.Min(length, 3) * BattleConst.MAP_GRID_SIDE_LENGTH + cameraXOffset, Mathf.Max(0, length - 3) * BattleConst.MAP_GRID_SIDE_LENGTH + cameraXOffset);
        float clampX = Mathf.Clamp(pos.x, _cameraXLimit.x, _cameraXLimit.y);
        float clampZ = Mathf.Clamp(pos.z, _cameraYLimit.x, _cameraYLimit.y);
        return new Vector3(clampX, pos.y, clampZ);
    }

    public void FocusUnit(int uid)
    {
        _cameraFSM.SwitchToState((int)CameraState.FocusUnitState);
    }

    public void ResetCamera()
    {
        _cameraFSM.SwitchToState((int)CameraState.NormalState);
    }

    public void SetCameraFocusPosition(Vector2 relativePos, bool bTween = false)
    {
        int width = BattleProcedure.CurSessionVO.MapVO.Width;
        int length = BattleProcedure.CurSessionVO.MapVO.Length;
        float cameraXOffset = -_cameraHeight * Mathf.Cos(Mathf.Deg2Rad * pitchAngle);
        var newPos = ConstraintCameraPos(new Vector3(relativePos.x, _cameraHeight, relativePos.y + cameraXOffset));
        if (bTween)
        {
            bIsMoving = true;
            _cameraCarrier.transform.DOMove(newPos, 0.3f).SetEase(Ease.InOutExpo).OnComplete(() => { OnMoveCompleted(); });
        }
        else
        {
            _cameraCarrier.transform.position = newPos;
        }
    }
    public void SetCameraRotation(Vector3 rotation, bool bTween = false)
    {
        pitchAngle = rotation.x;
        if (bTween)
        {
            bIsRotating = true;
            _mCamera.gameObject.transform.DORotate(rotation, 0.3f).SetEase(Ease.InOutExpo).OnComplete(() => { OnRotateCompleted(); });
        }
        else
        {
            _mCamera.transform.rotation = Quaternion.Euler(rotation);
        }
    }
    #endregion

    private void OnMoveCompleted()
    {
        bIsMoving = false;
    }
    private void OnRotateCompleted()
    {
        bIsRotating = false;
    }
}

public class CameraNormalState : IFSMState
{
    public int GetKey()
    {
        return (int)CameraState.NormalState;
    }

    public void OnEnter(FSMContext context)
    {

    }

    public void OnLeave(FSMContext context)
    {

    }

    public void OnUpdate(FSMContext context)
    {

    }
}

public class CameraFocusUnitState : IFSMState
{
    private Camera _camera;
    private GameObject _carrier;
    public int GetKey()
    {
        return (int)CameraState.FocusUnitState;
    }

    public void OnEnter(FSMContext context)
    {
        _camera = context.GetVariable("Camera") as Camera;
        _carrier = context.GetVariable("Carrier") as GameObject;
        var avatar = BattleProcedure.CurSession.Field.FindUnit(BattleProcedure.CurSession.CurSelectUid);
        var curPos = CameraManager.Instance.CameraCarrier.transform.position;
        CameraManager.Instance.SetCameraRotation(new Vector3(90, 0, 0), true);
        CameraManager.Instance.SetCameraFocusPosition(new Vector2(avatar.transform.position.x, avatar.transform.position.y), true);
    }

    public void OnLeave(FSMContext context)
    {
        var pos = CameraManager.Instance.CameraCarrier.transform.position;
        CameraManager.Instance.SetCameraRotation(new Vector3(50, 0, 0), true);
        CameraManager.Instance.SetCameraFocusPosition(new Vector2(pos.x, pos.z), true);
    }

    public void OnUpdate(FSMContext context)
    {

    }
}