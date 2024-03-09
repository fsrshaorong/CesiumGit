using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToUI : MonoBehaviour
{
    public Camera cameraBigDisplay;
    public GameObject UI1;
    public GameObject UI2;
    bool isOn1;
    bool isOn2;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cameraBigDisplay.ScreenPointToRay(Input.mousePosition);//…‰œﬂºÏ≤‚
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                ClickToChart(hit);
            }
        }

        void ClickToChart(RaycastHit hit)
        {
            if (hit.collider.tag == "Cube")
            {
                isOn1 = UI1.activeSelf;
                if (isOn1)
                {
                    UI1.SetActive(false);
                }
                else
                {
                    UI1.SetActive(true);
                }
            }
            if (hit.collider.tag == "Chart")
            {
                isOn2 = UI2.activeSelf;
                if (isOn2)
                {
                    UI2.SetActive(false);
                }
                else
                {
                    UI2.SetActive(true);
                }
            }
        }
    }
}
