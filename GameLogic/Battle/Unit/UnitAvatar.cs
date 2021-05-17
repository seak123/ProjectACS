using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;
using DG.Tweening;

public class UnitAvatar : MonoBehaviour
{
    private Vector2Int _curCoord;
    private BattleDirection _direction;
    private int _uid;
    private float _moveSpeed;
    private int _camp;
    private BattleUnitVO _vo;

    private UnitTitle _title;

    #region Properties
    public int Uid
    {
        get
        {
            return _uid;
        }
    }
    public string Name { get { return _vo.Name; } }
    public int Camp
    {
        get { return _camp; }
    }
    public Vector2Int CurCoord
    {
        get
        {
            var value = BattleProcedure.CurLuaSession.GetUnitCoord(_uid);
            return value;
        }
    }
    public BattleDirection Direction
    {
        get
        {
            int direction = BattleProcedure.CurLuaSession.GetUnitDirection(_uid);
            return (BattleDirection)direction;
        }
    }
    public Vector2Int CurViewCoord
    {
        get
        {
            return BattleProcedure.CurSession.Map.World2MapCoord(transform.position);
        }
    }
    public int MaxHp
    {
        get { return GetProperty("MaxHp"); }
    }
    public int Hp
    {
        get { return GetProperty("Hp"); }
    }
    public int MaxEnergy
    {
        get { return GetProperty("MaxEnergy"); }
    }

    public int Energy
    {
        get { return GetProperty("Energy"); }
    }
    public int Speed
    {
        get { return GetProperty("Speed"); }
    }

    public UnitTitle Title
    {
        get
        {
            return _title;
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
  
    private void OnDestroy()
    {
        EventManager.Instance.Off(EventConst.ON_SELECT_OP_UNIT, OnSelected);
        if (_title != null)
        {
            GameObject.Destroy(_title.gameObject);
        }
    }
    public void Init(int uid, BattleUnitVO vo)
    {
        _uid = uid;
        _vo = vo;
        _curCoord = vo.Coord;
        _moveSpeed = vo.MoveSpeed;
        _camp = vo.Camp;
        _direction = (BattleDirection)vo.Direction;

        transform.position = BattleProcedure.CurSession.Map.MapCoord2World(_curCoord);
        transform.rotation = BattleProcedure.CurSession.Map.MapDirection2Quat(_direction);

        // Unit title
        var titleObj = ResourceManager.Instance.LoadUIPrefab("UI/Prefabs/Battle/UI_UnitTitle", UILayer.Scene);
        _title = titleObj.AddComponent<UnitTitle>();
        _title.BindUnit(this);

        EventManager.Instance.On(EventConst.ON_SELECT_OP_UNIT, OnSelected);
    }

    public void SetSelected(bool bSelect)
    {
        _title.SetSelectFlag(bSelect);
    }

    private void OnSelected(object uid)
    {
        int mUid = int.Parse(uid.ToString());
        _title.SetActFlag(mUid == _uid);
    }

    private int GetProperty(string name)
    {
        var value = BattleProcedure.CurLuaSession.GetUnitProperty(_uid, name);
        return value;
    }

    #region Animation
    public void TurnToDirection(BattleDirection direction, Action OnCompleted = null)
    {
        var rotation = BattleProcedure.CurSession.Map.MapDirection2Quat(direction);
        float angle = Mathf.Abs(rotation.eulerAngles.y - transform.rotation.eulerAngles.y);
        float time = angle / 720;
        gameObject.transform.DORotate(rotation.eulerAngles, time).OnComplete(() => { if (OnCompleted != null) OnCompleted.Invoke(); });
    }

    public void TurnToGrid(Vector2Int goal, Action OnCompleted = null)
    {
        Vector3 goalPos = BattleProcedure.CurSession.Map.MapCoord2World(goal);
        Vector2 vector = new Vector2(goalPos.x - transform.position.x, goalPos.z - transform.position.z);
        float angle = Mathf.Rad2Deg * Mathf.Asin(vector.x / vector.magnitude);
        if (vector.y < 0) angle = 180 - angle;
        Debug.Log(angle);
        float time = Vector2.Angle(transform.forward, vector) / 720;
        gameObject.transform.DORotate(new Vector3(0, angle, 0), time).OnComplete(() => { if (OnCompleted != null) OnCompleted.Invoke(); });
    }

    public void MoveToGrid(Vector2Int goal, Action OnCompleted = null)
    {
        var map = BattleProcedure.CurSession.Map;
        var goalPos = map.MapCoord2World(goal);
        float distance = Vector3.Distance(goalPos, transform.position);
        float time = distance / _moveSpeed;
        gameObject.transform.DOMove(goalPos, time).OnComplete(() => { if (OnCompleted != null) OnCompleted.Invoke(); });
    }

    public void PlayAnimation(string animName, Action OnCompleted = null)
    {
        if (animName == "Melee")
        {
            var Start = gameObject.transform.position;
            var End = gameObject.transform.position + new Vector3(0, 1, 0);
            gameObject.transform.DOMove(End, 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
            {
                if (OnCompleted != null)
                {
                    OnCompleted.Invoke();
                }
            });
        }
        else if (animName == "Hurt")
        {
            gameObject.transform.DOShakeRotation(0.2f, 30).OnComplete(() =>
            {
                if (OnCompleted != null)
                {
                    OnCompleted.Invoke();
                }
            });
        }
        else if (animName == "Dead")
        {
            float randAngle = UnityEngine.Random.Range(-180, 180);
            gameObject.transform.DORotate(new Vector3(90, randAngle, 0), 0.5f).OnComplete(() =>
              {
                  if (OnCompleted != null)
                  {
                      OnCompleted.Invoke();
                  }
              });
        }
    }

    public void PlayValueNotice(int value, ValueNoticeType noticeType)
    {
        var worldPos = transform.position + new Vector3(0, 0.5f, 0);
        var screenPos = Camera.main.WorldToScreenPoint(worldPos);
        var noticeObj = ResourceManager.Instance.LoadUIPrefab(UIConst.VALUE_NOTICE, UILayer.Notice);
        noticeObj.transform.position = screenPos;
        noticeObj.GetComponent<ValueNotice>().SetValue(value, noticeType);
    }
    #endregion
}
