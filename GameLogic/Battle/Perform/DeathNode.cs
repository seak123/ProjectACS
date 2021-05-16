using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class DeathNode : BaseNode, IPerformNode
{
    private int _casterUid;
    private bool bCompleted;
    public PerformNodeType Type => PerformNodeType.Death;
    public void InjectData(LuaTable table)
    {
        _casterUid = table.Get<int>("caster");
    }
    public void Construct()
    {
        bCompleted = false;
    }
    public void Play(float deltaTime)
    {
        BattleProcedure.CurSession.Field.DestroyUnit(_casterUid, 0.5f);
        bCompleted = true;
    }
    public bool IsFinished()
    {
        return bCompleted;
    }
}