using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using XLua;

public class EventManager : Singleton<EventManager>, IManager
{
    private Dictionary<string, Action> actionMap;
    private Dictionary<string,Action<object>> oneParamMap;
    private Dictionary<string,Action<object,object>> twoParamsMap;
    private Dictionary<string,Action<object,object,object>> threeParamsMap;
    public void Init()
    {
        eventMap = new Dictionary<string, Action>();
    }

    public void Release()
    {

    }

    public void On(string eventName, Action<EventArgs> callback)
    {
        if (!eventMap.ContainsKey(eventName))
        {
            eventMap.Add(eventName, callback);
        }
        else
        {
            eventMap[eventName] += callback;
        }
    }

    public void Off(string eventName, Action<EventArgs> callback)
    {
        Action<EventArgs> action;
        if (eventMap.TryGetValue(eventName, out action))
        {
            action -= callback;
        }
    }

    public void Off(string eventName)
    {
        Action<EventArgs> action;
        if (eventMap.TryGetValue(eventName, out action))
        {
            eventMap.Remove(eventName);
        }
    }

    public void Emit(string eventName, EventArgs args)
    {
        Action<EventArgs> action;
        if (eventMap.TryGetValue(eventName, out action))
        {
            action.Invoke(args);
        }
    }

    [LuaCallCSharp]
    public static void LuaEmit(string eventName,LuaTable argTable)
    {
        EventArgs eventArgs = new EventArgs(argTable);
        EventManager.Instance.Emit(eventName, eventArgs);
    }
}
