using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using XLua;

public class LuaManager : MonoSingleton<LuaManager>, IManager
{
    const string MainPath = "";
    private LuaEnv _luaEnv;

    public LuaEnv LuaEnv
    {
        get
        {
            return _luaEnv;
        }
    }

    public void Init()
    {
        _luaEnv = new LuaEnv();
        _luaEnv.AddLoader(LuaLoader);

        _luaEnv.DoString(
            "require 'Main' " +
            "RootFunction()");
    }

    public void Release()
    {
        _luaEnv.Dispose();
    }

    private void Update()
    {
        _luaEnv.Tick();
    }

    private byte[] LuaLoader(ref string filePath)
    {
        filePath = filePath.Replace('.', '/');
        string path = Application.streamingAssetsPath + @"/LuaScripts/" + filePath + ".lua";
        
        
#if UNITY_ANDROID || UNITY_IPHONE
        string text = File.ReadAllText(path);
#else
        string text = File.ReadAllText(path);
#endif

        return System.Text.Encoding.UTF8.GetBytes(text);
    }
}
