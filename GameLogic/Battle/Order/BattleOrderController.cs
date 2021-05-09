using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public interface IInputTask
{
    void InjectData(LuaTable data);
    void BeginTask(BattleOrder order);
    bool UpdateTask(float delta);
    void EndTask();
    void StopTask();
}

public enum InputTaskType
{
    Target = 0,
    Path = 1,
}

public class BattleOrderController
{
    private BattleOrder _curOrder;
    public BattleOrder TakeInputJob(List<LuaTable> taskTable)
    {
        _curOrder = new BattleOrder();
        _curOrder.type = OrderType.Play;
        _curOrder.bFinish = false;
        _curOrder.tasks = new List<IInputTask>();
        for (int i = 0; i < taskTable.Count; ++i)
        {
            InputTaskType taskType = (InputTaskType)taskTable[i].Get<int>("taskType");
            var task = CreateTask(taskType);
            task.InjectData(taskTable[i]);
            _curOrder.tasks.Add(task);
        }

        return _curOrder;
    }

    public void StopInputJob()
    {
        if (_curOrder != null)
        {
            _curOrder.StopTask();
            _curOrder = null;
        }
    }

    public void OnUpdate(float deltaTime)
    {
        if (_curOrder != null && !_curOrder.bFinish)
        {
            if (_curOrder.type == OrderType.Play)
                _curOrder.DoTask(deltaTime);
        }

    }

    private IInputTask CreateTask(InputTaskType taskType)
    {
        switch (taskType)
        {
            case InputTaskType.Target:
                {
                    var task = new UnitInputTask();
                    return task;
                }
            case InputTaskType.Path:
                {
                    var task = new PathInputTask();
                    return task;
                }
        }
        return null;
    }
}
