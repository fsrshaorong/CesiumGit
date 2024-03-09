using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbstractEventPanel : MonoBehaviour, IPointerDownHandler
{
    float lastClick = 0;
    int clickCount = 1;

    const float DOUBLE_CLICK_THRESHOLD = 0.3f;

    public static SwitchContent left;
    public static SwitchContent right;

    [NonSerialized]
    public GameObject detailLeftScreen; //��ϸ�����Ӧ�Ķ���ĸ�ֵ�����������
                                        //1.��ͨUI�����EventPanel����Щ����������ļ�����SaveLayoutToJSON������˸�ֵ
                                        //2.�������ñ���Ķ���û�������ļ��������SaveLayoutToJSON��û�д���һ�����ʵ��IUILayoutSerializable���ļ������detailScreen����ĳ�ʼ���͸�ֵ����Temperature������÷�ʽ
    [NonSerialized]
    public GameObject detailRightScreen;

    private IUILayoutSerializable layoutData;

    private GameObject thumbnailGroup;
    private GameObject thumbnailPanel;
    // Start is called before the first frame update
    void Start()
    {
        lastClick = Time.time;
        clickCount = 1;

        //leftScreen = transform.parent.gameObject.GetComponent<SaveLayoutToJSON>().leftScreen;
        //rightScreen = transform.parent.gameObject.GetComponent<SaveLayoutToJSON>().rightScreen;
        //left = EventCameraSwitch.leftScreen.GetComponent<SwitchContent>();
        //right = EventCameraSwitch.rightScreen.GetComponent<SwitchContent>();

        layoutData = GetComponent<IUILayoutSerializable>();

        //thumbnailGroup = transform.parent.parent.Find("Thumbnail").gameObject;
        //thumbnailGroup = GameObject.FindGameObjectWithTag("Thumbnails");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLeftDetailButtonClick()
    {
        left.SwitchTo(detailLeftScreen);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Time.time - lastClick < DOUBLE_CLICK_THRESHOLD)
        {
            clickCount++;
        }
        else
        {
            clickCount = 1;
            lastClick = Time.time;
        }

        if (clickCount == 2)
        {
            thumbnailPanel = (GameObject)Instantiate(Resources.Load(layoutData.GetThumbnailPrefabKey()), thumbnailGroup.transform);
            thumbnailPanel.GetComponent<AbstractThumbnail>().rootPanel = this;
        }
    }
}
