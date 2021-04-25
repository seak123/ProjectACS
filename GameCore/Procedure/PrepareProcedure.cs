using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareProcedure : IProcedure
{
    private MLogger _logger = new MLogger("PrepareProcedure");
    private BattleSession session;
    public void OnEnter()
    {
        _logger.Log("enter prepare");
        InitManagers();

        ProcedureManager.Instance.SwitchProcedure(ProcedureType.Main);
    }

    public void OnUpdate()
    {

    }

    public void OnLeave()
    {

    }

    private void InitManagers()
    {
        LuaManager.Instance.Init();
        LuaBehaviourManager.Instance.Init();
        GestureManager.Instance.Init();
        CameraManager.Instance.Init();
        ScenesManager.Instance.Init();
    }
}
