using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DataRefresh : MonoBehaviour
{
    public TextMeshProUGUI data;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        data.text = FindObjectOfType<CurveControll>().dataRealTime.ToString();
    }
}
