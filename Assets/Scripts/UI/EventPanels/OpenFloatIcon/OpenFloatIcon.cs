using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class OpenFloatIcon : MonoBehaviour, IUILayoutSerializable
{
    private static GameObject openLeftPanel;
    private static GameObject openRightPanel;

    private Text valueText;
    private bool hasData;

    public string GetDetailPrefabKey()
    {
        return "UI/EventPanels/OpenFloatIcon/OpenFloatIconDetail";
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
        return "UI/EventPanels/OpenFloatIcon/OpenFloatIconThumbnail";
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject leftScreen = GameObject.FindGameObjectWithTag("LeftScreen");
        GameObject rightScreen = GameObject.FindGameObjectWithTag("RightScreen");
        //所有按钮共用一个温度面板
        if (openLeftPanel == null)
        {
            GameObject detailLeftPanel = Instantiate((GameObject)Resources.Load(GetDetailPrefabKey()), leftScreen.transform);
            detailLeftPanel.SetActive(false);

            openLeftPanel = detailLeftPanel;
        }

        if (openRightPanel == null)
        {
            GameObject detailRightPanel = Instantiate((GameObject)Resources.Load(GetDetailPrefabKey()), rightScreen.transform);
            detailRightPanel.SetActive(false);

            openRightPanel = detailRightPanel;
        }

        GetComponent<AbstractEventPanel>().detailLeftScreen = openLeftPanel;
        GetComponent<AbstractEventPanel>().detailRightScreen = openRightPanel;

        valueText = transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Text>();

        DropdownEvents dropdown = FindObjectOfType<DropdownEvents>();
        dropdown.onChangeSelectedData.AddListener(DropdownChange);
        DropdownChange(dropdown.selectedData);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasData)
        {
            //valueText.text = "20";
        }
        else
        {
            valueText.text = "";
        }
    }

    void DropdownChange(DataSource selectedData)
    {
        if (selectedData.type == "null" || selectedData == null)
        {
            hasData = false;
        }
        else
        {
            hasData = true;
        }
    }
}
