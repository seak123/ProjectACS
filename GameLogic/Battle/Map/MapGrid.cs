using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    private BattleMap _map;
    
    private void Awake()
    {

    }

    private void Start()
    {

    }

    public void InitGrid(BattleMap map, MapGridVO vo)
    {
        _map = map;
       
        transform.position = _map.MapCoord2World(vo.Coord);
    }
}
