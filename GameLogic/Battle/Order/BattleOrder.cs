using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public enum OrderType
{
    Play = 0,
    Pass = 1
}

[LuaCallCSharp]
public class BattleOrder
{
    public OrderType type;
    public bool bFinish;
    public List<int> units = new List<int>();
    public List<List<Vector2Int>> paths = new List<List<Vector2Int>>();
    [BlackList]
    public List<IInputTask> tasks = new List<IInputTask>();
    [BlackList]
    private IInputTask _curTask;
    [BlackList]
    private int _curTaskIndex = 0;
    [BlackList]
    public void DoTask(float deltaTime)
    {
        if (_curTask == null)
        {
            if (_curTaskIndex < tasks.Count)
            {
                _curTask = tasks[_curTaskIndex];
                _curTask.BeginTask(this);
                ++_curTaskIndex;
            }
            else
            {
                bFinish = true;
                return;
            }
        }
        if (_curTask != null)
        {
            if (_curTask.UpdateTask(deltaTime))
            {
                _curTask.EndTask();
                _curTask = null;
            }
        }
    }

    public void StopTask()
    {
        for (int i = 0; i < _curTaskIndex; ++i)
        {
            tasks[i].StopTask();
        }
    }
}

enum TaskUnitType
{
    Self = 1,
    Friend = 2,
    Enemy = 3
}

public class UnitInputTask : IInputTask
{
    private BattleOrder _order;
    private TaskUnitType _unitType;
    private int _count = 0;
    private bool bFinish = false;
    private List<int> _selectUnits;
    public void BeginTask(BattleOrder order)
    {
        _order = order;
        _selectUnits = new List<int>();
        switch (_unitType)
        {
            case TaskUnitType.Self:
                {
                    bFinish = true;
                    BattleProcedure.CurSession.Field.FindUnit(BattleProcedure.CurSession.CurSelectUid).SetSelected(true);
                    _selectUnits.Add(BattleProcedure.CurSession.CurSelectUid);
                    _order.units.Add(BattleProcedure.CurSession.CurSelectUid);
                    break;
                }
            default:
                return;
        }
    }

    public void EndTask()
    {

    }

    public void StopTask()
    {
        for (int i = 0; i < _selectUnits.Count; ++i)
        {
            BattleProcedure.CurSession.Field.FindUnit(_selectUnits[i]).SetSelected(false);
        }
    }

    public void InjectData(LuaTable data)
    {
        _unitType = (TaskUnitType)data.Get<int>("type");
        _count = data.Get<int>("count");
    }

    public bool UpdateTask(float delta)
    {
        return bFinish;
    }
}
enum TaskPathType
{
    WalkPath = 1,
}
public class PathInputTask : IInputTask
{
    private BattleOrder _order;
    private int _count;
    private TaskPathType _type;
    private bool bFinish = false;
    public void BeginTask(BattleOrder order)
    {
        _order = order;
        switch (_type)
        {
            case TaskPathType.WalkPath:
                {
                    int unitUid = _order.units[_order.units.Count - 1];
                    CameraManager.Instance.FocusUnit(unitUid);
                    GestureManager.Instance.ClickAction += OnClick;
                    GestureManager.Instance.LongPressBeginAction += OnBeginPress;
                    GestureManager.Instance.LongPressAction += OnPressMove;
                    GestureManager.Instance.LongPressEndAction += OnEndPress;
                    BattleProcedure.CurSession.Map.ShowUnitMovableRegion(unitUid, true, _count);
                    break;
                }
            default:
                return;
        }
    }

    public void EndTask()
    {
        switch (_type)
        {
            case TaskPathType.WalkPath:
                {
                    CameraManager.Instance.ResetCamera();
                    break;
                }
            default:
                return;
        }
    }

    public void StopTask()
    {
        EndTask();
    }

    public void InjectData(LuaTable data)
    {
        _type = (TaskPathType)data.Get<int>("type");
        _count = data.Get<int>("count");
    }

    public bool UpdateTask(float delta)
    {
        return bFinish;
    }

    private void OnClick(GestureData gestureData)
    {
        int unitUid = _order.units[_order.units.Count - 1];
        var map = BattleProcedure.CurSession.Map;
        var mapGrid = map.GetMapGridByScreenPos(gestureData.touchPos);
        if (mapGrid != null)
        {
            var goal = mapGrid.Coord;
            var path = map.FindPath2Goal(unitUid, goal);
            if (path.Count > _count + 1)
            {
                path.RemoveRange(_count + 1, path.Count - _count - 1);
            }
            map.ShowPath(path, true);
            _order.paths.Add(path);
            bFinish = true;
        }
    }
    private void OnBeginPress(GestureData gestureData)
    {

    }
    private void OnPressMove(GestureData gestureData)
    {

    }
    private void OnEndPress(GestureData gestureData)
    {

    }
}
