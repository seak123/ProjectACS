using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleField
{
    private Dictionary<int, UnitAvatar> unitMap;
    private int _unitCounter;
    // Start is called before the first frame update
    public void Init()
    {
        unitMap = new Dictionary<int, UnitAvatar>();
        _unitCounter = 0;
    }

    public void CreateUnit(BattleUnitVO unitVO)
    {
        string prefabPath = unitVO.Camp == 1 ? "Prefabs/Unit/FriendUnit" : "Prefabs/Unit/EnemyUnit";
        var unitObj = ResourceManager.Instance.LoadPrefab(prefabPath);
        var avatar = unitObj.AddComponent<UnitAvatar>();
        ++_unitCounter;
        avatar.Init(unitVO);
        unitMap.Add(_unitCounter, avatar);
    }
}
