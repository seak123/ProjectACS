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
    private BattlePerformer _performer;
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
    public BattlePerformer Performer
    {
        get
        {
            return _performer;
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

        // Init Performer
        _performer = new BattlePerformer();

        // Event
        EventManager.Instance.On(EventConst.ON_SELECT_OP_UNIT, OnSelectUnit);
        EventManager.Instance.On(EventConst.REQ_PLAYCARD_PARAMS, OnReqPlaycardParams);
        EventManager.Instance.On(EventConst.ON_CANCEL_PLAYCARD, OnCancelPlayCard);
        EventManager.Instance.On(EventConst.ON_CONFIRM_PLAYCARD, OnConfirmPlayCard);
        EventManager.Instance.On(EventConst.ON_PERFORM_START, OnPerformStart);
    }

    public void OnUpdate(float delta)
    {
        _battleFSM.Context.SetVariable("DeltaTime", delta);
        _battleFSM.Update(delta);
        _orderController.Update(delta);
    }

    private void OnSelectUnit(object uid)
    {
        _curSelectUid = int.Parse(uid.ToString());
    }

    private void OnReqPlaycardParams(object arg)
    {
        if (_battleFSM.CurStateKey == (int)SessionState.PlayCardState)
        {
            _battleFSM.SwitchToState((int)SessionState.IdleState);
        }
        if (_battleFSM.CurStateKey != (int)SessionState.IdleState) return;
        LuaTable table = arg as LuaTable;
        var paramList = table.Cast<List<LuaTable>>();
        _battleFSM.Context.SetVariable("PlayCardParamList", paramList);
        _battleFSM.SwitchToState((int)SessionState.PlayCardState);
    }

    private void OnCancelPlayCard()
    {
        _battleFSM.SwitchToState((int)SessionState.IdleState);
    }

    private void OnConfirmPlayCard()
    {
        _battleFSM.SwitchToState((int)SessionState.IdleState);
    }

    private void OnPerformStart(object arg)
    {
        LuaTable table = arg as LuaTable;
        var performRoot = table.Cast<LuaTable>();
        _battleFSM.Context.SetVariable("PerformRoot", performRoot);
        _battleFSM.SwitchToState((int)SessionState.PerformState);
    }
}

public class SessionIdleState : IFSMState
{
    public int GetKey()
    {
        return (int)SessionState.IdleState;
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

public class SessionPlayCardState : IFSMState
{
    private List<LuaTable> paramList;
    private BattleOrder playOrder;
    private bool bReady = false;
    public int GetKey()
    {
        return (int)SessionState.PlayCardState;
    }

    public void OnEnter(FSMContext context)
    {
        SetReady(false);
        paramList = context.GetVariable("PlayCardParamList") as List<LuaTable>;
        playOrder = BattleProcedure.CurSession.OrderController.TakeInputJob(paramList);
    }

    public void OnLeave(FSMContext context)
    {
        BattleProcedure.CurSession.OrderController.StopInputJob();
        SetReady(false);
        playOrder = null;
    }

    public void OnUpdate(FSMContext context)
    {
        if (playOrder != null && playOrder.bFinish)
        {
            SetReady(true);
        }
    }

    private void SetReady(bool value)
    {
        if (bReady == value) return;
        bReady = value;
        BattleOrder order = value ? playOrder : null;
        BattleProcedure.CurLuaSession.Get<LuaFunction>("UpdateReadyOrder").Call(bReady, order);
    }
}

public class SessionPerformState : IFSMState
{
    private LuaTable _performRoot;
    public int GetKey()
    {
        return (int)SessionState.PerformState;
    }

    public void OnEnter(FSMContext context)
    {
        _performRoot = context.GetVariable("PerformRoot") as LuaTable;
        BattleProcedure.CurSession.Performer.Perform(_performRoot);
    }

    public void OnLeave(FSMContext context)
    {
    }

    public void OnUpdate(FSMContext context)
    {
        float deltaTime = float.Parse(context.GetVariable("DeltaTime").ToString());
        BattleProcedure.CurSession.Performer.OnUpdate(deltaTime);
        if(BattleProcedure.CurSession.Performer.Finished)
        {

        }
    }
}
