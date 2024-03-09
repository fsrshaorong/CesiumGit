using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SerializePanelEventPanel01 : MonoBehaviour, IUILayoutSerializable, IAdditionalDataProcessor
{
    public string GetDetailPrefabKey()
    {
        return "UI/EventPanels/EventPanel01/EventPanel01Detail";
    }

    public string GetThumbnailPrefabKey()
    {
        return "UI/EventPanels/EventPanel01/EventPanel01DetailThumbnail";
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
        return "UI/EventPanels/EventPanel01/EventPanel01";
    }

    public void ProcessAdditionalData(Dictionary<string, string> additionalData)
    {
        print("��������:" + additionalData["extra"]);
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
