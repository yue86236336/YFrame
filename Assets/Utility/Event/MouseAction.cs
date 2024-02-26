
//修改记录：
/****************************************************************************************
2023年11月2日
---添加检测在UI上，禁用触发

2023年11月2日
---添加左键双击事件
---优化拖拽和点击独立运行
****************************************************************************************/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class MouseAction : MonoBehaviour
{
    public UnityAction OnMouseDownAction;
    public UnityAction OnMouseDragAction;
    public UnityAction OnMouseUpAsButtonAction;
    public UnityAction OnMouseEnterAction;
    public UnityAction OnMouseExitAction;
    public UnityAction OnMouseOverAction;
    public UnityAction OnMouseUpAction;
    public UnityAction OnDoubleMouseAsButtonAction;

    private bool interactive = true;
    public bool Interactive
    {
        get { return interactive; }
        set
        {
            interactive = value;
            Collider[] colliders = GetComponents<Collider>();
            foreach (var item in colliders)
            {
                item.enabled = interactive;
            }
        }
    }

    /// <summary>
    /// 左键双击间隔
    /// </summary>
    private static float clickUseInterval = 0.2f;

    /// <summary>
    /// 判断拖拽的阈值 大于阈值不触发点击时间了
    /// </summary>
    private static float dragThreshold = 0.2f;

    /// <summary>
    /// 是否按下
    /// </summary>
    private bool isMouseDown = false;

    /// <summary>
    /// 按下事件
    /// </summary>
    private float mouseDownTime = 0;
    /// <summary>
    /// 拖拽时间
    /// </summary>
    private float dragDistance = 0;

    /// <summary>
    /// 点击量
    /// </summary>
    private int clicksThrough = 0;
    private float clickTime = 0;

    private void Update()
    {
        if (isMouseDown)
        {
            mouseDownTime += Time.deltaTime;
        }

        if (clicksThrough != 0)
        {
            if (Time.time - clickTime >= clickUseInterval)
            {
                // 可执行更多的点击数,如果你手速够快 当前阈值 0.1s 点击时间
                if (clicksThrough == 1)
                {
                    OnMouseUpAsButtonAction?.Invoke();
                }
                else if (clicksThrough == 2)
                {
                    OnDoubleMouseAsButtonAction?.Invoke();
                }
                else
                {
                    //...  可执行更多的点击数,如果你手速够快 当前阈值 0.3s 点击时间
                }
                //重置连击数
                clicksThrough = 0;
            }
        }
    }

    private void OnMouseDown()
    {
        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) return;
        OnMouseDownAction?.Invoke();
        isMouseDown = true;
        mouseDownTime = 0;
        dragDistance = 0;
    }

    private void OnMouseDrag()
    {
        // if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) return;
        OnMouseDragAction?.Invoke();
#if ENABLE_INPUT_SYSTEM
        dragDistance += Mouse.current.delta.ReadValue().magnitude;
#endif
    }

    private void OnMouseUpAsButton()
    {
        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) return;
        //拖拽距离小于拖拽阈值 执行点击
        if (dragDistance < dragThreshold)
        {
            //第一次点击 记录时间
            if (clicksThrough == 0)
            {
                clickTime = Time.time;
            }
            clicksThrough++;
        }
    }

    private void OnMouseEnter()
    {
        //if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) return;
        OnMouseEnterAction?.Invoke();
    }

    private void OnMouseExit()
    {
        // if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) return;
        OnMouseExitAction?.Invoke();
    }
    private void OnMouseOver()
    {
        // if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) return;
        OnMouseOverAction?.Invoke();
    }
    private void OnMouseUp()
    {
        // if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) return;
        OnMouseUpAction?.Invoke();
        isMouseDown = false;
    }

}
