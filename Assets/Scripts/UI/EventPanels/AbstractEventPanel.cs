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
    public GameObject detailLeftScreen; //详细界面对应的对象的赋值有两种情况：
                                        //1.普通UI界面的EventPanel，这些面板有配置文件，在SaveLayoutToJSON中完成了赋值
                                        //2.其他套用本类的对象没有配置文件，因此在SaveLayoutToJSON中没有处理，一般会在实现IUILayoutSerializable的文件中完成detailScreen对象的初始化和赋值，如Temperature面板套用方式
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
