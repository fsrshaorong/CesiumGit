using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using DG.Tweening;

public class CurveControll : MonoBehaviour
{
    public Transform cam;
    public GameObject curve;
    public float xSpace;
    public float rollspeed;
    public Vector3 scalechange = new Vector3(0.1f, 0f, 0f);
    public int datacount;
    public bool isrolling = true;
    public float dataRealTime;
    float x;
    float scale;
    float posendline;
    float[] dataBase;

    LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        dataBase = new float[datacount];
        for (int i = 0; i < datacount; i++)
        {
            dataBase[i] = Random.Range(50f, 1900f);
        }
        dataRealTime = dataBase[0];
        line = curve.GetComponent<LineRenderer>();
        line.positionCount = 0;
        scale = line.GetComponentInParent<RectTransform>().transform.lossyScale.x;
        StartCoroutine(nameof(IName));
    }

    // Update is called once per frame
    void Update()
    {
        // 坐标点的位置超过屏幕的3/4后，曲线开始右移；
        if (x>2880 && isrolling)
        {
            transform.Translate(Vector3.left * 250f * scale * Time.deltaTime);
        }

        // 曲线滚动；
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                isrolling = false;
                transform.localScale += scalechange;
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                isrolling = false;
                transform.Translate(Vector3.right * rollspeed * scale);
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                isrolling = false;
                transform.localScale -= scalechange;
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                isrolling = false;
                transform.Translate(Vector3.right * -rollspeed * scale);
            }

        }

        // 滚回实时状态时，继续自动滚动
        posendline = transform.position.x / scale + (float)x * transform.localScale.x - cam.position.x / scale;
        if ( posendline < 960f)
        {
            isrolling = true;
        }
    }

    void OnDisable()
    {
        StopCoroutine(nameof(IName)); // 关闭协程
    }
    public IEnumerator IName()
    {
        while (true)
        {
            //DrawAnPoint();
            // 每n秒写一个点
            for (int i = 0; i < dataBase.Length; i++)
            {
                line.positionCount = i + 1;
                x = (float)i * xSpace;
                dataRealTime = dataBase[i];
                line.SetPosition(i, new Vector3((float)i * 250f, dataBase[i], 0));
                // print(posendline); // 测试用
                yield return new WaitForSeconds(1f); // 延时n秒再继续向下执行
            }
        }
    }
}
