using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EscMenu : MonoBehaviour
{
    bool isOn = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0.8f, 0, 0.8f); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOn)
            {
                transform.DOScale(new Vector3(0.8f, 0, 0.8f), 0.2f);
            }
            else
            {
                transform.DOScale(0.8f, 0.2f);
            }
            isOn = !isOn;
        }
    }


    public void ClickOut()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//编辑状态下退出
        #else
        Application.Quit();//打包编译后退出
        #endif
    }

    public void ClickCon()
    {
        transform.DOScale(new Vector3(0.8f, 0, 0.8f), 0.2f);
        isOn = false;
    }
}
