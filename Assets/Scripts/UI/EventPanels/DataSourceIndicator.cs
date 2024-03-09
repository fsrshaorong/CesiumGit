using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataSourceIndicator : MonoBehaviour
{
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<Text>();
        DropdownEvents dropdown = FindObjectOfType<DropdownEvents>();
        dropdown.onChangeSelectedData.AddListener(DropdownChange);
        DropdownChange(dropdown.selectedData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DropdownChange(DataSource selectedData)
    {
        if(selectedData.type == "null" || selectedData == null)
        {
            text.text = "��ѡ������Դ";
        }
        else
        {
            text.text = "����Դ:" + selectedData.name;
        }
    }
}
