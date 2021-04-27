using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public struct SessionConfig
{
    public Vector2Int mapSize;
}

public enum SessionState
{
    IdleState,
    PlayCardState,
    PerformState
}

public class BattleSession
{
    private FSM _battleFSM;
    private BattleMap _map;
    private BattleField _field;
    private BattleOrderController _orderController;
    private GameObject _battleUI;

    private int _curSelectUid = 0;
    #region Properties
    public BattleMap Map
    {
        get
        {
            return _map;
        }
    }

    public BattleField Field
    {
        get
        {
            return _field;
        }
    }

    public BattleOrderController OrderController
    {
        get
        {
            return _orderController;
        }
    }

    public int CurSelectUid
    {
        get
        {
            return _curSelectUid;
        }
    }
    #endregion

    public void InitSession()
    {
        // Init Camera
        CameraManager.Instance.OnEnterBattle(this);

        // Init Map
        _map = new BattleMap();
        _map.Init(BattleProcedure.CurSessionVO.MapVO);
        _battleUI =
            ResourceManager
                .Instance
                .LoadUIPrefab(UIConst.BATTLE_MAIN_PANEL, UILayer.Normal_1);

        // Init Field
        _field = new BattleField();
        _field.Init();

        //Init FSM
        _battleFSM = new FSM();
        _battleFSM.RegisterState((int)SessionState.IdleState, new SessionIdleState());
        _battleFSM.RegisterState((int)SessionState.PerformState, new SessionPerformState());
        _battleFSM.RegisterState((int)SessionState.PlayCardState, new SessionPlayCardState());
        _battleFSM.SwitchToState((int)SessionState.IdleState);

        // Init OrderController
        _orderController = new BattleOrderController();

        // Event
        EventManager.Instance.On(EventConst.ON_SELECT_OP_UNIT, OnSelectUnit);
        EventManager.Instance.On(EventConst.REQ_ORDER_INPUT, this.OnReqOrderInput);
    }

    public void OnUpdate(float delta)
    {
        _battleFSM.Update(delta);
    }

    private void OnSelectUnit(object uid)
    {
        _curSelectUid = int.Parse(uid.ToString());
    }

    private void OnReqOrderInput(object arg)
    {
        LuaTable table = arg as LuaTable;
        var inputList = table.Cast<List<LuaTable>>();
        int count = inputList.Count;
        _battleFSM.SwitchToState((int)SessionState.PlayCardState);
    }
}

public class SessionIdleState : IFSMState
{
    public int GetKey()
    {
        return (int)SessionState.IdleState;
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

public class SessionPlayCardState : IFSMState
{
    public int GetKey()
    {
        return (int)SessionState.PlayCardState;
    }

    public void OnEnter()
    {
        CameraManager.Instance.FocusUnit(BattleProcedure.CurSession.CurSelectUid);
    }

    public void OnLeave()
    {
    }

    public void OnUpdate(float delta)
    {
    }
}

public class SessionPerformState : IFSMState
{
    public int GetKey()
    {
        return (int)SessionState.PerformState;
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
