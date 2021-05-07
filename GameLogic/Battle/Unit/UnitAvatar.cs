using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;

public class UnitAvatar : MonoBehaviour
{
    private Vector2Int _curCoord;
    private BattleDirection _direction;
    private int _uid;
    private BattleUnitVO _vo;

    private UnitTitle _title;

    #region Properties
    public string Name { get { return _vo.Name; } }
    public int Camp
    {
        get { return _vo.Camp; }
    }
    public Vector2Int CurCoord
    {
        get
        {
            var value = BattleProcedure.CurLuaSession.Get<LuaFunction>("GetUnitCoord").Call(_uid)[0] as LuaTable;
            return new Vector2Int(value.Get<int>("x"), value.Get<int>("y"));
        }
    }
    public int MaxHp
    {
        get { return GetProperty("MaxHp"); }
    }
    public int Hp
    {
        get { return GetProperty("Hp"); }
    }
    public int MaxEnergy
    {
        get { return GetProperty("MaxEnergy"); }
    }

    public int Energy
    {
        get { return GetProperty("Energy"); }
    }
    public int Speed
    {
        get { return GetProperty("Speed"); }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(int uid, BattleUnitVO vo)
    {
        _uid = uid;
        _vo = vo;
        _curCoord = vo.Coord;
        _direction = vo.Direction;

        transform.position = BattleProcedure.CurSession.Map.MapCoord2World(_curCoord);
        transform.rotation = BattleProcedure.CurSession.Map.MapDirection2Quat(_direction);

        // Unit title
        var titleObj = ResourceManager.Instance.LoadUIPrefab("UI/Prefabs/Battle/UI_UnitTitle", UILayer.Normal_3);
        _title = titleObj.AddComponent<UnitTitle>();
        _title.BindUnit(this);

        EventManager.Instance.On(EventConst.ON_SELECT_OP_UNIT, OnSelected);
    }

    public void SetSelected(bool bSelect)
    {
        _title.SetSelectFlag(bSelect);
    }

    private void OnSelected(object uid)
    {
        int mUid = int.Parse(uid.ToString());
        _title.SetActFlag(mUid == _uid);
    }

    private int GetProperty(string name)
    {
        var values = BattleProcedure.CurLuaSession.Get<LuaFunction>("GetUnitProperty").Call(_uid, name);
        return int.Parse(values[0].ToString());
    }
}
