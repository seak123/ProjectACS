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
        var sess = BattleProcedure.CurSession;
        switch (_stage)
        {
            case 1:
                _stage = 0;
                if (_targetUid != 0)
                {
                    var target = sess.Field.FindUnit(_targetUid);
                    _unit.TurnToGrid(target.CurCoord, () => { _stage = 2; });
                }
                else
                {
                    _stage = 2;
                }
                break;
            case 2:
                _stage = 0;
                _unit.PlayAnimation(_animName, () => { _stage = 3; });
                break;
            case 3:
                if (_targetUid != 0)
                {
                    _unit.TurnToDirection(_unit.Direction, () => { bCompleted = true; });
                }
                else
                {
                    bCompleted = true;
                }
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
