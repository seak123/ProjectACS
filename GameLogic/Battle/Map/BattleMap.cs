using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

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
    private bool IsMapCoordValid(Vector2Int coord)
    {
        return coord.x >= 0 && coord.x < _mapWidth && coord.y >= 0 && coord.y < _mapLength;
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
    private MapGrid GetMapGrid(Vector2Int gridCoord)
    {
        int index = gridCoord.y * _mapWidth + gridCoord.x;
        if (_mapGrids.ContainsKey(index)) return _mapGrids[index];
        return null;
    }
    public MapGrid GetMapGridByScreenPos(Vector2 screenPos)
    {
        var ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("BattleMap")))
        {
            return hit.transform.gameObject.GetComponent<MapGrid>();
        }
        return null;
    }
    public UnitAvatar GetUnitByScreenPos(Vector2 screenPos)
    {
        var ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("BattleUnit")))
        {
            return hit.transform.gameObject.GetComponent<UnitAvatar>();
        }
        return null;
    }
    private bool IsMapGridMovable(int uid, Vector2Int coord)
    {
        return (bool)BattleProcedure.CurLuaSession.Get<LuaFunction>("IsGridMovable").Call(uid, coord)[0];
    }

    public Vector3 MapCoord2World(Vector2Int coord, int height = 0)
    {
        if (!IsMapCoordValid(coord)) return Vector3.zero;
        Vector3 offset = new Vector3(BattleConst.MAP_GRID_SIDE_LENGTH * coord.x + BattleConst.MAP_GRID_HALF_SIDE_LENGTH, 0, BattleConst.MAP_GRID_SIDE_LENGTH * coord.y + BattleConst.MAP_GRID_HALF_SIDE_LENGTH);
        return offset + _mapRoot.transform.position;
    }
    public Vector2Int GetAdjacentCoord(Vector2Int coord, BattleDirection direction)
    {
        Vector2Int aCoord = coord;
        switch (direction)
        {
            case BattleDirection.North:
                aCoord = aCoord + new Vector2Int(0, 1);
                break;
            case BattleDirection.East:
                aCoord = aCoord + new Vector2Int(1, 0);
                break;
            case BattleDirection.Sourth:
                aCoord = aCoord + new Vector2Int(0, -1);
                break;
            case BattleDirection.West:
                aCoord = aCoord + new Vector2Int(-1, 0);
                break;
        }
        if (IsMapCoordValid(aCoord)) return aCoord;
        return coord;
    }

    public int GetDistanceBetweenGrids(Vector2Int point1, Vector2Int point2)
    {
        return Mathf.Abs(point1.x - point2.x) + Mathf.Abs(point1.y - point2.y);
    }
    public Vector2Int World2MapCoord(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / BattleConst.MAP_GRID_SIDE_LENGTH);
        int y = Mathf.FloorToInt(pos.z / BattleConst.MAP_GRID_SIDE_LENGTH);
        return new Vector2Int(x, y);
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

    public List<Vector2Int> FindPath2Goal(int uid, Vector2Int goal)
    {
        var path = new List<Vector2Int>();
        LuaTable pathTable = BattleProcedure.CurLuaSession.Get<LuaFunction>("GetPathToGoal").Call(uid, goal)[0] as LuaTable;
        var list = pathTable.Cast<List<LuaTable>>();
        for (int i = 0; i < list.Count; ++i)
        {
            path.Add(new Vector2Int(list[i].Get<int>("x"), list[i].Get<int>("y")));
        }
        return path;
    }
    #endregion
    public void ShowUnitMovableRegion(int uid, bool bShow, int length = 0)
    {
        var unit = BattleProcedure.CurSession.Field.FindUnit(uid);
        LuaTable region = BattleProcedure.CurLuaSession.Get<LuaFunction>("GetReachableRegion").Call(uid, length)[0] as LuaTable;
        var regionList = region.Cast<List<LuaTable>>();

        foreach (var point in regionList)
        {
            var vector = new Vector2Int(point.Get<int>("x"), point.Get<int>("y"));
            GetMapGrid(vector).SwitchGridState(MapGridState.Notify, bShow);
        }
    }

    public void ShowPath(List<Vector2Int> path, bool bShow)
    {
        for (int i = 0; i < path.Count; ++i)
        {
            GetMapGrid(path[i]).SwitchGridState(MapGridState.HighLight, bShow);
        }
    }
}
