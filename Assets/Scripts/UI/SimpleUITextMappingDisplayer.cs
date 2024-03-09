using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

public class SimpleUITextMappingDisplayer : MonoBehaviour
{
    public string uiKey;
    public string labelKey;

    private DataSupplier dataSupplier;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        dataSupplier = GameObject.FindObjectOfType<DataSupplier>();
        text = GetComponent<Text>();
        dataSupplier.onDataSourceChanged.AddListener(UpdateWithUIConfig);
    }

    void UpdateWithUIConfig(DataSourceChangedEvent e)
    {
        text.text = dataSupplier.getUICustomInfo(uiKey)[labelKey];
    }
}
