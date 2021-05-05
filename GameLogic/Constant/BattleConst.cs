using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public static class BattleConst
{
    public static Vector3 MAP_ORIGIN_POS = new Vector3(0, 0.3f, 0);
    public static float MAP_GRID_SIDE_LENGTH = 1;
    public static float MAP_GRID_HALF_SIDE_LENGTH = 0.5f;
    public static float MAP_GRID_HEIGHT_UNIT = 1;

    public static Vector3[] MAP_GRID_CORNERS =
    {
        new Vector3(MAP_GRID_HALF_SIDE_LENGTH,0f,MAP_GRID_HALF_SIDE_LENGTH),
        new Vector3(-MAP_GRID_HALF_SIDE_LENGTH,0f,MAP_GRID_HALF_SIDE_LENGTH),
        new Vector3(-MAP_GRID_HALF_SIDE_LENGTH,0f,-MAP_GRID_HALF_SIDE_LENGTH),
        new Vector3(MAP_GRID_HALF_SIDE_LENGTH,0f,-MAP_GRID_HALF_SIDE_LENGTH),
        new Vector3(MAP_GRID_HALF_SIDE_LENGTH,0f,MAP_GRID_HALF_SIDE_LENGTH),
    };
}
public enum BattleDirection
{
    North = 0,
    West = 1,
    Sourth = 2,
    East = 3
}

public enum MapGridAttr
{
    Walkable = 1 << 0,
    Obstructive = 1 << 1,
}
