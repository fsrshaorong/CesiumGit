using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSizeView : MonoBehaviour, IUILayoutSerializable
{
    public string GetDetailPrefabKey()
    {
        return "UI/EventPanels/ModelSizeView/ModelSizeViewDetail";
    }

    public Rect GetLayoutRect()
    {
        throw new System.NotImplementedException();
    }

    public string GetName()
    {
        throw new System.NotImplementedException();
    }

    public string GetPrefabKey()
    {
        throw new System.NotImplementedException();
    }

    public string GetThumbnailPrefabKey()
    {
        return "UI/EventPanels/ModelSizeView/ModelSizeViewThumbnail";
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject leftScreen = GameObject.FindGameObjectWithTag("LeftScreen");
        GameObject rightScreen = GameObject.FindGameObjectWithTag("RightScreen");

        GameObject detailPanel;

        detailPanel = Instantiate((GameObject)Resources.Load(GetDetailPrefabKey()), leftScreen.transform);
        detailPanel.SetActive(false);
        GetComponent<AbstractEventPanel>().detailLeftScreen = detailPanel;

        detailPanel = Instantiate((GameObject)Resources.Load(GetDetailPrefabKey()), rightScreen.transform);
        detailPanel.SetActive(false);
        GetComponent<AbstractEventPanel>().detailRightScreen = detailPanel;
    }
}
