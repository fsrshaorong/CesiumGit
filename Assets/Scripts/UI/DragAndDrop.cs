using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IDragHandler
{
    private RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos;
        Vector3 zeroPos;


        if (RectTransformUtility.ScreenPointToWorldPointInRectangle
        (rt, eventData.delta, eventData.pressEventCamera, out pos) &&
        RectTransformUtility.ScreenPointToWorldPointInRectangle
        (rt, new Vector2(0.0f, 0.0f), eventData.pressEventCamera, out zeroPos))
        {
            rt.position += pos - zeroPos;
        }

    }
}
