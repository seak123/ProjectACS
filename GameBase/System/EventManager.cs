using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class DelegateEvent
{
    public delegate void EventHandler();

    public event EventHandler eventHandle;
    public void Handle()
    {
        if (eventHandle != null)
            eventHandle();
    }
    public void removeListener(EventHandler removeHandle)
    {
        if (eventHandle != null)
            eventHandle -= removeHandle;
    }

    public void addListener(EventHandler addHandle)
    {
        eventHandle += addHandle;
    }
}
public class DelegateOneParamEvent
{
    public delegate void EventHandler(object param);

    public event EventHandler eventHandle;
    public void Handle(object arg)
    {
        if (eventHandle != null)
            eventHandle(arg);
    }
    public void removeListener(EventHandler removeHandle)
    {
        if (eventHandle != null)
            eventHandle -= removeHandle;
    }

    public void addListener(EventHandler addHandle)
    {
        eventHandle += addHandle;
    }
}
public class DelegateTwoParamEvent
{
    public delegate void EventHandler(object param1, object param2);

    public event EventHandler eventHandle;
    public void Handle(object arg1, object arg2)
    {
        if (eventHandle != null)
            eventHandle(arg1, arg2);
    }
    public void removeListener(EventHandler removeHandle)
    {
        if (eventHandle != null)
            eventHandle -= removeHandle;
    }

    public void addListener(EventHandler addHandle)
    {
        eventHandle += addHandle;
    }
}
public class DelegateThreeParamEvent
{
    public delegate void EventHandler(object param1, object param2, object param3);

    public event EventHandler eventHandle;
    public void Handle(object arg1, object arg2, object arg3)
    {
        if (eventHandle != null)
            eventHandle(arg1, arg2, arg3);
    }
    public void removeListener(EventHandler removeHandle)
    {
        if (eventHandle != null)
            eventHandle -= removeHandle;
    }

    public void addListener(EventHandler addHandle)
    {
        eventHandle += addHandle;
    }
}

public class EventManager : Singleton<EventManager>, IManager
{
    private Dictionary<string, DelegateEvent> actionMap;

    private Dictionary<string, DelegateOneParamEvent> oneParamMap;

    private Dictionary<string, DelegateTwoParamEvent> twoParamsMap;

    private Dictionary<string, DelegateThreeParamEvent> threeParamsMap;

    public void Init()
    {
        actionMap = new Dictionary<string, DelegateEvent>();
        oneParamMap = new Dictionary<string, DelegateOneParamEvent>();
        twoParamsMap = new Dictionary<string, DelegateTwoParamEvent>();
        threeParamsMap =
            new Dictionary<string, DelegateThreeParamEvent>();
    }

    public void Release()
    {
    }

    public void On(string eventName, DelegateEvent.EventHandler callback)
    {
        if (!actionMap.ContainsKey(eventName))
        {
            DelegateEvent delegateEvent = new DelegateEvent();
            actionMap.Add(eventName, delegateEvent);
        }

        actionMap[eventName].addListener(callback);
    }

    public void Off(string eventName, DelegateEvent.EventHandler callback)
    {
        if (actionMap.ContainsKey(eventName))
        {
            actionMap[eventName].removeListener(callback);
        }
    }

    public void On(string eventName, DelegateOneParamEvent.EventHandler callback)
    {
        if (!oneParamMap.ContainsKey(eventName))
        {
            DelegateOneParamEvent delegateEvent = new DelegateOneParamEvent();
            oneParamMap.Add(eventName, delegateEvent);
        }

        oneParamMap[eventName].addListener(callback);
    }

    public void Off(string eventName, DelegateOneParamEvent.EventHandler callback)
    {
        if (oneParamMap.ContainsKey(eventName))
        {
            oneParamMap[eventName].removeListener(callback);
        }
    }

    public void On(string eventName, DelegateTwoParamEvent.EventHandler callback)
    {
        if (!twoParamsMap.ContainsKey(eventName))
        {
            DelegateTwoParamEvent delegateEvent = new DelegateTwoParamEvent();
            twoParamsMap.Add(eventName, delegateEvent);
        }

        twoParamsMap[eventName].addListener(callback);
    }

    public void Off(string eventName, DelegateTwoParamEvent.EventHandler callback)
    {
        if (twoParamsMap.ContainsKey(eventName))
        {
            twoParamsMap[eventName].removeListener(callback);
        }
    }

    public void On(string eventName, DelegateThreeParamEvent.EventHandler callback)
    {
        if (!threeParamsMap.ContainsKey(eventName))
        {
            DelegateThreeParamEvent delegateEvent = new DelegateThreeParamEvent();
            threeParamsMap.Add(eventName, delegateEvent);
        }

        threeParamsMap[eventName].addListener(callback);
    }

    public void Off(string eventName, DelegateThreeParamEvent.EventHandler callback)
    {
        if (threeParamsMap.ContainsKey(eventName))
        {
            threeParamsMap[eventName].removeListener(callback);
        }
    }

    public void Off(string eventName)
    {
        if (actionMap.ContainsKey(eventName))
        {
            actionMap.Remove(eventName);
        }
        if (oneParamMap.ContainsKey(eventName))
        {
            oneParamMap.Remove(eventName);
        }
        if (twoParamsMap.ContainsKey(eventName))
        {
            twoParamsMap.Remove(eventName);
        }
        if (threeParamsMap.ContainsKey(eventName))
        {
            threeParamsMap.Remove(eventName);
        }
    }

    public void Emit(string eventName, params object[] args)
    {
        switch (args.Length)
        {
            case 0:
                {
                    if (actionMap.ContainsKey(eventName))
                    {
                        actionMap[eventName].Handle();
                    }
                    break;
                }
            case 1:
                {
                    if (oneParamMap.ContainsKey(eventName))
                    {
                        oneParamMap[eventName].Handle(args[0]);
                    }
                    break;
                }
            case 2:
                {
                    if (twoParamsMap.ContainsKey(eventName))
                    {
                        twoParamsMap[eventName].Handle(args[0], args[1]);
                    }
                    break;
                }
            case 3:
                {
                    if (threeParamsMap.ContainsKey(eventName))
                    {
                        threeParamsMap[eventName].Handle(args[0], args[1], args[2]);
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
        List<object> args = argTable.Get<List<object>>("args");
        switch (count)
        {
            case 0:
                {
                    EventManager.Instance.Emit(eventName);
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
