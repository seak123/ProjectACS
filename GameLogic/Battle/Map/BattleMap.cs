using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMap
{
    private int _mapLength;
    private int _mapWidth;
    private GameObject _mapRoot;
    private Dictionary<int, MapGrid> _mapGrids;

    public void Init(BattleMapVO vo)
    {
        _mapWidth = vo.Width;
        _mapLength = vo.Length;
        _mapGrids = new Dictionary<int, MapGrid>();
        _mapRoot = new GameObject("MapRoot");
        _mapRoot.transform.position = BattleConst.MAP_ORIGIN_POS;
        CreateMapGrids(vo);
    }
    #region Utils
    private bool IsMapCoordValid(int mapX, int mapY)
    {
        return mapX >= 0 && mapX < _mapWidth && mapY >= 0 && mapY < _mapLength;
    }
    private void CreateMapGrids(BattleMapVO vo)
    {
        for (int i = 0; i < vo.Grids.Count; ++i)
        {
            var gridPrefab = Resources.Load(vo.Grids[i].GroundPath) as GameObject;
            var mapGrid = Object.Instantiate(gridPrefab);
            mapGrid.transform.SetParent(_mapRoot.transform);
            var gridScript = mapGrid.AddComponent<MapGrid>();


            gridScript.InitGrid(this, vo.Grids[i]);
            _mapGrids.Add(vo.Grids[i].Coord.y * _mapWidth + vo.Grids[i].Coord.x, gridScript);
        }

    }

    public Vector3 MapCoord2World(Vector2Int coord, int height = 0)
    {
        if (!IsMapCoordValid(coord.x, coord.y)) return Vector3.zero;
        Vector3 offset = new Vector3(BattleConst.MAP_GRID_SIDE_LENGTH * coord.x + BattleConst.MAP_GRID_HALF_SIDE_LENGTH, 0, BattleConst.MAP_GRID_SIDE_LENGTH * coord.y + BattleConst.MAP_GRID_HALF_SIDE_LENGTH);
        return offset + _mapRoot.transform.position;
    }

    public Quaternion MapDirection2Quat(BattleDirection direction)
    {
        switch (direction)
        {
            case BattleDirection.East:
                return Quaternion.Euler(0, 90, 0);
            case BattleDirection.Sourth:
                return Quaternion.Euler(0, 180, 0);
            case BattleDirection.West:
                return Quaternion.Euler(0, 270, 0);
            case BattleDirection.North:
            default:
                return Quaternion.Euler(0, 0, 0);
        }
    }
    #endregion
}
