using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XLua;

[LuaCallCSharp]
public class MButton : Button
{
    // ���캯��
    protected MButton()
    {
        my_onDoubleClick = new ButtonClickedEvent();
        my_onLongPress = new ButtonClickedEvent();
    }

    // ����
    public ButtonClickedEvent my_onLongPress;
    public ButtonClickedEvent my_onLongPressEnd;
    public ButtonClickedEvent onLongPress
    {
        get { return my_onLongPress; }
        set { my_onLongPress = value; }
    }

    public ButtonClickedEvent onLongPressEnd
    {
        get { return my_onLongPressEnd; }
        set { my_onLongPressEnd = value; }
    }

    // ˫��
    public ButtonClickedEvent my_onDoubleClick;
    public ButtonClickedEvent onDoubleClick
    {
        get { return my_onDoubleClick; }
        set { my_onDoubleClick = value; }
    }

    // ������Ҫ�ı�������
    private bool my_isStartPress = false;
    private float my_curPointDownTime = 0f;
    private float my_longPressTime = 0.6f;
    private bool my_longPressTrigger = false;
    private bool my_longPressEndTrigger = false;


    void Update()
    {
        CheckIsLongPress();
    }

    #region ����

    /// <summary>
    /// ������
    /// </summary>
    void CheckIsLongPress()
    {
        if (my_isStartPress && !my_longPressTrigger)
        {
            if (Time.time > my_curPointDownTime + my_longPressTime)
            {
                my_longPressTrigger = true;
                my_longPressEndTrigger = false;
                my_isStartPress = false;
                if (my_onLongPress != null)
                {
                    my_onLongPress.Invoke();
                }
            }
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // ����ˢ�®�ǰ�r�g
        base.OnPointerDown(eventData);
        my_curPointDownTime = Time.time;
        my_isStartPress = true;
        my_longPressTrigger = false;
        my_longPressEndTrigger = false;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        // ָᘔE�𣬽Y���_ʼ�L��
        base.OnPointerUp(eventData);
        my_isStartPress = false;
        if (my_longPressTrigger&& !my_longPressEndTrigger && my_onLongPressEnd != null)
        {
            my_onLongPressEnd.Invoke();
            my_longPressEndTrigger = true;
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        // ָ��Ƴ����Y���_ʼ�L����Ӌ�r�L����־
        base.OnPointerExit(eventData);
        my_isStartPress = false;
        if (my_longPressTrigger && !my_longPressEndTrigger && my_onLongPressEnd != null)
        {
            my_onLongPressEnd.Invoke();
            my_longPressEndTrigger = true;
        }
    }

    #endregion

    #region ˫����������

    public override void OnPointerClick(PointerEventData eventData)
    {
        //(�����ѽ��c���M���L���󣬔E�����r)
        if (!my_longPressTrigger)
        {
            // �����Γ� 
            if (eventData.clickCount == 2)
            {

                if (my_onDoubleClick != null)
                {
                    my_onDoubleClick.Invoke();
                }

            }// �p��
            else if (eventData.clickCount == 1)
            {
                onClick.Invoke();
            }
        }
    }
    #endregion
};
