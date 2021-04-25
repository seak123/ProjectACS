using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public static class UILibrary
{
    public static GameObject AddUIFrame(string uiPath, UILayer layer = UILayer.Normal_1)
    {
        return ResourceManager.Instance.LoadUIPrefab(uiPath, layer);
    }

    public static void RemoveUIFrame(GameObject frame)
    {
        GameObject.Destroy(frame);
    }

}
