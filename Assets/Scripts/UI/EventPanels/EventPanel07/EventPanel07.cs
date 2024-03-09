using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPanel07 : MonoBehaviour, IUILayoutSerializable, IAdditionalDataProcessor
{
    public string GetDetailPrefabKey()
    {
        return "UI/EventPanels/EventPanel07/EventPanel07Detail";
    }

    public Rect GetLayoutRect()
    {
        Rect layoutRect = new Rect();
        RectTransform rectTransform = GetComponent<RectTransform>();
        layoutRect.x = rectTransform.localPosition.x;
        layoutRect.y = rectTransform.localPosition.y;
        layoutRect.size = rectTransform.sizeDelta;
        return layoutRect;
    }

    public string GetName()
    {
        return transform.name;
    }

    public string GetPrefabKey()
    {
        return "UI/EventPanels/EventPanel07/EventPanel07";
    }

    public string GetThumbnailPrefabKey()
    {
        return "UI/EventPanels/EventPanel07/EventPanel07DetailThumbnail";
    }

    public void ProcessAdditionalData(Dictionary<string, string> additionalData)
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

