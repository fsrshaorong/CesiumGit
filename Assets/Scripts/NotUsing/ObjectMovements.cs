using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovements : MonoBehaviour
{
    public float rotateSpeed = 6f;      //旋转速度    
    private float axisX = 1;      //鼠标沿水平方向移动的增量   
    private float axisY = 1;      //鼠标沿竖直方向移动的增量   
    private float cXY;

    private Vector3 centerrealtime;
    private Vector3 collidercenter;

    // 缩放因子，可以在Inspector面板中调整
    public float scaleSpeed = 0.1f;
    // 最大和最小的缩放值，可以在Inspector面板中调整
    public float minScale = 0.1f;
    public float maxScale = 4f;

    private void Start()
    {
        collidercenter = transform.GetComponent<BoxCollider>().center;
    }

    void OnMouseDown()
    {
        //接受鼠标按下的事件// 
        axisX = 0f; axisY = 0f;
        centerrealtime = transform.position + collidercenter * transform.lossyScale.x;
    }

    void OnMouseDrag()     //鼠标拖拽时的操作// 
    {
        //centerrealtime = transform.position + collidercenter * transform.lossyScale.x;
        axisX = -Input.GetAxis("Mouse X");
        //获得鼠标增量 
        axisY = Input.GetAxis("Mouse Y");
        cXY = Mathf.Sqrt(axisX * axisX + axisY * axisY); //计算鼠标移动的长度//
        if (cXY == 0f) { cXY = 1f; }
        transform.RotateAround(centerrealtime, new Vector3(centerrealtime.x, 0, 0), -axisY * rotateSpeed);
        transform.RotateAround(centerrealtime, new Vector3(0, centerrealtime.y, 0), axisX * rotateSpeed);
    }

    IEnumerator OnMouseOver()
    {
        Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);//三维物体坐标转屏幕坐标
        //将鼠标屏幕坐标转为三维坐标，再计算物体位置与鼠标之间的距离
        var offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
        print("down");
        while (Input.GetMouseButton(1))
        {
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
            transform.position = curPosition;
            yield return new WaitForFixedUpdate();
        }

        // 获取鼠标滚轮的滚动值
        float scroll = Input.mouseScrollDelta.y;

        // 如果鼠标滚轮有滚动
        if (scroll != 0)
        {
            // 计算缩放值
            float scale = scroll * scaleSpeed;

            // 获取物体当前的缩放值
            Vector3 currentScale = transform.localScale;

            // 计算新的缩放值
            Vector3 newScale = currentScale + Vector3.one * scale;

            // 限制新的缩放值在最大和最小范围内
            newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
            newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
            newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);

            // 设置物体的新的缩放值
            transform.localScale = newScale;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
