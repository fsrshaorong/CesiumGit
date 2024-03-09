using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBoard : MonoBehaviour
{
    private TableController table;

    private DataSupplier dataSupplier;

    private float lastTime;
    private bool hasData;
    private float cycle = 30.0f;
    // Start is called before the first frame update
    void Start()
    {
        table = GetComponent<TableController>();

        dataSupplier = GameObject.FindGameObjectWithTag("DataSource").GetComponent<DataSupplier>();
        dataSupplier.Listen("输入板项1", SetInput);
        dataSupplier.Listen("输入板项2", SetInput);
        dataSupplier.Listen("输入板项3", SetInput);

        lastTime = Time.time - cycle;

        DropdownEvents dropdown = FindObjectOfType<DropdownEvents>();
        dropdown.onChangeSelectedData.AddListener(DropdownChange);
        DropdownChange(dropdown.selectedData);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastTime >= cycle)
        {
            if(hasData)
            {
                for (int i = 2; i < 3; i++)
                {
                    //table.SetContent(i, 1, Random.Range(0, 100).ToString());
                }
            }
            lastTime = Time.time;
        }
    }

    void DropdownChange(DataSource selectedData)
    {
        if(selectedData.type == "null" || selectedData == null)
        {
            hasData = false;
        }
        else
        {
            lastTime = Time.time - cycle;
            hasData = true;
        }
    }

    void SetInput(DataDeliveryEvent msg)
    {
        if (msg.type == 0)
        {
            switch (msg.name)
            {
                case "输入板项1":
                    table.SetContent(0, 1, Math.Round(double.Parse(msg.data), 3).ToString());
                    break;
                case "输入板项2":
                    table.SetContent(1, 1, Math.Round(double.Parse(msg.data), 3).ToString());
                    break;
                case "输入板项3":
                    table.SetContent(2, 1, Math.Round(double.Parse(msg.data), 3).ToString());
                    break;
            }
        }
    }
}
