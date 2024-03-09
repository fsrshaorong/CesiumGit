using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventCameraSwitch : MonoBehaviour, IPointerEnterHandler
{
    public static GameObject leftScreen;
    public static GameObject rightScreen;
    public static GameObject mainScreen;

    public static GameObject currentScreen;

    public enum ScreenIndex
    {
        MAIN = 0,
        RIGHT = 1,
        LEFT = 2
    }

    public Camera[] switchTarget;
    public Canvas[] screens;

    public static ScreenIndex GetCurrentScreen()
    {
        //if(currentScreen == leftScreen)
        //{
        //    return ScreenIndex.LEFT;
        //}
        //else if(currentScreen == rightScreen)
        //{
        //    return ScreenIndex.RIGHT;
        //}
        //else
        //{
        //    return ScreenIndex.MAIN;
        //}
        return (ScreenIndex)GetDisplayNumber();
    }

    public static Vector2 GetRelativeMousePosition()
    {
        Vector2 relMousePos;
#if UNITY_EDITOR
        relMousePos = Input.mousePosition;
#else
        // Convert the global mouse position into a relative position for the current display 
        //var mousePosInScreenCoords = Display.RelativeMouseAt(Input.mousePosition);
        Vector3 relMouse = Display.RelativeMouseAt(Input.mousePosition);
        relMousePos = relMouse;
#endif
        return relMousePos;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //foreach (var screen in screens)
        //{
        //    print("½øÈë" + transform.name);
        //    currentScreen = gameObject;
        //    screen.worldCamera = switchTarget[0];
        //};
    }

    void Start()
    {
        if(transform.parent.name == "Actuator Screen")
        {
            leftScreen = gameObject;
        }
        else if(transform.parent.name == "Detector Screen")
        {
            rightScreen = gameObject;
        }
        else if(transform.name == "Big Screen")
        {
            mainScreen = gameObject;
        }
    }

    void Update()
    {
        int displayNumber;

        displayNumber = GetDisplayNumber();

        //if(displayNumber == int.Parse(ScreenIndex.MAIN.ToString()))
        //{

        //}
        //else if (displayNumber == int.Parse(ScreenIndex.RIGHT.ToString()))
        //{

        //}
        //else if (displayNumber == int.Parse(ScreenIndex.LEFT.ToString()))
        //{

        //}
        foreach (var screen in screens)
        {
            screen.worldCamera = switchTarget[displayNumber];
        };
    }

    public static int GetDisplayNumber()
    {
        int displayNumber;
#if UNITY_EDITOR
        displayNumber = Display.activeEditorGameViewTarget;
#else
        // Convert the global mouse position into a relative position for the current display 
        //var mousePosInScreenCoords = Display.RelativeMouseAt(Input.mousePosition);
        Vector3 relMouse = Display.RelativeMouseAt(Input.mousePosition);
        displayNumber = (int)relMouse.z;
#endif
        return displayNumber;
    }
}
