using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class OutputFloatIcon : MonoBehaviour, IUILayoutSerializable
{
    private static GameObject outputLeftPanel;
    private static GameObject outputRightPanel;

    public string GetDetailPrefabKey()
    {
        return "UI/EventPanels/OutputFloatIcon/OutputFloatIconDetail";
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
        return "UI/EventPanels/OutputFloatIcon/OutputFloatIconThumbnail";
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject leftScreen = GameObject.FindGameObjectWithTag("LeftScreen");
        GameObject rightScreen = GameObject.FindGameObjectWithTag("RightScreen");
        //所有按钮共用一个温度面板
        if (outputLeftPanel == null)
        {
            GameObject detailLeftPanel = Instantiate((GameObject)Resources.Load(GetDetailPrefabKey()), leftScreen.transform);
            detailLeftPanel.SetActive(false);

            outputLeftPanel = detailLeftPanel;
        }

        if (outputRightPanel == null)
        {
            GameObject detailRightPanel = Instantiate((GameObject)Resources.Load(GetDetailPrefabKey()), rightScreen.transform);
            detailRightPanel.SetActive(false);

            outputRightPanel = detailRightPanel;
        }

        GetComponent<AbstractEventPanel>().detailLeftScreen = outputLeftPanel;
        GetComponent<AbstractEventPanel>().detailRightScreen = outputRightPanel;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
