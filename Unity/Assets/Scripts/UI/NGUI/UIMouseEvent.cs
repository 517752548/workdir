using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// This script should be attached to each camera that's used to draw the objects with
/// UI components on them. This may mean only one camera (main camera or your UI camera),
/// or multiple cameras if you happen to have multiple viewports. Failing to attach this
/// script simply means that objects drawn by this camera won't receive UI notifications:
/// 
/// - OnHover (isOver) is sent when the mouse hovers over a collider or moves away.
/// - OnPress (isDown) is sent when a mouse button gets pressed on the collider.
/// - OnSelect (selected) is sent when a mouse button is released on the same object as it was pressed on.
/// - OnClick () is sent with the same conditions as OnSelect, with the added check to see if the mouse has not moved much. UICamera.currentTouchID tells you which button was clicked.
/// - OnDoubleClick () is sent when the click happens twice within a fourth of a second. UICamera.currentTouchID tells you which button was clicked.
/// - OnDrag (delta) is sent when a mouse or touch gets pressed on a collider and starts dragging it.
/// - OnDrop (gameObject) is sent when the mouse or touch get released on a different collider than the one that was being dragged.
/// - OnInput (text) is sent when typing (after selecting a collider by clicking on it).
/// - OnTooltip (show) is sent when the mouse hovers over a collider for some time without moving.
/// - OnScroll (float delta) is sent out when the mouse scroll wheel is moved.
/// - OnKey (KeyCode key) is sent when keyboard or controller input is used.
/// </summary>
/// 

public class UIEventBase : MonoBehaviour
{
    public object UserState;
}

public class UIEventArgs : EventArgs
{
    public object UserState { set; get; }
}

public class UIMouseClick : UIEventBase
{

    // Use this for initialization
    void Start()
    {

    }

    void OnClick()
    {
        if (Click != null)
            Click(this, new UIEventArgs() { UserState = this.UserState });
    }

    public EventHandler<UIEventArgs> Click;
}

public class UIMouseDrag : UIEventBase
{
    public void OnDrag(float delta)
    {
        TargetObject = null;
        if (OnDragBegin != null)
        {
            OnDragBegin(this, new UIEventArgs() { UserState = this.UserState });
        }
    }

    public void OnDrop(GameObject obj)
    {
        TargetObject = obj;
        if (OnDragEnd != null)
        {
            OnDragEnd(this, new UIEventArgs() { UserState = this.UserState });
        }
    }

    public GameObject TargetObject { set; get; }

    public EventHandler<UIEventArgs> OnDragBegin;
    public EventHandler<UIEventArgs> OnDragEnd;
}

public class UIMousePress : UIEventBase
{
    public void OnPress(bool isDwon)
    {
        if (Press != null)
        {
            Press(this, new MousePressArgs() { IsDwon = isDwon, UserState = UserState });
        }
    }

    public EventHandler<MousePressArgs> Press;
}

public class MousePressArgs : UIEventArgs
{
    public bool IsDwon { set; get; }
}

public class UIMouseLongPress : UIEventBase
{
    public void OnPress(bool isDwon)
    {
        isPressing = isDwon;
        beginTime = Time.realtimeSinceStartup;
        if (!isDwon)
            eventHadSent = false;
    }

    private bool eventHadSent = false;
    private bool isPressing = false;

    void Update()
    {
        if (isPressing)
        {
            if (eventHadSent) return;
            if (beginTime + LongPressCheckTime <= Time.realtimeSinceStartup)
            {
                eventHadSent = true;
                if (LongPress != null)
                {
                    LongPress(this, new UIEventArgs() { UserState = this.UserState });
                }
            }
        }
    }

    private float beginTime = 0f;

    public static float LongPressCheckTime = 0.85f;

    public EventHandler<UIEventArgs> LongPress;

}

public class MouseTooltipArgs : UIEventArgs
{
    public bool IsShow { set; get; }
}

public class UIMouseTooltip : UIEventBase
{
    public void OnTooltip(bool show)
    {
        Debug.Log(show);
        if (Tooltip != null)
            Tooltip(this, new MouseTooltipArgs { IsShow = show });
    }

    public EventHandler<MouseTooltipArgs> Tooltip;
}


public class MouseHitArgs : UIEventArgs
{
    public bool Hiting { set; get; }
    public Vector3 Position { set; get; }
}