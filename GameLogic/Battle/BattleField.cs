using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleField
{
    private Dictionary<int, UnitAvatar> unitMap;
    // Start is called before the first frame update
    public void Init()
    {
        unitMap = new Dictionary<int, UnitAvatar>();

        EventManager.Instance.On(EventConst.ON_CREATE_UNIT, this.OnCreateUnit);
    }

    public void CreateUnit(int uid, BattleUnitVO unitVO)
    {
        string prefabPath = unitVO.Camp == 1 ? "Prefabs/Unit/FriendUnit" : "Prefabs/Unit/EnemyUnit";
        var unitObj = ResourceManager.Instance.LoadPrefab(prefabPath);
        var avatar = unitObj.AddComponent<UnitAvatar>();

        avatar.Init(uid, unitVO);
        unitMap.Add(uid, avatar);
    }

    public void DestroyUnit(int uid, float delay = 0.0f)
    {
        if (unitMap.ContainsKey(uid))
        {
            GameObject.Destroy(unitMap[uid].gameObject, delay);
            unitMap.Remove(uid);
        }
    }

    public UnitAvatar FindUnit(int uid)
    {
        if (unitMap.ContainsKey(uid)) return unitMap[uid];
        return null;
    }

    private void OnCreateUnit(object uid, object unitVO)
    {
        int mUid = int.Parse(uid.ToString());
        BattleUnitVO mVO = unitVO as BattleUnitVO;
        this.CreateUnit(mUid, mVO);
    }
}
