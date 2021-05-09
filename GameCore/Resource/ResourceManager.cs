using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public enum UILayer
{
    Scene,
    Normal_1,
    Normal_2,
    Normal_3,
    Normal_4,
    Notice,
}

public class ResourceManager : Singleton<ResourceManager>, IManager
{
    Dictionary<UILayer, GameObject> layerEntites;
    private MLogger _logger = new MLogger("ResourceManager");

    public void Init()
    {
        layerEntites = new Dictionary<UILayer, GameObject>();
        var canvas = GameObject.Find("MCanvas");
        GameObject.DontDestroyOnLoad(canvas);

        layerEntites.Add(UILayer.Scene, GameObject.Find("SceneLayer"));
        layerEntites.Add(UILayer.Normal_1, GameObject.Find("NormalLayer_1"));
        layerEntites.Add(UILayer.Normal_2, GameObject.Find("NormalLayer_2"));
        layerEntites.Add(UILayer.Normal_3, GameObject.Find("NormalLayer_3"));
        layerEntites.Add(UILayer.Normal_4, GameObject.Find("NormalLayer_4"));
        layerEntites.Add(UILayer.Notice, GameObject.Find("NoticeLayer"));
    }

    public void Release()
    {

    }

    public GameObject LoadPrefab(string prefabPath, bool bInstant = true)
    {
        var gameObject = Resources.Load(prefabPath) as GameObject;
        if (bInstant && gameObject != null)
        {
            return Object.Instantiate(gameObject);
        }
        return gameObject;
    }

    public GameObject LoadUIPrefab(string prefabPath, UILayer layer)
    {
        _logger.Log("Load UI Prefab ", prefabPath);
        var gameObject = Resources.Load(prefabPath) as GameObject;
        if (gameObject != null)
        {
            return Object.Instantiate(gameObject, layerEntites[layer].transform, false);
        }
        return gameObject;
    }
}
