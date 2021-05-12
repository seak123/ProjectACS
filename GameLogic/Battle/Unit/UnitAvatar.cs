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
    private BattleUnitVO _vo;

    private UnitTitle _title;

    #region Properties
    public string Name { get { return _vo.Name; } }
    public int Camp
    {
        get { return _vo.Camp; }
    }
    public Vector2Int CurCoord
    {
        get
        {
            var value = BattleProcedure.CurLuaSession.Get<LuaFunction>("GetUnitCoord").Call(_uid)[0] as LuaTable;
            return new Vector2Int(value.Get<int>("x"), value.Get<int>("y"));
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

    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(int uid, BattleUnitVO vo)
    {
        _uid = uid;
        _vo = vo;
        _curCoord = vo.Coord;
        _moveSpeed = vo.MoveSpeed;
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
        var values = BattleProcedure.CurLuaSession.Get<LuaFunction>("GetUnitProperty").Call(_uid, name);
        return int.Parse(values[0].ToString());
    }

    #region Animation
    public void TurnToDirection(BattleDirection direction, Action OnCompleted = null)
    {
        var rotation = BattleProcedure.CurSession.Map.MapDirection2Quat(direction);
        float angle = Mathf.Abs(rotation.eulerAngles.y - transform.rotation.eulerAngles.y);
        float time = angle / 720;
        gameObject.transform.DORotate(rotation.eulerAngles, time).OnComplete(() => { if (OnCompleted != null) OnCompleted.Invoke(); });
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

        }
    }
    #endregion
}
