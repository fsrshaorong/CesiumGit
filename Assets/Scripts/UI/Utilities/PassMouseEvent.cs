using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class PassMouseEvent : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public bool notifyAll = false;

    private GameObject currentDrag;

    private HashSet<GameObject> lastOver = new HashSet<GameObject>();
    private HashSet<GameObject> currentOver = new HashSet<GameObject>();

    /// <summary>
    /// ��������¼����ݵ��²� UI �� GameObject
    /// </summary>
    private void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function, bool all = false, bool updateCurrentOver = false)
        where T : IEventSystemHandler
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        var current = data.pointerCurrentRaycast.gameObject;
        if(updateCurrentOver)
        {
            currentOver.Clear();
        }
        foreach (var t in results.Where(t => current != t.gameObject))
        {
            if(updateCurrentOver)
            {
                currentOver.Add(t.gameObject);
            }
            ExecuteEvents.Execute(t.gameObject, data, function);
            //RaycastAll��ugui���Լ����������ֻ����Ӧ͸��ȥ�������һ����Ӧ������ExecuteEvents.Execute��ֱ��break���С�
            if (!all)
            {
                break;
            }
        }
    }

    //��������
    public void OnPointerDown(PointerEventData eventData)
    {
        currentDrag = eventData.selectedObject;
        PassEvent(eventData, ExecuteEvents.pointerDownHandler, notifyAll);
    }

    //����̧��
    public void OnPointerUp(PointerEventData eventData)
    {
        currentDrag = null;
        PassEvent(eventData, ExecuteEvents.pointerUpHandler, true);
    }

    //�������
    public void OnPointerClick(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.submitHandler, notifyAll);
        PassEvent(eventData, ExecuteEvents.pointerClickHandler, notifyAll);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        lastOver.Clear();
        lastOver.UnionWith(currentOver);
        PassEvent(eventData, ExecuteEvents.pointerMoveHandler, notifyAll, true);
        //print("lastOver:");
        //foreach (var t in lastOver)
        //{
        //    print(t.transform.name);
        //}
        //print("currentOver:");
        foreach (var t in currentOver)
        {
            print(t.transform.name);
        }
        foreach (var t in lastOver)
        {
            if(!currentOver.Contains(t))
            {
                //print("ExitBG");
                ExecuteEvents.Execute(t, eventData, ExecuteEvents.pointerExitHandler);
            }
        }
        foreach(var t in currentOver)
        {
            if(!lastOver.Contains(t))
            {
                ExecuteEvents.Execute(t, eventData, ExecuteEvents.pointerEnterHandler);
            }
        }
    }
}
