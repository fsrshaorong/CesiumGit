using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ģ���ϵ�ͼ������Ļ�ռ���������ʾ����Ӧλ��
/// </summary>
public class PositionWorldSpaceToScreenSpace : MonoBehaviour, IMouseBaseEvent
{
    
    public Transform marker;
    public float scale = 1.0f;
    public float scaleDuration = 0.2f;

    private Camera camera;
    private RectTransform rectTransform;
    private Canvas canvas;  //������ʾ��ȣ�ͨ��canvas��planeDistance����

    private Vector3 oriScale;

    private float oriDis = 0.0f;

    private bool bigger = false;
    private Vector3 targetScale;

    // Start is called before the first frame update
    void Start()
    {
        camera = transform.parent.GetComponent<Canvas>().worldCamera;
        rectTransform = GetComponent<RectTransform>();
        canvas = transform.parent.GetComponent<Canvas>();
        oriScale = transform.localScale;
        oriDis = (marker.position - camera.transform.position).magnitude;
        //oriScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = camera.WorldToScreenPoint(marker.position);
        rectTransform.anchoredPosition = screenPos;
        canvas.planeDistance = screenPos.z;
        float disToCam = (marker.position - camera.transform.position).magnitude;
        targetScale = scale * (bigger ? 1.2f : 1.0f) * (oriDis / disToCam) * oriScale * Screen.width / ResolutionAdapt.baseUnit;    //������Ļ�ֱ���
        if ((targetScale - transform.localScale).magnitude > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 0.5f);
        }
        //transform.parent.localScale = scale * (180.0f / disToCam) * Vector3.one;
        //transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    [ContextMenu("����Ļ�ռ������������PosMarker����")]
    private void AlignToPosMarker()
    {
        camera = transform.parent.GetComponent<Canvas>().worldCamera;
        rectTransform = GetComponent<RectTransform>();
        canvas = transform.parent.GetComponent<Canvas>();
        Vector3 screenPos = camera.WorldToScreenPoint(marker.position);
        rectTransform.anchoredPosition = screenPos;
        canvas.planeDistance = screenPos.z;
    }

    public void OnMouseEnter(PointerEventData eventData)
    {
        bigger = true;
        transform.parent.parent.GetComponent<IMouseBaseEvent>().OnMouseEnter(eventData);
        //transform.DOBlendableScaleBy(0.2f * oriScale, scaleDuration);
    }

    public void OnMouseExit(PointerEventData eventData)
    {
        bigger = false;
        transform.parent.parent.GetComponent<IMouseBaseEvent>().OnMouseExit(eventData);
        //transform.DOBlendableScaleBy(-0.2f * oriScale, scaleDuration);
    }

    public void OnMouseDown(PointerEventData eventData)
    {
        transform.parent.parent.GetComponent<IMouseBaseEvent>().OnMouseDown(eventData);
    }

    public void OnMouseUp(PointerEventData eventData)
    {
        transform.parent.parent.GetComponent<IMouseBaseEvent>().OnMouseUp(eventData);
    }

    public void OnMouseOver(PointerEventData eventData)
    {
        transform.parent.parent.GetComponent<IMouseBaseEvent>().OnMouseOver(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.parent.parent.GetComponent<IMouseBaseEvent>().OnDrag(eventData);
    }
}
