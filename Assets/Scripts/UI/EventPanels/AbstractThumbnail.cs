using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbstractThumbnail : MonoBehaviour, IPointerUpHandler, IPointerClickHandler, IPointerDownHandler
{
    [NonSerialized]
    public AbstractEventPanel rootPanel;

    private RectTransform mainScreenRect;

    private Transform thumbnails;

    private Vector3 originScale;

    public void OnPointerClick(PointerEventData eventData)
    {
        //print("clickTest");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //print("pdtest");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //print("puptest");
        EventCameraSwitch.ScreenIndex screen = EventCameraSwitch.GetCurrentScreen();
        SwitchContent targetChart;
        GameObject detailPanel;
        bool project = false;
        if (screen == EventCameraSwitch.ScreenIndex.RIGHT)
        {
            project = true;
            targetChart = AbstractEventPanel.right;
            detailPanel = rootPanel.detailRightScreen;
        }
        else if (screen == EventCameraSwitch.ScreenIndex.LEFT)
        {
            project = true;
            //AbstractEventPanel.left.SwitchTo(rootPanel.detailLeftScreen);
            targetChart = AbstractEventPanel.left;
            detailPanel = rootPanel.detailLeftScreen;
        }
        else
        {
            Vector2 relMouse = EventCameraSwitch.GetRelativeMousePosition();
            //print(relMouse);
            if (relMouse.x < 0.0f)
            {
                project = true;
                targetChart = AbstractEventPanel.left;
                detailPanel = rootPanel.detailLeftScreen;
            }
            else if(relMouse.x > Screen.width)
            {
                project = true;
                targetChart = AbstractEventPanel.right;
                detailPanel = rootPanel.detailRightScreen;
            }
            else
            {
                project = false;
                targetChart = AbstractEventPanel.left;  //注释掉这行编译器报错，实际上没有这行也不会进入下面的条件块
                detailPanel = rootPanel.detailLeftScreen;
            }
        }

        if (project)
        {
            transform.DOBlendableMoveBy(targetChart.transform.position - transform.position, 0.5f);
            transform.DOBlendableScaleBy(targetChart.transform.lossyScale.x / transform.lossyScale.x * transform.localScale - transform.localScale, 0.5f).onComplete =
            () =>
            {
                targetChart.SwitchTo(detailPanel);
                gameObject.SetActive(false);
                Destroy(gameObject);
            };
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mainScreenRect = EventCameraSwitch.mainScreen.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DestroyInTween();
        }
    }

    private void Awake()
    {
        originScale = transform.localScale;
        transform.localScale = new Vector3(originScale.x, 0.0f, originScale.z);
        thumbnails = GameObject.FindGameObjectWithTag("Thumbnails").transform;
    }

    void OnEnable()
    {
        transform.DOScale(originScale, 0.2f);
        if (thumbnails.childCount > 1)
        {
            for (int i = 0; i < thumbnails.childCount; i++)
            {
                Transform child = thumbnails.GetChild(i);
                if(child != transform)
                {
                    child.gameObject.GetComponent<AbstractThumbnail>().DestroyInTween();
                }
            }
        }
    }

    public void DestroyInTween()
    {
        transform.DOScale(new Vector3(originScale.x, 0.0f, originScale.z), 0.2f).onComplete =
        () =>
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        };
    }
}
