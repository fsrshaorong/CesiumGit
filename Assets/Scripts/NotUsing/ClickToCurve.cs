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
            Ray ray = cameraInProducing.ScreenPointToRay(Input.mousePosition);//���߼��
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) // ���߼�⣬������⵽�����嶪��hit��
            {
                // ���detector���飬��������
                Transform cube = hit.collider.transform;  // ��ȡ�����߼��hit�Ķ���(cube��
                Transform detector = cube.parent;    // cube�ĸ��ڵ�
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
