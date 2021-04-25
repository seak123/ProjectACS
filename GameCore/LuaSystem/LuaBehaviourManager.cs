using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[CSharpCallLua]
public interface ILuaBehaviourManager
{
    LuaTable CreateBehaviour(string typeName, GameObject go);

    void CleanBehaviourCache(string typeName);

    void Update();
}
public class LuaBehaviourManager : MonoSingleton<LuaBehaviourManager>, IManager
{
    private ILuaBehaviourManager _manager;

    public LuaTable CreateBehaviour(string typeName, GameObject obj)
    {
        var table = _manager.CreateBehaviour(typeName, obj);
        return table;
    }

    public void CleanBehaviourCache(string typeName)
    {
        _manager.CleanBehaviourCache(typeName);
    }

    void Update()
    {
        _manager.Update();
    }

    public void Init()
    {
        var table = LuaManager.Instance.LuaEnv.Global;
        _manager = LuaManager.Instance.LuaEnv.Global.Get<ILuaBehaviourManager>("BehaviourManager");
        if (_manager == null)
        {
            Debug.Log("null");
        }
    }


    public void Release()
    {

    }
}
