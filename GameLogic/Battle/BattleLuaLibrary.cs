using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public static class BattleLuaLibrary
{
    public static void ReqEnterBattle()
    {
        // Temp
        var sessVO = new BattleSessionVO();
        sessVO.MapVO = new BattleMapVO();
        sessVO.MapVO.Width = 8;
        sessVO.MapVO.Length = 9;
        sessVO.MapVO.Grids = new List<MapGridVO>();
        for (int x = 0; x < sessVO.MapVO.Width; ++x)
        {
            for (int y = 0; y < sessVO.MapVO.Length; ++y)
            {
                var grid = new MapGridVO();
                grid.Coord = new Vector2Int(x, y);
                grid.GridAttr = (int)MapGridAttr.Walkable;
                grid.GroundPath = "Prefabs/Map/MapGrid";
                sessVO.MapVO.Grids.Add(grid);
            }
        }

        sessVO.Units = new List<BattleUnitVO>();
        var unit1 = new BattleUnitVO();
        unit1.Name = "宫本雀";
        unit1.Camp = 1;
        unit1.Coord = new Vector2Int(0, 0);
        unit1.Direction = BattleDirection.North;
        unit1.Cards = new List<int>();
        unit1.RoundDrawNum = 5;
        unit1.Cards.Add(1);
        unit1.Cards.Add(1);
        unit1.Cards.Add(1);
        unit1.Cards.Add(1);
        unit1.Cards.Add(1);
        unit1.Cards.Add(1);
        unit1.Cards.Add(1);
        unit1.Cards.Add(1);
        unit1.Cards.Add(1);
        unit1.Cards.Add(1);
        unit1.Cards.Add(1);
        unit1.Cards.Add(1);
        var unit2 = new BattleUnitVO();
        unit2.Name = "宫本雀";
        unit2.Camp = 2;
        unit2.Coord = new Vector2Int(7, 8);
        unit2.Direction = BattleDirection.Sourth;
        unit2.RoundDrawNum = 5;
        unit2.Cards = new List<int>();
        unit2.Cards.Add(1);
        unit2.Cards.Add(1);
        unit2.Cards.Add(1);
        unit2.Cards.Add(1);
        unit2.Cards.Add(1);
        unit2.Cards.Add(1);
        unit2.Cards.Add(1);
        unit2.Cards.Add(1);
        unit2.Cards.Add(1);
        unit2.Cards.Add(1);
        unit2.Cards.Add(1);
        unit2.Cards.Add(1);

        sessVO.Units.Add(unit1);
        sessVO.Units.Add(unit2);

        ProcedureManager.Instance.OnReqEnterBattle(sessVO);
    }

    public static void OnOperateCardNotify(int demandCount, LuaTable demandTable)
    {

    }
}
