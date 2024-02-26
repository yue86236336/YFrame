using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIPointerEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    UnityEvent<PointerEventData> onPointerEnterEvent = new UnityEvent<PointerEventData>();
    UnityEvent<PointerEventData> onPointerExitEvent = new UnityEvent<PointerEventData>();
    public UnityEvent<PointerEventData> OnPointerEnterEvent { get => onPointerEnterEvent; }
    public UnityEvent<PointerEventData> OnPointerExitEvent { get => onPointerExitEvent; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterEvent?.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitEvent?.Invoke(eventData);
    }
}
