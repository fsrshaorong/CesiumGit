using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClickDispatcher : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler, IPointerExitHandler
{
    public LiveCameraController[] cameras;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        foreach (var camera in cameras)
        {
            camera.MouseDown(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (var camera in cameras)
        {
            camera.MouseUp(eventData);
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        //print("mouseOver");
        foreach (var camera in cameras)
        {
            camera.CheckPointerRay(eventData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var camera in cameras)
        {
            camera.MouseExit(eventData);
        }
    }
}
