using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class DataSupplierTextSimpleDisplayer : MonoBehaviour
{
    public string dataName;
    public int round = 3;
    public string formatter = "{0}";

    private DataSupplier dataSupplier;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        dataSupplier = GameObject.FindObjectOfType<DataSupplier>();
        text = GetComponent<Text>();
        dataSupplier.Listen(dataName, OnDataDelivery);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDataDelivery(DataDeliveryEvent msg)
    {
        if(msg.type == 0)
        {
            text.text = String.Format(formatter, Math.Round(double.Parse(msg.data), round).ToString());
        }
    }
}
