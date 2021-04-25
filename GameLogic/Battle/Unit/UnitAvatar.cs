using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAvatar : MonoBehaviour
{
    private Vector2Int _curCoord;
    private BattleDirection _direction;

    private UnitTitle _title;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(BattleUnitVO vo)
    {
        _curCoord = vo.Coord;
        _direction = vo.Direction;

        transform.position = BattleProcedure.CurSession.Map.MapCoord2World(_curCoord);
        transform.rotation = BattleProcedure.CurSession.Map.MapDirection2Quat(_direction);

        // Unit title
        var titleObj = ResourceManager.Instance.LoadUIPrefab("UI/Prefabs/Battle/UI_UnitTitle", UILayer.Normal_3);
        _title = titleObj.AddComponent<UnitTitle>();
        _title.BindUnit(this);
    }

}
