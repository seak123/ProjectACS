using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Vector2 _cameraXLimit = Vector2.zero;
    private Vector2 _cameraYLimit = Vector2.zero;
    private BattleSession _curSession;
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
        _cameraFSM.SwitchToState((int)CameraState.NormalState);
    }

    public void Release()
    {

    }

    public void OnEnterBattle(BattleSession session)
    {
        _curSession = session;
        const float height = 17.0f;
        float cameraXOffset = -height * Mathf.Cos(50);
        int width = BattleProcedure.CurSessionVO.MapVO.Width;
        int length = BattleProcedure.CurSessionVO.MapVO.Length;
        _cameraCarrier.transform.position = new Vector3(width / 2 * BattleConst.MAP_GRID_SIDE_LENGTH, height, length / 2 * BattleConst.MAP_GRID_SIDE_LENGTH + cameraXOffset);
        _cameraCarrier.transform.rotation = Quaternion.Euler(50, 0, 0);

        _cameraXLimit = new Vector2(Mathf.Min(width, 2) * BattleConst.MAP_GRID_SIDE_LENGTH, Mathf.Max(0, width - 2) * BattleConst.MAP_GRID_SIDE_LENGTH);
        _cameraYLimit = new Vector2(Mathf.Min(length, 3) * BattleConst.MAP_GRID_SIDE_LENGTH + cameraXOffset, Mathf.Max(0, length - 3) * BattleConst.MAP_GRID_SIDE_LENGTH + cameraXOffset);


        GestureManager.Instance.SwipeAction += OnSwipe;
    }

    public void OnLeaveBattle()
    {

    }

    #region BattleCamera
    private void OnSwipe(GestureData gData)
    {
        _cameraCarrier.transform.position -= new Vector3(gData.deltaVal.x * 0.005f, 0, gData.deltaVal.y * 0.005f);
        ConstraintCamera();
    }

    private void ConstraintCamera()
    {
        float clampX = Mathf.Clamp(_cameraCarrier.transform.position.x, _cameraXLimit.x, _cameraXLimit.y);
        float clampZ = Mathf.Clamp(_cameraCarrier.transform.position.z, _cameraYLimit.x, _cameraYLimit.y);
        _cameraCarrier.transform.position = new Vector3(clampX, _cameraCarrier.transform.position.y, clampZ);
    }

    public void FocusUnit(int uid)
    {
        _cameraFSM.SwitchToState((int)CameraState.FocusUnitState);
    }
    #endregion
}

public class CameraNormalState : IFSMState
{
    public int GetKey()
    {
        return (int)CameraState.NormalState;
    }

    public void OnEnter()
    {

    }

    public void OnLeave()
    {

    }

    public void OnUpdate(float delta)
    {

    }
}

public class CameraFocusUnitState : IFSMState
{
    public int GetKey()
    {
        return (int)CameraState.FocusUnitState;
    }

    public void OnEnter()
    {
        var avatar = BattleProcedure.CurSession.Field.FindUnit(BattleProcedure.CurSession.CurSelectUid);
        var curPos = CameraManager.Instance.CameraCarrier.transform.position;
        CameraManager.Instance.CameraCarrier.transform.position = new Vector3(avatar.transform.position.x, curPos.y, avatar.transform.position.y);
    }

    public void OnLeave()
    {

    }

    public void OnUpdate(float delta)
    {

    }
}