using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapGridState
{
    Notify = 1,
    HighLight = 1 << 1,
    Alert = 1 << 2,
}

public class MapGrid : MonoBehaviour
{
    private BattleMap _map;
    private GameObject _greenSign;
    private GameObject _redSign;
    private GameObject _blueSign;
    private Vector2Int _coord;
    private int _gridState;

    #region Properties
    public Vector2Int Coord
    {
        get { return _coord; }
    }
    #endregion
    private void Awake()
    {
        _greenSign = transform.Find("Signs/GreenSign").gameObject;
        _redSign = transform.Find("Signs/RedSign").gameObject;
        _blueSign = transform.Find("Signs/BlueSign").gameObject;

        _gridState = 0;
    }

    private void Start()
    {

    }

    public void InitGrid(BattleMap map, MapGridVO vo)
    {
        _map = map;
        _coord = vo.Coord;
        transform.position = _map.MapCoord2World(vo.Coord) + new Vector3(0, vo.Height, 0);
    }

    public void SwitchGridState(MapGridState state, bool bOn)
    {
        if (bOn)
        {
            _gridState = _gridState | (int)state;
        }
        else
        {
            _gridState = _gridState & (~(int)state);
        }
        RefreshGridSign();
    }

    private void RefreshGridSign()
    {
        _blueSign.SetActive((_gridState & (int)MapGridState.Notify) > 0);
        _greenSign.SetActive((_gridState & (int)MapGridState.HighLight) > 0);
        _redSign.SetActive((_gridState & (int)MapGridState.Alert) > 0);
    }
}
