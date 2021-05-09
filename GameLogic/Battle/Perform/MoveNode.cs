using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class MoveNode : BaseNode, IPerformNode
{
    private int _casterUid;
    private BattleDirection _direction;
    private UnitAvatar _unit;
    private float _time;
    private int _stage;
    private bool bCompleted;
    public PerformNodeType Type => PerformNodeType.Move;
    public void InjectData(LuaTable table)
    {
        _casterUid = table.Get<int>("caster");
        _direction = (BattleDirection)table.Get<int>("direction");
    }
    public void Construct()
    {
        _unit = BattleProcedure.CurSession.Field.FindUnit(_casterUid);
        _time = 0f;
        _stage = 1;
        bCompleted = false;
    }
    public void Play(float deltaTime)
    {
        _time += deltaTime;
        switch (_stage)
        {
            case 1:
                {
                    _stage = 0;
                    _unit.TurnToDirection(_direction, () => { _stage = 2; });
                }
                break;
            case 2:
                {
                    _stage = 0;
                    _unit.MoveToGrid(BattleProcedure.CurSession.Map.GetAdjacentCoord(_unit.CurViewCoord, _direction), () => { bCompleted = true; });
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
