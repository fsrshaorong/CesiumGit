using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ParameterSetup : MonoBehaviour
{
    // 获取画布对象
    public RectTransform bigSceen;
    public RectTransform background;
    
    // 获取可调参数名字数组
    public Dictionary<string, float> paraDic = new Dictionary<string, float>();
    public string[] parameters;

    // 获取可调参数模型
    FlameSwitch FlameSwitch;
    SandMaterialContrl SandMaterialContrl;

    // 输入条暂存
    GameObject tempInputBar;

    // 输入区域数组
    TMP_InputField[] inputFields;

    // Start is called before the first frame update
    void Start()
    {
        // 将可调参数名字数组写入字典
        foreach (var parameter in parameters)
        {
            if (!paraDic.ContainsKey(parameter))
            {
                paraDic.Add(parameter, 0f);
            }
        }
        InitMenu();

        // 为每个输入区域添加监听，对应关系：参数parameters[]顺序对应
        inputFields = transform.GetComponentsInChildren<TMP_InputField>();
        for (int i = 0; i < inputFields.Length; i++)
        {
            int x = i;
            //print(inputFields[i].name);
            inputFields[i].GetComponent<TMP_InputField>().onDeselect.AddListener((string value) => { ValueUpadate(x, value); });
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ValueUpadate(int index, string value)
    {
        FlameSwitch = GameObject.Find("Flames").GetComponent<FlameSwitch>();
        SandMaterialContrl = GameObject.Find("Sand").GetComponent<SandMaterialContrl>();

        paraDic[parameters[index]] = float.Parse(value);
        print(parameters[index] + "：" + paraDic[parameters[index]]);

        switch (index)
        {
            case 0:        //投料速度
                break;
            case 1:        //投料时长
                break;
            case 2:        //投料间隔
                break;
            case 3:        //推料速度
                SandMaterialContrl.feedSpeed = float.Parse(value);
                break;
            case 4:        //推料间隔
                SandMaterialContrl.feedInterval = float.Parse(value);
                break;
            case 5:        //喷火时长
                FlameSwitch.flameDuration = float.Parse(value);
                break;
            case 6:        //喷火间隔
                FlameSwitch.flameInterval = float.Parse(value);
                break;
            case 7:        //熔液流速
                break;
            case 8:        //搅拌速度
                break;
        }
    }

    void InitMenu()
    {   //根据参数的设置背景的大小;
        float width = bigSceen.sizeDelta.x * 0.3f;
        float hight = paraDic.Count * 120f;
        background.sizeDelta = new Vector2(width, hight);


        //根据参数个数生成对应数量的文字对象、输入栏；
        //设置各组件位置；
        float topgap = 0.25f;
        float buttongap = 0.1f;
        float yoffset = background.sizeDelta.y * (0.5f - topgap);
        foreach (var para in paraDic)
        {
            tempInputBar = Instantiate(Resources.Load("Input Bar")) as GameObject;
            Transform temptrans = tempInputBar.transform;
            temptrans.SetParent(background);
            //temptrans.localScale = new Vector3(1, 1, 1);
            temptrans.GetComponent<RectTransform>().sizeDelta = new Vector2(background.sizeDelta.x * 0.5f, 50f);
            temptrans.localPosition = new Vector2(0, yoffset);
            TMP_Text paratext = temptrans.GetChild(0).GetComponent<TMP_Text>();
            paratext.text = para.Key;
            paratext.GetComponent<RectTransform>().sizeDelta -= new Vector2(0.08f * temptrans.GetComponent<RectTransform>().sizeDelta.x, 0);
            RectTransform inputField = temptrans.GetChild(1).GetComponent<RectTransform>();
            inputField.sizeDelta = new Vector2(0.7f * temptrans.GetComponent<RectTransform>().sizeDelta.x, 50f);
            yoffset -= background.sizeDelta.y * ((1f - topgap - buttongap) / (paraDic.Count - 1));
        }
    }
}
