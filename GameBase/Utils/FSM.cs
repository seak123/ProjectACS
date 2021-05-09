using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFSMState
{
    int GetKey();

    void OnEnter(FSMContext context);

    void OnUpdate(FSMContext context);

    void OnLeave(FSMContext context);
}

public delegate bool FSMTransferConditionDelegate();

public class FSMContext
{
    private Dictionary<string, object> _variables;
    public float deltaTime;
    public IFSMState lastState;
    private FSM _fsm;

    public FSMContext(FSM fsm)
    {
        _fsm = fsm;
        _variables = new Dictionary<string, object>();
    }

    public FSM FSM{
        get{
            return _fsm;
        }
    }

    public void SetVariable(string name, object value)
    {
        if(!_variables.ContainsKey(name))
        {
            _variables.Add(name,null);
        }
        _variables[name] = value;
    }

    public object GetVariable(string name)
    {
        if(_variables.ContainsKey(name))return _variables[name];
        return null;
    }
}

public class FSM
{
    private Dictionary<int, IFSMState> _stateMap;

    private List<string> _triggers;

    private IFSMState _curState;
    private FSMContext _context;

    public FSMContext Context
    {
        get{
            return _context;
        }
    }

    public int CurStateKey
    {
        get{
            return _curState.GetKey();
        }
    }

    public FSM()
    {
        _stateMap = new Dictionary<int, IFSMState>();
        _triggers = new List<string>();
        _context = new FSMContext(this);
    }

    public void OnUpdate(float deltaTime)
    {
        _context.deltaTime = deltaTime;
        if (_curState != null)
        {
            _curState.OnUpdate (_context);
        }
    }

    public void Release()
    {
        foreach (var name in _triggers)
        {
            EventManager.Instance.Off (name);
        }
        _stateMap.Clear();
        _triggers.Clear();
    }

    public void RegisterState(int key, IFSMState state)
    {
        _stateMap.Add (key, state);
    }

    public void RegisterEventTransfer(
        string triggerName,
        int fromState,
        int toState,
        FSMTransferConditionDelegate condDelegate = null
    )
    {
        EventManager
            .Instance
            .On(triggerName,
            () =>
            {
                if (_curState.GetKey() != fromState) return;
                if (condDelegate == null || condDelegate())
                {
                    SwitchToState (toState);
                }
            });

        _triggers.Add (triggerName);
    }

    public void RegisterTransfer(
        int fromState,
        int toState,
        FSMTransferConditionDelegate condDelegate
    )
    {
    }

    public void SwitchToState(int toKey)
    {
        if (_curState != null)
        {
            _curState.OnLeave(_context);
            _context.lastState = _curState;
        }
        _curState = _stateMap[toKey];
        _curState.OnEnter(_context);
    }
}
