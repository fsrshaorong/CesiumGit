using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

interface IMouseBaseEvent
{
    void OnMouseEnter(PointerEventData eventData);
    void OnMouseExit(PointerEventData eventData);
    void OnMouseDown(PointerEventData eventData);
    void OnMouseUp(PointerEventData eventData);
    void OnMouseOver(PointerEventData eventData);
    void OnDrag(PointerEventData eventData);
}
