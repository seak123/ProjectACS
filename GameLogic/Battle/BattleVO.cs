﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class BattleUnitVO
{
    public string Name;
    public int MaxHp;
    public int MaxEnergy;
    public int Camp;
    public Vector2Int Coord;
    public BattleDirection Direction;
    public int RoundDrawNum;
    public List<int> Cards;
}

[LuaCallCSharp]
public class MapGridVO
{
    public Vector2Int Coord;
    public string GroundPath;
    public int GridAttr;
}

[LuaCallCSharp]
public class BattleMapVO
{
    public int Length;
    public int Width;
    public List<MapGridVO> Grids;
}

[LuaCallCSharp]
public class BattleSessionVO
{
    public BattleMapVO MapVO;
    public List<BattleUnitVO> Units;
}