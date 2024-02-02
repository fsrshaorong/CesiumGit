using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovements : MonoBehaviour, IMouseBaseEvent
{
    //public Transform target;//��ȡ��תĿ��
    public Transform Camera;
    public float rotateSpeed = 5f;
    public float translateSpeed = 700f;
    public float zoomSpeed = 20f;

    private bool isBigScreen;
    private float duration = 0.5f;
    private Vector3 center;
    private Vector3 posBig = new(-100f, 150f, -100f);
    private Vector3 rota = new(50f, 0f, 0f);
    private Vector3 posNorm = new(-100f, 110f, -40f);

    private float minDisBig;
    private float minDisNorm;
    private float maxDisBig = 150f;
    private float maxDisNorm = 100f;

    private float disRealtime;

    private bool mouseDown = false;

    void Start()
    {
        center = transform.GetComponent<BoxCollider>().center;
        minDisBig = (posBig - center).magnitude;
        //print(minDisBig);
        minDisNorm = (posNorm - center).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        isBigScreen = FindObjectOfType<ScreenSwitch>().withCharts;
        //camerarotate();
        //camerazoom();
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isBigScreen)
            {
                Camera.DOMove(posBig, duration);
                Camera.DOLocalRotate(rota, duration);
            }
            else
            {
                Camera.DOMove(posNorm, duration);
                Camera.DOLocalRotate(rota, duration);
            }
        }
        disRealtime = (Camera.position - center).magnitude;
    }

    public void OnMouseOver()
    {
        //cameratranslate();
        camerazoom();
        //if(mouseDown)
        //{
        //    camerarotate();
        //}
    }

    public void OnMouseDrag()
    {
        //camerarotate();
    }

    private void cameratranslate() //�����Χ��Ŀ����ת����
    {
        //transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime); //�����Χ��Ŀ����ת
        var mouse_x = Input.GetAxis("Mouse X");//��ȡ���X���ƶ�
        var mouse_y = -Input.GetAxis("Mouse Y");//��ȡ���Y���ƶ�

        if (Input.GetKey(KeyCode.Mouse1))
        {
            Camera.Translate(Vector3.left * (mouse_x * translateSpeed) * Time.deltaTime);
            Camera.Translate(Vector3.up * (mouse_y * translateSpeed) * Time.deltaTime);
        }

    }

    private void camerarotate() //�����Χ��Ŀ����ת����
    {
        //transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime); //�����Χ��Ŀ����ת
        var mouse_x = Input.GetAxis("Mouse X");//��ȡ���X���ƶ�
        var mouse_y = -Input.GetAxis("Mouse Y");//��ȡ���Y���ƶ�

        Camera.RotateAround(center, Vector3.up, mouse_x * rotateSpeed);
        //Camera.RotateAround(center, transform.right, mouse_y * rotateSpeed);

    }

    private void camerazoom() //�������������
    {
        if (isBigScreen)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && disRealtime > maxDisBig)
            {
                Camera.Translate(Vector3.forward * zoomSpeed);
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0 && disRealtime < minDisBig)
            {
                Camera.Translate(Vector3.forward * -zoomSpeed);
            }
        }

        if (!isBigScreen)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && disRealtime >maxDisNorm)
            {
                Camera.Translate(Vector3.forward * zoomSpeed);
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0 && disRealtime < minDisNorm)
            {
                Camera.Translate(Vector3.forward * -zoomSpeed);
            }
        }
    }

    public void OnMouseEnter()
    {
        
    }

    public void OnMouseExit()
    {
        mouseDown = false;
    }

    public void OnMouseDown()
    {
        mouseDown = true;
    }

    public void OnMouseUp()
    {
        mouseDown = false;
    }

    public void OnDrag()
    {
        camerarotate();
    }
}