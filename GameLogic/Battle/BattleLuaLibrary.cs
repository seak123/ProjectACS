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
        var obstructGrids = new List<Vector2Int>();
        obstructGrids.Add(new Vector2Int(5, 4));
        obstructGrids.Add(new Vector2Int(0, 5));
        obstructGrids.Add(new Vector2Int(0, 6));
        obstructGrids.Add(new Vector2Int(0, 7));
        obstructGrids.Add(new Vector2Int(0, 8));
        obstructGrids.Add(new Vector2Int(1, 8));

        for (int x = 0; x < sessVO.MapVO.Width; ++x)
        {
            for (int y = 0; y < sessVO.MapVO.Length; ++y)
            {
                var grid = new MapGridVO();
                grid.Coord = new Vector2Int(x, y);
                
                grid.Height = -0.53f;
                grid.GridAttr = obstructGrids.Contains(grid.Coord)?(int)MapGridAttr.Obstructive:(int)MapGridAttr.Walkable;
                grid.GroundPath = "Prefabs/Map/MapGrid";
                sessVO.MapVO.Grids.Add(grid);
            }
        }

        sessVO.Units = new List<BattleUnitVO>();
        var unit1 = new BattleUnitVO();
        unit1.Name = "宫本雀";
        unit1.MaxEnergy = 2;
        unit1.MaxHp = 15;
        unit1.Speed = 3;
        unit1.Camp = 1;
        unit1.Coord = new Vector2Int(0, 0);
        unit1.Direction = (int)BattleDirection.North;
        unit1.Cards = new List<int>();
        unit1.RoundDrawNum = 5;
        unit1.Cards.Add(1);
        unit1.Cards.Add(2);
        unit1.Cards.Add(1);
        unit1.Cards.Add(2);
        unit1.Cards.Add(1);
        unit1.Cards.Add(2);
        unit1.Cards.Add(1);
        unit1.Cards.Add(2);
        unit1.Cards.Add(1);
        unit1.Cards.Add(2);
        unit1.Cards.Add(1);
        unit1.Cards.Add(1);
        var unit2 = new BattleUnitVO();
        unit2.Name = "宫本雀";
        unit2.Camp = 2;
        unit2.Coord = new Vector2Int(7, 8);
        unit2.Direction = (int)BattleDirection.Sourth;
        unit2.RoundDrawNum = 3;
        unit2.Cards = new List<int>();
        unit2.MaxEnergy = 3;
        unit2.MaxHp = 15;
        unit2.Speed = 3;
        unit2.Cards.Add(1);
        unit2.Cards.Add(2);
        unit2.Cards.Add(1);
        unit2.Cards.Add(2);
        unit2.Cards.Add(2);
        unit2.Cards.Add(1);
        unit2.Cards.Add(2);
        unit2.Cards.Add(2);
        unit2.Cards.Add(1);
        unit2.Cards.Add(2);
        unit2.Cards.Add(1);
        unit2.Cards.Add(1);
        var unit3 = new BattleUnitVO();
        unit3.Name = "武藏平";
        unit3.Camp = 1;
        unit3.Coord = new Vector2Int(1, 0);
        unit3.Direction = (int)BattleDirection.North;
        unit3.RoundDrawNum = 3;
        unit3.Cards = new List<int>();
        unit3.MaxEnergy = 3;
        unit3.MaxHp = 12;
        unit3.Speed = 2;
        unit3.Cards.Add(1);
        unit3.Cards.Add(2);
        unit3.Cards.Add(1);
        unit3.Cards.Add(2);
        unit3.Cards.Add(2);
        unit3.Cards.Add(1);
        unit3.Cards.Add(2);
        unit3.Cards.Add(2);
        unit3.Cards.Add(1);
        unit3.Cards.Add(2);
        unit3.Cards.Add(1);
        unit3.Cards.Add(1);
        var unit4 = new BattleUnitVO();
        unit4.Name = "武藏平";
        unit4.Camp = 2;
        unit4.Coord = new Vector2Int(6, 8);
        unit4.Direction = (int)BattleDirection.Sourth;
        unit4.RoundDrawNum = 3;
        unit4.Cards = new List<int>();
        unit4.MaxEnergy = 3;
        unit4.MaxHp = 12;
        unit4.Speed = 2;
        unit4.Cards.Add(1);
        unit4.Cards.Add(2);
        unit4.Cards.Add(1);
        unit4.Cards.Add(2);
        unit4.Cards.Add(2);
        unit4.Cards.Add(1);
        unit4.Cards.Add(2);
        unit4.Cards.Add(2);
        unit4.Cards.Add(1);
        unit4.Cards.Add(2);
        unit4.Cards.Add(1);
        unit4.Cards.Add(1);

        sessVO.Units.Add(unit1);
        sessVO.Units.Add(unit2);
        sessVO.Units.Add(unit3);
        sessVO.Units.Add(unit4);

        ProcedureManager.Instance.OnReqEnterBattle(sessVO);
    }

    public static void OnOperateCardNotify(int demandCount, LuaTable demandTable)
    {

    }
}
