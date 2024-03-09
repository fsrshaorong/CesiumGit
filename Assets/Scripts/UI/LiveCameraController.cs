using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class LiveCameraController : MonoBehaviour
{
    Camera cam;
    private IMouseBaseEvent current;
    private IMouseBaseEvent currentDrag;

    private bool isMouseDown;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        //CheckPointerRay();
        //if(current != null)
        //{
        //    print(current.ToString());
        //}
        //else
        //{
        //    print("empty");
        //}
    }

    public void CheckPointerRay(PointerEventData eventData)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Transform hitTransform = hit.transform;
            //子物体没有事件监听的话看看父物体有没有
            while(hitTransform != null && hitTransform.GetComponent<IMouseBaseEvent>() == null)
            {
                hitTransform = hitTransform.parent;
            }
            if (hitTransform?.GetComponent<IMouseBaseEvent>() != null)
            {
                if (current == null)
                {
                    current = hitTransform.GetComponent<IMouseBaseEvent>();
                    current.OnMouseEnter(eventData);
                    //current.OnMouseHover(true);
                }
                else
                {
                    if (hitTransform.GetComponent<IMouseBaseEvent>() != current)
                    {
                        current.OnMouseExit(eventData);
                        current = hitTransform.GetComponent<IMouseBaseEvent>();
                        current.OnMouseEnter(eventData);
                        //current.OnMouseHover(true);
                    }
                    else
                    {
                        current.OnMouseOver(eventData);
                    }
                }
            }
            else
            {
                if (current != null)
                {
                    current.OnMouseExit(eventData);
                    current = null;
                }
            }
        }
        else
        {
            if (current != null)
            {
                current.OnMouseExit(eventData);
                current = null;
            }
        }
        if (currentDrag != null)
        {
            currentDrag.OnDrag(eventData);
        }
    }

    public void MouseDown(PointerEventData eventData)
    {
        isMouseDown = true;
        currentDrag = current;
        if (current != null)
        {
            current.OnMouseDown(eventData);
        }
    }

    public void MouseUp(PointerEventData eventData)
    {
        isMouseDown = false;
        currentDrag = null;
        if (current != null)
        {
            current.OnMouseUp(eventData);
        }
    }

    public void MouseExit(PointerEventData eventData)
    {
        if(current != null)
        {
            current.OnMouseExit(eventData);
        }
        currentDrag = null;
        if (current != null && isMouseDown)
        {
            current.OnMouseUp(eventData);
            isMouseDown = false;
        }
        current = null;
    }
}
