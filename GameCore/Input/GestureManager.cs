using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public enum GestureType
{
    Pinch,
    Swipe,
    LongPress,
    Click,
    DoubleClick,
}

public struct GestureData
{
    public GestureType gestureType;
    public Vector2 touchPos;
    public Vector2 deltaVal;
}

public class GestureManager : MonoSingleton<GestureManager>, IManager
{
    public Action<GestureData> PinchAction;
    public Action<GestureData> SwipeAction;
    public Action<GestureData> LongPressAction;
    public Action<GestureData> ClickAction;
    public Action<GestureData> DoubleClickAction;

    private MLogger _logger = new MLogger("GestureManager");

    private int _curTouchNum = 0;
    private int _touchOneId = 0;
    private int _touchTwoId = 0;
    private Vector2 _touchOnePos = Vector2.zero;
    private Vector2 _touchTwoPos = Vector2.zero;
    private float _pressTime = 0.0f;

    private const float CLICK_TRIG_TIME = 0.3f;
    private const float LONG_PRESS_TRIG_TIME = 0.8f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID || UNITY_IPHONE
        var touches = Input.touches;
        List<Touch> validTouches = new List<Touch>();
        foreach (var touch in touches)
        {
            if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                validTouches.Add(touch);
            }
        }

        if (_curTouchNum == 0)
        {
            if (validTouches.Count == 1)
            {
                _curTouchNum = 1;
                _touchOneId = validTouches[0].fingerId;
                _touchOnePos = validTouches[0].position;
                _pressTime = 0.0f;
            }
            else if (validTouches.Count >= 2)
            {
                _curTouchNum = 2;
                _touchOneId = validTouches[0].fingerId;
                _touchTwoId = validTouches[1].fingerId;
                _touchOnePos = validTouches[0].position;
                _touchTwoPos = validTouches[1].position;
            }
        }
        else if (_curTouchNum == 1)
        {
            _pressTime += Time.deltaTime;
            if (validTouches.Count == 0)
            {
                if (_pressTime < CLICK_TRIG_TIME)
                {
                    var gestureData = new GestureData();
                    gestureData.gestureType = GestureType.Click;
                    if (ClickAction != null)
                    {
                        ClickAction.Invoke(gestureData);
                    }
                }
                _curTouchNum = 0;
                _touchOneId = 0;
                _touchOnePos = Vector2.zero;
            }
            else if (validTouches.Count == 1)
            {
                var curTouchPos = validTouches[0].position;
                if (_pressTime >= LONG_PRESS_TRIG_TIME && (curTouchPos - _touchOnePos).magnitude < 0.01f)
                {
                    var gestureData = new GestureData();
                    gestureData.gestureType = GestureType.LongPress;
                    gestureData.touchPos = curTouchPos;
                    if (LongPressAction != null)
                    {
                        LongPressAction.Invoke(gestureData);
                    }
                    _touchOnePos = curTouchPos;
                }
                else if ((curTouchPos - _touchOnePos).magnitude >= 0.01f)
                {
                    var gestureData = new GestureData();
                    gestureData.gestureType = GestureType.Swipe;
                    gestureData.deltaVal = curTouchPos - _touchOnePos;
                    if (SwipeAction != null)
                    {
                        SwipeAction.Invoke(gestureData);
                    }
                    _touchOnePos = curTouchPos;
                    _pressTime = 0.0f;
                }
            }
            else
            {
                _touchTwoId = validTouches[1].fingerId;
                _touchTwoPos = validTouches[1].position;
            }
        }
        else
        {
            // TODO pinch
        }
#else

        if (_curTouchNum == 0)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _curTouchNum = 1;
                _touchOnePos = Input.mousePosition;
                _pressTime = 0.0f;
            }
        }
        else if (_curTouchNum == 1)
        {
            _pressTime += Time.deltaTime;
            if (Input.GetMouseButtonUp(0))
            {
                if (_pressTime < CLICK_TRIG_TIME)
                {
                    var gestureData = new GestureData();
                    gestureData.gestureType = GestureType.Click;
                    if (ClickAction != null)
                    {
                        ClickAction.Invoke(gestureData);
                    }
                }
                _curTouchNum = 0;
                _touchOnePos = Vector2.zero;
            }
            else if (Input.GetMouseButton(0))
            {
                var curTouchPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                if (_pressTime >= LONG_PRESS_TRIG_TIME && (curTouchPos - _touchOnePos).magnitude < 0.01f)
                {
                    var gestureData = new GestureData();
                    gestureData.gestureType = GestureType.LongPress;
                    gestureData.touchPos = curTouchPos;
                    if (LongPressAction != null)
                    {
                        LongPressAction.Invoke(gestureData);
                    }
                    _touchOnePos = curTouchPos;
                }
                else if ((curTouchPos - _touchOnePos).magnitude >= 0.01f)
                {
                    var gestureData = new GestureData();
                    gestureData.gestureType = GestureType.Swipe;
                    gestureData.deltaVal = curTouchPos - _touchOnePos;
                    if (SwipeAction != null)
                    {
                        SwipeAction.Invoke(gestureData);
                    }
                    _touchOnePos = curTouchPos;
                    _pressTime = 0.0f;
                }
            }
        }
        else
        {
            // TODO pinch
        }
#endif

    }

    public void Init()
    {

    }

    public void Release()
    {

    }
}
