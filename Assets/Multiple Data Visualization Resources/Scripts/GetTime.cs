using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
 
public class GetTime : MonoBehaviour
{
    public Text text;

    private DataSupplier supplier;
    private DropdownEvents dropdown;

    // Start is called before the first frame update
    void Start()
    {
        supplier = GameObject.FindObjectOfType<DataSupplier>();
        dropdown = GameObject.FindObjectOfType<DropdownEvents>();
        supplier.Listen("时间戳", OnDataDelivery);
    }

    // Update is called once per frame
    void Update()
    {
        if(dropdown.selectedData.type == "db" || dropdown.selectedData.type == "generated")
        {
            return;
        }
        text.text = System.DateTime.Now.ToString();
    }

    private void OnDataDelivery(DataDeliveryEvent msg)
    {
        if(msg.type == 0)
        {
            if(dropdown.selectedData.type == "db" || dropdown.selectedData.type == "generated")
            {
                TimeZoneInfo tzi = TimeZoneInfo.Local;
                DateTime dt = DateTime.ParseExact(msg.data, "yyyy/MM/dd H:mm:ss", CultureInfo.CurrentCulture);
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(dt, tzi);
                text.text = localTime.ToString();
            }
        }
    }
}
