using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class InputFloatIcon : MonoBehaviour, IUILayoutSerializable
{
    private static GameObject inputLeftPanel;
    private static GameObject inputRightPanel;

    public string GetDetailPrefabKey()
    {
        return "UI/EventPanels/InputFloatIcon/InputFloatIconDetail";
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
        return "UI/EventPanels/InputFloatIcon/InputFloatIconThumbnail";
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject leftScreen = GameObject.FindGameObjectWithTag("LeftScreen");
        GameObject rightScreen = GameObject.FindGameObjectWithTag("RightScreen");
        //所有按钮共用一个温度面板
        if (inputLeftPanel == null)
        {
            GameObject detailLeftPanel = Instantiate((GameObject)Resources.Load(GetDetailPrefabKey()), leftScreen.transform);
            detailLeftPanel.SetActive(false);

            inputLeftPanel = detailLeftPanel;
        }

        if (inputRightPanel == null)
        {
            GameObject detailRightPanel = Instantiate((GameObject)Resources.Load(GetDetailPrefabKey()), rightScreen.transform);
            detailRightPanel.SetActive(false);

            inputRightPanel = detailRightPanel;
        }

        GetComponent<AbstractEventPanel>().detailLeftScreen = inputLeftPanel;
        GetComponent<AbstractEventPanel>().detailRightScreen = inputRightPanel;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
