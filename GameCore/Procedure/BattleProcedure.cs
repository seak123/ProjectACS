using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class BattleProcedure : IProcedure
{
    private MLogger _logger = new MLogger("BattleProcedure");
    private static BattleSession _curSession;
    private static BattleSessionVO _curSessionVO;

    public static BattleSession CurSession
    {
        get
        {
            return _curSession;
        }
    }

    public static BattleSessionVO CurSessionVO
    {
        get
        {
            return _curSessionVO;
        }
        set
        {
            _curSessionVO = value;
        }
    }

    public void OnEnter()
    {
        _logger.Log("enter battle");
        var sess = LuaManager.Instance.LuaEnv.Global.Get<LuaTable>("BattleSession");
        sess.Get<LuaFunction>("StartBattle").Call(CurSessionVO);
        ScenesManager.Instance.LoadScene("TestScene1", () =>
        {
            _logger.Log("init session");
            _curSession = new BattleSession();
            _curSession.InitSession();
        });
    }

    public void OnUpdate()
    {

    }

    public void OnLeave()
    {

    }

}
