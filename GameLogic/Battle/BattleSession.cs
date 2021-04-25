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

    public BattleMap Map
    {
        get
        {
            return _map;
        }
    }

    public BattleOrderController OrderController
    {
        get
        {
            return _orderController;
        }
    }

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

        // Units
        var units = BattleProcedure.CurSessionVO.Units;
        for (int i = 0; i < units.Count; ++i)
        {
            _field.CreateUnit(units[i]);
        }

        //Init FSM
        _battleFSM = new FSM();
        _battleFSM
            .RegisterState((int) SessionState.IdleState,
            new SessionIdleState());

        // Init OrderController
        _orderController = new BattleOrderController();
        EventManager.Instance.On(EventConst.REQ_ORDER_INPUT, this.OnTest);
    }

    public void OnUpdate(float delta)
    {
        _battleFSM.Update (delta);
    }

    private void OnTest()
    {
        LuaTable table = args.Get(0) as LuaTable;
        int count = table.Get<int>("count");
        Debug.LogWarning (count);
    }
}

public class SessionIdleState : IFSMState
{
    public int GetKey()
    {
        return (int) SessionState.IdleState;
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
        return (int) SessionState.PlayCardState;
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

public class SessionPerformState : IFSMState
{
    public int GetKey()
    {
        return (int) SessionState.PerformState;
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
