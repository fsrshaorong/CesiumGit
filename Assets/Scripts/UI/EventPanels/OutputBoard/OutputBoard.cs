using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class OutputBoard : MonoBehaviour
{
    private TableController table;

    private DataSupplier dataSupplier;

    private ScrollRect scrollRect;

    private float lastTime;
    private float cycle = 30.0f;

    private bool hasData;
    // Start is called before the first frame update
    void Start()
    {
        table = GetComponent<TableController>();

        scrollRect = GetComponent<ScrollRect>();

        dataSupplier = GameObject.FindGameObjectWithTag("DataSource").GetComponent<DataSupplier>();
        dataSupplier.Listen("输出板项1", SetInput);
        dataSupplier.Listen("输出板项2", SetInput);
        dataSupplier.Listen("输出板项3", SetInput);
        dataSupplier.Listen("输出板项4", SetInput);
        dataSupplier.Listen("输出板项5", SetInput);
        dataSupplier.Listen("输出板项6", SetInput);

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
                for (int i = 0; i < 3; i++)
                {
                    table.SetContent(i, 1, UnityEngine.Random.Range(0, 100).ToString());
                }
            }
            lastTime = Time.time;
        }

        scrollRect.normalizedPosition = new Vector2(scrollRect.normalizedPosition.x, (-Time.time * 0.3f) % 1.0f + 1);
    }
    void DropdownChange(DataSource selectedData)
    {
        if (selectedData.type == "null" || selectedData == null)
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
            int i = 0;
            switch(msg.name)
            {
                case "输出板项1":
                    table.SetContent(0, 1, Math.Round(double.Parse(msg.data), 3).ToString());
                    break;
                case "输出板项2":
                    table.SetContent(1, 1, Math.Round(double.Parse(msg.data), 3).ToString());
                    break;
                case "输出板项3":
                    table.SetContent(2, 1, Math.Round(double.Parse(msg.data), 3).ToString());
                    break;
                case "输出板项4":
                    table.SetContent(3, 1, Math.Round(double.Parse(msg.data), 3).ToString());
                    break;
                case "输出板项5":
                    table.SetContent(4, 1, Math.Round(double.Parse(msg.data), 3).ToString());
                    break;
                case "输出板项6":
                    table.SetContent(5, 1, Math.Round(double.Parse(msg.data), 3).ToString());
                    break;
            }
        }
    }
}
