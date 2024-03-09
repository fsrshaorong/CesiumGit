using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;
using XChartsDemo;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CurveSwitch : MonoBehaviour, IMouseBaseEvent
{
    public Transform chart;

    GameObject dataBoard;
    bool isActive;
    BaseChart baseChart;
    //Dictionary<int, double> lastValues = new Dictionary<int, double>();

    private void Start()
    {
        dataBoard = transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
        isActive = dataBoard.activeSelf;
    }

    private void Update()
    {
        //baseChart = chart.GetComponent<BaseChart>();
        int i = int.Parse(transform.name);
        if (FromDataToChart.lastValues.ContainsKey(i))
        {
            string value = FromDataToChart.lastValues[i].ToString();
            dataBoard.transform.GetChild(2).GetComponent<Text>().text = value + "¡æ";
        }
    }

    public void OnMouseEnter(PointerEventData eventData)
    {
        transform.localScale = 1.2f * transform.localScale/*new Vector3(1.2f, 1.2f, 1.2f)*/;
        //dataBoard.SetActive(true);
    }

    public void OnMouseExit(PointerEventData eventData)
    {
        transform.localScale = 5.0f / 6.0f * transform.localScale/*new Vector3(1f, 1f, 1f)*/;
        //if (!isActive)
        //{
        //    dataBoard.SetActive(false);
        //}
        //else
        //{
        //    dataBoard.SetActive(true);
        //}
    }

    public void OnMouseDown(PointerEventData eventData)
    {
        //int i = int.Parse(transform.name);
        //var line = baseChart.GetSerie(i);
        //line.show = !line.show;
        //isActive = !isActive;
    }

    public void OnMouseUp(PointerEventData eventData)
    {
        
    }

    public void OnMouseOver(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
