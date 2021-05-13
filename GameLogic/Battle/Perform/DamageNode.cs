using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class DamageNode : BaseNode, IPerformNode
{
    private int _casterUid;
    private int _targetUid;
    private int _value;
    private UnitAvatar _unit;
    private bool bCompleted;
    public PerformNodeType Type => PerformNodeType.Damage;
    public void InjectData(LuaTable table)
    {
        _casterUid = table.Get<int>("caster");
        _targetUid = table.Get<int>("target");
        _value = table.Get<int>("value");
    }
    public void Construct()
    {
        _unit = BattleProcedure.CurSession.Field.FindUnit(_targetUid);
        bCompleted = false;
    }
    public void Play(float deltaTime)
    {
        // Play Damage Fx here
        _unit.PlayValueNotice(_value, ValueNoticeType.Damage);
        _unit.Title.RefreshHp();
        bCompleted = true;
    }
    public bool IsFinished()
    {
        return bCompleted;
    }
}
