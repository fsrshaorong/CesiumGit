using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatIconMouseEventConvert : MonoBehaviour, IMouseBaseEvent
{
    private AbstractEventPanel abstractEventPanel;
    private IPointerDownHandler pointerDownHandler;
    private CurveSwitch curveSwitch;

    public void OnDrag(PointerEventData eventData)
    {
        //curveSwitch.OnDrag(eventData);
    }

    public void OnMouseDown(PointerEventData eventData)
    {
        pointerDownHandler.OnPointerDown(eventData);
        //curveSwitch.OnMouseDown(eventData);
    }

    public void OnMouseEnter(PointerEventData eventData)
    {
        //curveSwitch.OnMouseEnter(eventData);
    }

    public void OnMouseExit(PointerEventData eventData)
    {
        //curveSwitch?.OnMouseExit(eventData);
    }

    public void OnMouseOver(PointerEventData eventData)
    {
        //curveSwitch.OnMouseOver(eventData);
    }

    public void OnMouseUp(PointerEventData eventData)
    {
        //curveSwitch.OnMouseUp(eventData);
    }

    // Start is called before the first frame update
    void Start()
    {
        abstractEventPanel = GetComponent<AbstractEventPanel>();
        pointerDownHandler = GetComponent<IPointerDownHandler>();
        //curveSwitch = GetComponent<CurveSwitch>();
    }

    // Update is called once per frame
    void Update()
    {
        //print(curveSwitch);
    }


}
