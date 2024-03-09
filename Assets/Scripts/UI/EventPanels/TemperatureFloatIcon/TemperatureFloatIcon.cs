using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class TemperatureFloatIcon : MonoBehaviour, IUILayoutSerializable
{
    private static GameObject temperatureLeftPanel;
    private static GameObject temperatureRightPanel;

    private Text valueText;
    private bool hasData;

    public string GetDetailPrefabKey()
    {
        return "UI/EventPanels/TemperatureFloatIcon/TemperatureFloatIconDetail";
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
        return "UI/EventPanels/TemperatureFloatIcon/TemperatureFloatIconThumbnail";
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject leftScreen = GameObject.FindGameObjectWithTag("LeftScreen");
        GameObject rightScreen = GameObject.FindGameObjectWithTag("RightScreen");
        //所有按钮共用一个温度面板
        if (temperatureLeftPanel == null)
        {
            GameObject detailLeftPanel = Instantiate((GameObject)Resources.Load(GetDetailPrefabKey()), leftScreen.transform);
            detailLeftPanel.SetActive(false);

            temperatureLeftPanel = detailLeftPanel;
        }

        if (temperatureRightPanel == null)
        {
            GameObject detailRightPanel = Instantiate((GameObject)Resources.Load(GetDetailPrefabKey()), rightScreen.transform);
            detailRightPanel.SetActive(false);

            temperatureRightPanel = detailRightPanel;
        }

        GetComponent<AbstractEventPanel>().detailLeftScreen = temperatureLeftPanel;
        GetComponent<AbstractEventPanel>().detailRightScreen = temperatureRightPanel;

        valueText = transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Text>();

        DropdownEvents dropdown = FindObjectOfType<DropdownEvents>();
        dropdown.onChangeSelectedData.AddListener(DropdownChange);
        DropdownChange(dropdown.selectedData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomNumber()
    {
        if(hasData)
        {
            valueText.text = (Random.value * 100).ToString("f2") + "℃";
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
