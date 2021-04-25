using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class EventManager : Singleton<EventManager>, IManager
{
    private Dictionary<string, Action> actionMap;

    private Dictionary<string, Action<object>> oneParamMap;

    private Dictionary<string, Action<object, object>> twoParamsMap;

    private Dictionary<string, Action<object, object, object>> threeParamsMap;

    public void Init()
    {
        actionMap = new Dictionary<string, Action>();
        oneParamMap = new Dictionary<string, Action<object>>();
        twoParamsMap = new Dictionary<string, Action<object, object>>();
        threeParamsMap =
            new Dictionary<string, Action<object, object, object>>();
    }

    public void Release()
    {
    }

    public void On(string eventName, Action callback)
    {
        if (!actionMap.ContainsKey(eventName))
        {
            actionMap.Add (eventName, callback);
        }
        else
        {
            actionMap[eventName] += callback;
        }
    }

    public void Off(string eventName, Action callback)
    {
        Action action;
        if (actionMap.TryGetValue(eventName, out action))
        {
            action -= callback;
        }
    }

    public void On(string eventName, Action<object> callback)
    {
        if (!oneParamMap.ContainsKey(eventName))
        {
            oneParamMap.Add (eventName, callback);
        }
        else
        {
            oneParamMap[eventName] += callback;
        }
    }

    public void Off(string eventName, Action<object> callback)
    {
        Action<object> action;
        if (oneParamMap.TryGetValue(eventName, out action))
        {
            action -= callback;
        }
    }

    public void On(string eventName, Action<object, object> callback)
    {
        if (!twoParamsMap.ContainsKey(eventName))
        {
            twoParamsMap.Add (eventName, callback);
        }
        else
        {
            twoParamsMap[eventName] += callback;
        }
    }

    public void Off(string eventName, Action<object, object> callback)
    {
        Action<object, object> action;
        if (twoParamsMap.TryGetValue(eventName, out action))
        {
            action -= callback;
        }
    }

    public void On(string eventName, Action<object, object, object> callback)
    {
        if (!threeParamsMap.ContainsKey(eventName))
        {
            threeParamsMap.Add (eventName, callback);
        }
        else
        {
            threeParamsMap[eventName] += callback;
        }
    }

    public void Off(string eventName, Action<object, object, object> callback)
    {
        Action<object, object, object> action;
        if (threeParamsMap.TryGetValue(eventName, out action))
        {
            action -= callback;
        }
    }

    public void Off(string eventName)
    {
        if (actionMap.ContainsKey(eventName))
        {
            actionMap.Remove (eventName);
        }
        if (oneParamMap.ContainsKey(eventName))
        {
            oneParamMap.Remove (eventName);
        }
        if (twoParamsMap.ContainsKey(eventName))
        {
            twoParamsMap.Remove (eventName);
        }
        if (threeParamsMap.ContainsKey(eventName))
        {
            threeParamsMap.Remove (eventName);
        }
    }

    public void Emit(string eventName, params object[] args)
    {
        switch (args.Length)
        {
            case 0:
                {
                    Action action;
                    if (actionMap.TryGetValue(eventName, out action))
                    {
                        action.Invoke();
                    }
                    break;
                }
            case 1:
                {
                    Action action;
                    if (oneParamMap.TryGetValue(eventName, out action))
                    {
                        action.Invoke(args[0]);
                    }
                    break;
                }
            case 2:
                {
                    Action action;
                    if (twoParamsMap.TryGetValue(eventName, out action))
                    {
                        action.Invoke(args[0], args[1]);
                    }
                    break;
                }
            case 3:
                {
                    Action action;
                    if (threeParamsMap.TryGetValue(eventName, out action))
                    {
                        action.Invoke(args[0], args[1], args[2]);
                    }
                    break;
                }
            default:
                return;
        }
    }

    [LuaCallCSharp]
    public static void LuaEmit(string eventName, LuaTable argTable)
    {
        int count = argTable.Get<int>("argCount");
        List<object> args = argTable.Get<List<object>>("argTable");
        switch (count)
        {
            case 0:
                {
                    EventManager.Instance.Emit (eventName);
                    break;
                }
            case 1:
                {
                    EventManager.Instance.Emit(eventName, args[0]);
                    break;
                }
            case 2:
                {
                    EventManager.Instance.Emit(eventName, args[0], args[1]);
                    break;
                }
            case 3:
                {
                    EventManager
                        .Instance
                        .Emit(eventName, args[0], args[1], args[2]);
                    break;
                }
            default:
                return;
        }
    }
}
