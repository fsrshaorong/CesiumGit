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
        // ������λ�ó�����Ļ��3/4�����߿�ʼ���ƣ�
        if (x>2880 && isrolling)
        {
            transform.Translate(Vector3.left * 250f * scale * Time.deltaTime);
        }

        // ���߹�����
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

        // ����ʵʱ״̬ʱ�������Զ�����
        posendline = transform.position.x / scale + (float)x * transform.localScale.x - cam.position.x / scale;
        if ( posendline < 960f)
        {
            isrolling = true;
        }
    }

    void OnDisable()
    {
        StopCoroutine(nameof(IName)); // �ر�Э��
    }
    public IEnumerator IName()
    {
        while (true)
        {
            //DrawAnPoint();
            // ÿn��дһ����
            for (int i = 0; i < dataBase.Length; i++)
            {
                line.positionCount = i + 1;
                x = (float)i * xSpace;
                dataRealTime = dataBase[i];
                line.SetPosition(i, new Vector3((float)i * 250f, dataBase[i], 0));
                // print(posendline); // ������
                yield return new WaitForSeconds(1f); // ��ʱn���ټ�������ִ��
            }
        }
    }
}
