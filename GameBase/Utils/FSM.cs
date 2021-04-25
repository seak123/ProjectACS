using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IFSMState
{
    int GetKey();
    void OnEnter();
    void OnUpdate(float delta);
    void OnLeave();
}

public delegate bool FSMTransferConditionDelegate();
public class FSM
{
    private Dictionary<int, IFSMState> stateMap;
    private List<string> triggers;
    private IFSMState curState;

    public FSM()
    {
        stateMap = new Dictionary<int, IFSMState>();
        triggers = new List<string>();
    }

    public void Update(float deltaTime)
    {
        if (curState != null)
        {
            curState.OnUpdate(deltaTime);
        }
    }

    public void Release()
    {
        foreach (var name in triggers)
        {
            EventManager.Instance.Off(name);
        }
        stateMap.Clear();
        triggers.Clear();
    }

    public void RegisterState(int key, IFSMState state)
    {
        stateMap.Add(key, state);
    }
    public void RegisterEventTransfer(string triggerName, int fromState, int toState, FSMTransferConditionDelegate condDelegate = null)
    {
        EventManager.Instance.On(triggerName, (EventArgs args) =>
        {
            if (curState.GetKey() != fromState) return;
            if (condDelegate == null || condDelegate())
            {
                SwitchToState(toState);
            }
        });

        triggers.Add(triggerName);
    }

    public void RegisterTransfer(int fromState,int toState, FSMTransferConditionDelegate condDelegate)
    {

    }

    public void SwitchToState(int toKey)
    {
        if (curState != null)
        {
            curState.OnLeave();
        }
        curState = stateMap[toKey];
        curState.OnEnter();
    }
}
