using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class AnimNode : BaseNode, IPerformNode
{
    private int _casterUid;
    private int _targetUid;
    private string _animName;
    private UnitAvatar _unit;
    private int _stage;
    private bool bCompleted;
    public PerformNodeType Type => PerformNodeType.Anim;
    public void InjectData(LuaTable table)
    {
        _casterUid = table.Get<int>("caster");
        _targetUid = table.Get<int>("target");
        _animName = table.Get<string>("animName");
    }
    public void Construct()
    {
        _unit = BattleProcedure.CurSession.Field.FindUnit(_casterUid);
        _stage = 1;
        bCompleted = false;
    }
    public void Play(float deltaTime)
    {
        switch(_stage)
        {
            case 1:
                _stage = 0;
                _unit.PlayAnimation(_animName,()=>{bCompleted = true;});
                break;
            default:
                break;
        }
    }
    public bool IsFinished()
    {
        return bCompleted;
    }
}
