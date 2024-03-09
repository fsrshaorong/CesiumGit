using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 根据dataNames里所有数据项的范围标定数轴上下界，并插值显示
/// </summary>
public class DataSupplierSimpleAxisTextDisplayer : MonoBehaviour
{
    public List<string> dataNames;
    public float interpo = 0;
    public int round = 0;
    public string formatter = "{0}";

    private DataSupplier dataSupplier;
    private Text text;
    private float min;
    private float max;
    // Start is called before the first frame update
    void Start()
    {
        dataSupplier = GameObject.FindObjectOfType<DataSupplier>();
        text = GetComponent<Text>();
        dataSupplier.onDataSourceChanged.AddListener(OnDataSourceChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetMin()
    {
        return min;
    }
    public float GetMax() 
    { 
        return max;
    }

    void OnDataSourceChanged(DataSourceChangedEvent msg)
    {
        min = float.MaxValue;
        max = float.MinValue;
        foreach(var dataName in dataNames)
        {
            //var border = dataSupplier.GetBorder(dataName);
            //min = Mathf.Min(float.Parse(border.Item1), min);
           // max = Mathf.Max(float.Parse(border.Item2), max);
        }
        text.text = String.Format(formatter, Math.Round(min + (max - min) * interpo, round));
    }
}
