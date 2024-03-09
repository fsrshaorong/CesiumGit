using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExcelDataReader;
using System.IO;
using System.Data;

public class EventController : MonoBehaviour
{
    public Transform model;
    public float max = 1700f;
    public float min = 1300f;
    public string[] tagnames = new string[] {"Temperature", "Pressure", "OilGauge", "OilGaugePivot", "Text", "Cube"};
    Transform[] objects;

    // Start is called before the first frame update
    void Start()
    {
        // ��ȡ��Ҫ���Ƶı������ģ�Ͷ���
        Transform[] charts = GetComponentsInChildren<Transform>();
        Transform[] models = model.GetComponentsInChildren<Transform>();

        // ȥ�������󣻺ϲ����ֶ���Ϊһ�����飻
        objects = new Transform[charts.Length + models.Length - 2];
        for (int i = 0; i < charts.Length - 1; i++)
        {
            objects[i] = charts[i + 1];
        }
        for (int i = charts.Length - 1; i < charts.Length + models.Length - 2; i++)
        {
            objects[i] = models[i - charts.Length + 2];
        }

        // ��ʼЭ�̣���ʱһ���ȡExcel������ݣ�
        StartCoroutine(nameof(IName)); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ValueUpdate()
    {
        try
        {
            // �����쳣�����
            FileStream stream = File.Open(@"D:\Documents\SCUT�о���\07 ������Ҥ��������\���Ա��.xlsx", FileMode.Open, FileAccess.Read);
            IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            var result = excelDataReader.AsDataSet();

            ItemsEditor(objects, result);

            excelDataReader.Close();
            stream.Close();
        }
        catch (IOException)
        {
            // ���������
            // print("�ļ��ѱ���");
        }
    }

    void ItemsEditor(Transform[] items, DataSet excelData)
    {
        // print(items.Length);
        for (int i = 0; i < items.Length; i++)
        {
            Transform item = items[i];
            // print(item.name);
            int rowindex = int.Parse(item.name);
            for (int j = 0; j < tagnames.Length; j++)
            {
                if (item.tag == tagnames[j])
                {
                    switch (j)
                    {
                        case 0: // Temperature��״ͼ����
                            float percentage0 = (float.Parse((excelData.Tables[0].Rows[1][rowindex + 1]).ToString()) - min) / (max - min);
                            item.GetComponent<Image>().fillAmount = percentage0;
                            break;
                        case 1: // Pressure��״ͼ����
                            float percentage1 = (float.Parse((excelData.Tables[0].Rows[2][rowindex + 1]).ToString()) - min) / (max - min);
                            item.GetComponent<Image>().fillAmount = percentage1;
                            break;
                        case 2: // �ͱ�������
                            float percentage2 = float.Parse((excelData.Tables[0].Rows[2][9]).ToString()) * 0.8f / (max - min) + (0.1f - ((0.8f * min) / (max - min)));
                            item.GetComponent<Image>().fillAmount = percentage2;
                            break;
                        case 3: // �ͱ�ָ�뷽�����
                            float percentage3 = float.Parse((excelData.Tables[0].Rows[2][9]).ToString()) * (256f / (min - max)) + (128f + (256f * min / (max - min)));
                            item.transform.localEulerAngles = Vector3.forward * percentage3; //percentage; // (Դ���룺128f - 256 * time)
                            break;
                        case 4: // ��ֵ�������Сֵ���ָ�ֵ
                            item.GetComponent<Text>().text = excelData.Tables[0].Rows[1][rowindex + 9].ToString();
                            break;
                        case 5: // ������ɫ����
                            float percentage = (float.Parse((excelData.Tables[0].Rows[1][rowindex + 1]).ToString()) - min) / (max - min);
                            item.GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.red, Color.yellow, percentage);
                            break;
                    }
                }
            }
        }
    }

    private void OnDisable()
    {
        StopCoroutine(nameof(IName)); // �ر�Э��
    }

    public IEnumerator IName()
    {
        while (true)
        {
            ValueUpdate();
            yield return new WaitForSeconds(1f);//��ʱ1���ټ�������ִ��
        }
    }
}
