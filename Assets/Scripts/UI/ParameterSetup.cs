using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ParameterSetup : MonoBehaviour
{
    // ��ȡ��������
    public RectTransform bigSceen;
    public RectTransform background;
    
    // ��ȡ�ɵ�������������
    public Dictionary<string, float> paraDic = new Dictionary<string, float>();
    public string[] parameters;

    // ��ȡ�ɵ�����ģ��
    FlameSwitch FlameSwitch;
    SandMaterialContrl SandMaterialContrl;

    // �������ݴ�
    GameObject tempInputBar;

    // ������������
    TMP_InputField[] inputFields;

    // Start is called before the first frame update
    void Start()
    {
        // ���ɵ�������������д���ֵ�
        foreach (var parameter in parameters)
        {
            if (!paraDic.ContainsKey(parameter))
            {
                paraDic.Add(parameter, 0f);
            }
        }
        InitMenu();

        // Ϊÿ������������Ӽ�������Ӧ��ϵ������parameters[]˳���Ӧ
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
        print(parameters[index] + "��" + paraDic[parameters[index]]);

        switch (index)
        {
            case 0:        //Ͷ���ٶ�
                break;
            case 1:        //Ͷ��ʱ��
                break;
            case 2:        //Ͷ�ϼ��
                break;
            case 3:        //�����ٶ�
                SandMaterialContrl.feedSpeed = float.Parse(value);
                break;
            case 4:        //���ϼ��
                SandMaterialContrl.feedInterval = float.Parse(value);
                break;
            case 5:        //���ʱ��
                FlameSwitch.flameDuration = float.Parse(value);
                break;
            case 6:        //�����
                FlameSwitch.flameInterval = float.Parse(value);
                break;
            case 7:        //��Һ����
                break;
            case 8:        //�����ٶ�
                break;
        }
    }

    void InitMenu()
    {   //���ݲ��������ñ����Ĵ�С;
        float width = bigSceen.sizeDelta.x * 0.3f;
        float hight = paraDic.Count * 120f;
        background.sizeDelta = new Vector2(width, hight);


        //���ݲ����������ɶ�Ӧ���������ֶ�����������
        //���ø����λ�ã�
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
