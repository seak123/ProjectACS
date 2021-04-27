using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAvatar : MonoBehaviour
{
    private Vector2Int _curCoord;
    private BattleDirection _direction;
    private int _uid;
    private BattleUnitVO _vo;
    private int _hp;
    private int _energy;

    private UnitTitle _title;

    #region Properties
    public string Name { get { return _vo.Name; } }
    
    public int Camp
    {
        get { return _vo.Camp; }
    }

    public int MaxHp
    {
        get { return _vo.MaxHp; }
    }

    public int Hp
    {
        get { return _hp; }
    }

    public int MaxEnergy
    {
        get { return _vo.MaxEnergy; }
    }

    public int Energy
    {
        get { return _energy; }
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
        _hp = vo.MaxHp;
        _energy = vo.MaxEnergy;
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

    private void OnSelected(object uid)
    {
        int mUid = int.Parse(uid.ToString());
        _title.SetSelectFlag(mUid == _uid);
    }
}
