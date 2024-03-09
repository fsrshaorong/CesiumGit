using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovements : MonoBehaviour
{
    public float rotateSpeed = 6f;      //��ת�ٶ�    
    private float axisX = 1;      //�����ˮƽ�����ƶ�������   
    private float axisY = 1;      //�������ֱ�����ƶ�������   
    private float cXY;

    private Vector3 centerrealtime;
    private Vector3 collidercenter;

    // �������ӣ�������Inspector����е���
    public float scaleSpeed = 0.1f;
    // ������С������ֵ��������Inspector����е���
    public float minScale = 0.1f;
    public float maxScale = 4f;

    private void Start()
    {
        collidercenter = transform.GetComponent<BoxCollider>().center;
    }

    void OnMouseDown()
    {
        //������갴�µ��¼�// 
        axisX = 0f; axisY = 0f;
        centerrealtime = transform.position + collidercenter * transform.lossyScale.x;
    }

    void OnMouseDrag()     //�����קʱ�Ĳ���// 
    {
        //centerrealtime = transform.position + collidercenter * transform.lossyScale.x;
        axisX = -Input.GetAxis("Mouse X");
        //���������� 
        axisY = Input.GetAxis("Mouse Y");
        cXY = Mathf.Sqrt(axisX * axisX + axisY * axisY); //��������ƶ��ĳ���//
        if (cXY == 0f) { cXY = 1f; }
        transform.RotateAround(centerrealtime, new Vector3(centerrealtime.x, 0, 0), -axisY * rotateSpeed);
        transform.RotateAround(centerrealtime, new Vector3(0, centerrealtime.y, 0), axisX * rotateSpeed);
    }

    IEnumerator OnMouseOver()
    {
        Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);//��ά��������ת��Ļ����
        //�������Ļ����תΪ��ά���꣬�ټ�������λ�������֮��ľ���
        var offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
        print("down");
        while (Input.GetMouseButton(1))
        {
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
            transform.position = curPosition;
            yield return new WaitForFixedUpdate();
        }

        // ��ȡ�����ֵĹ���ֵ
        float scroll = Input.mouseScrollDelta.y;

        // ����������й���
        if (scroll != 0)
        {
            // ��������ֵ
            float scale = scroll * scaleSpeed;

            // ��ȡ���嵱ǰ������ֵ
            Vector3 currentScale = transform.localScale;

            // �����µ�����ֵ
            Vector3 newScale = currentScale + Vector3.one * scale;

            // �����µ�����ֵ��������С��Χ��
            newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
            newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
            newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);

            // ����������µ�����ֵ
            transform.localScale = newScale;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
