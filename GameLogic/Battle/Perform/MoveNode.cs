using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNode : BaseNode, IPerformNode
{
    private int _casterUid;
    private BattleDirection _direction;
    public PerformNodeType GetType()
    {
        return PerformNodeType.Move;
    }
    public void InjectData(LuaTable table)
    {
        _casterUid = table.Get<int>("caster");
        _direction = (BattleDirection)table.Get<int>("direction");
    }
    public void Construct()
    {
        
    }
    public void Play(float deltaTime)
    {
        
    }
    public bool IsFinished()
    {

    }
}
