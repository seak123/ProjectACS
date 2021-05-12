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
    [BlackList]
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
    private int _range = 0;
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
            case TaskUnitType.Friend:
                {
                    break;
                }
            case TaskUnitType.Enemy:
                {
                    GestureManager.Instance.ClickAction += OnClick;
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
        _range = data.Get<int>("range");
    }

    public bool UpdateTask(float delta)
    {
        return bFinish;
    }
    private void OnClick(GestureData gestureData)
    {
        var map = BattleProcedure.CurSession.Map;
        var caster = BattleProcedure.CurSession.Field.FindUnit(BattleProcedure.CurSession.CurSelectUid);
        var target = map.GetUnitByScreenPos(gestureData.touchPos);

        if (target != null)
        {
            var dis = map.GetDistanceBetweenGrids(caster.CurCoord, target.CurCoord);
            switch (_unitType)
            {
                case TaskUnitType.Enemy:
                    if (target.Camp == 3 - caster.Camp && dis <= _range)
                    {
                        if (_selectUnits.Contains(target.Uid))
                        {
                            _selectUnits.Remove(target.Uid);
                        }
                        else
                        {
                            _selectUnits.Add(target.Uid);
                        }
                    }
                    break;
                case TaskUnitType.Friend:
                    if (target.Camp == 3 - caster.Camp && dis <= _range)
                    {
                        if (_selectUnits.Contains(target.Uid))
                        {
                            _selectUnits.Remove(target.Uid);
                        }
                        else
                        {
                            _selectUnits.Add(target.Uid);
                        }
                    }
                    break;
            }
            if (_selectUnits.Count == _count)
            {
                _order.units.AddRange(_selectUnits);
                bFinish = true;
            }
        }
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
    private int _uid;
    private List<Vector2Int> _path;
    private TaskPathType _type;
    private bool bFinish = false;
    public void BeginTask(BattleOrder order)
    {
        _order = order;
        switch (_type)
        {
            case TaskPathType.WalkPath:
                {
                    _uid = _order.units[_order.units.Count - 1];
                    CameraManager.Instance.FocusUnit(_uid);
                    GestureManager.Instance.ClickAction += OnClick;
                    GestureManager.Instance.LongPressBeginAction += OnBeginPress;
                    GestureManager.Instance.LongPressAction += OnPressMove;
                    GestureManager.Instance.LongPressEndAction += OnEndPress;
                    BattleProcedure.CurSession.Map.ShowUnitMovableRegion(_uid, true, _count);
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
                    GestureManager.Instance.ClickAction -= OnClick;
                    GestureManager.Instance.LongPressBeginAction -= OnBeginPress;
                    GestureManager.Instance.LongPressAction -= OnPressMove;
                    GestureManager.Instance.LongPressEndAction -= OnEndPress;
                    BattleProcedure.CurSession.Map.ShowUnitMovableRegion(_uid, false, _count);
                    break;
                }
            default:
                return;
        }
    }

    public void StopTask()
    {
        EndTask();
        var map = BattleProcedure.CurSession.Map;
        if (_path != null) map.ShowPath(_path, false);
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
            _path = map.FindPath2Goal(unitUid, goal);
            if (_path.Count > _count + 1)
            {
                _path.RemoveRange(_count + 1, _path.Count - _count - 1);
            }
            map.ShowPath(_path, true);
            _order.paths.Add(_path);
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
