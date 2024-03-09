using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToCurve : MonoBehaviour
{
    public Camera cameraInProducing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cameraInProducing.ScreenPointToRay(Input.mousePosition);//射线检测
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) // 射线检测，并将检测到的物体丢到hit上
            {
                // 点击detector方块，开关曲线
                Transform cube = hit.collider.transform;  // 获取到射线检测hit的对象(cube）
                Transform detector = cube.parent;    // cube的父节点
                LineRenderer curve = detector.GetComponentInChildren<LineRenderer>();
                bool isOn = curve.enabled == true;
                if (isOn)
                {
                    curve.enabled = false;
                }
                else
                {
                    curve.enabled = true;
                }

            }
        }
    }
}
