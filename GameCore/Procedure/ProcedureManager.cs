using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProcedureType
{
    Prepare,
    Main,
    Battle,
}

public class ProcedureManager : MonoSingleton<ProcedureManager>, IManager
{
    private MLogger _logger = new MLogger("ProcedureManager");
    private IProcedure _curProcedure;
    private Dictionary<ProcedureType, IProcedure> _procedures;
    
    public void Init()
    {
        _logger.Log("start init");
        _procedures = new Dictionary<ProcedureType, IProcedure>();

        RegisterProcedures();
        RegisterManagers();

        SwitchProcedure(ProcedureType.Prepare);
    }

    private void Update()
    {
        if (_curProcedure != null)
        {
            _curProcedure.OnUpdate();
        }
    }

    public void Release()
    {

    }

    public void OnReqEnterBattle(BattleSessionVO sessVO)
    {
        BattleProcedure.CurSessionVO = sessVO;
        SwitchProcedure(ProcedureType.Battle);
    }

    public void SwitchProcedure(ProcedureType pType)
    {
        if (!_procedures.ContainsKey(pType)) return;
        if (_curProcedure != null)
        {
            _curProcedure.OnLeave();
        }

        _curProcedure = _procedures[pType];

        _logger.Log("enter procedure ", pType);
        _curProcedure.OnEnter();
    }

    private void RegisterProcedures()
    {
        _procedures.Add(ProcedureType.Prepare, new PrepareProcedure());
        _procedures.Add(ProcedureType.Main, new MainProcedure());
        _procedures.Add(ProcedureType.Battle, new BattleProcedure());
    }

    private void RegisterManagers()
    {
        EventManager.Instance.Init();
        ResourceManager.Instance.Init();
    }

    private void UnRegisterManagers()
    {
        EventManager.Instance.Release();
        ResourceManager.Instance.Release();
    }
}
