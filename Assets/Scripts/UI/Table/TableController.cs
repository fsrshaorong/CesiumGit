using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableController : MonoBehaviour
{
    private GridLayoutGroup headerGrid;
    private GridLayoutGroup contentGrid;

    private void Awake()
    {
        headerGrid = transform.Find("Headers").GetComponent<GridLayoutGroup>();
        contentGrid = transform.Find("Viewport").Find("Content").GetComponent <GridLayoutGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHeader(int column, string data)
    {
        int index = column;
        SetCellText(headerGrid.transform.GetChild(index).gameObject, data);
    }

    public void SetContent(int row, int column, string data)
    {
        int index;
        if(contentGrid.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            index = row * contentGrid.constraintCount + column;
        }
        else if(contentGrid.constraint == GridLayoutGroup.Constraint.FixedRowCount)
        {
            index = column * contentGrid.constraintCount +  row;
        }
        else
        {
            index = 0;
            print("table行列是flexible的！暂时不能处理行列数据定位");
            return;
        }
        Transform cell = contentGrid.transform.GetChild(index);
        SetCellText(cell.gameObject, data);
    }

    void SetCellText(GameObject cell, string text)
    {
        TMP_Text textMesh = cell.transform.GetComponentInChildren<TMP_Text>();
        textMesh.text = text;
    }
}
