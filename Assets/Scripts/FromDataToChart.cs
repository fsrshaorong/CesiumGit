
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;
using System.IO;
using Npgsql;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace XChartsDemo
{
    [DisallowMultipleComponent]
    //[ExecuteInEditMode]
    public class FromDataToChart : MonoBehaviour
    {
        public bool tmpData = false;
        public List<string> dataNames;

        public static Dictionary<int, double> lastValues = new Dictionary<int, double>();
        
        private int maxCache;
        private BaseChart chart;
        private float lastTime = 0;
        private int count = 0;
        private string fileForTheOne;

        private Dictionary<int, double[]> data = new Dictionary<int, double[]>();

        private int seriesNum;
        private DataSource lastSelectedData;
        private bool isDrawingFile = false;

        private DataSupplier dataSupplier;
        private Dictionary<string, int> dataNameToSerie;

        private GameObject checkboxContent;
        private List<BlueCheckbox> checkboxes;

        void Awake()
        {
            DropdownEvents dropdown = FindObjectOfType<DropdownEvents>();
            //lastSelectedData = dropdown.selectedData;
            lastSelectedData = new DataSource()
            {
                value = "",
                type = "null"
            };
            dropdown.onChangeSelectedData.AddListener(DropdownSelectedChange);
            chart = GetComponent<BaseChart>();
            InitData();
            DropdownSelectedChange(dropdown.selectedData);

            checkboxContent = transform.Find("SerieVisibilityControl").Find("Viewport").Find("Content").gameObject;

            if (dataNames.Count > 0)
            {
                checkboxes = new List<BlueCheckbox>();
                
                checkboxes.Add(checkboxContent.transform.GetChild(0).GetComponent<BlueCheckbox>());

                checkboxes[0].gameObject.transform.Find("text").GetComponent<Text>().text = dataNames[0];
                checkboxes[0].checkboxOn.AddListener(EnableSerie);
                checkboxes[0].checkboxOff.AddListener(DisableSerie);
                for (int i = 1; i < dataNames.Count; i++)
                {
                    GameObject go = GameObject.Instantiate(checkboxes[0].gameObject, checkboxContent.transform);
                    go.transform.Find("text").GetComponent<Text>().text = dataNames[i];
                    checkboxes.Add(go.GetComponent<BlueCheckbox>());
                    checkboxes[i].checkboxOn.AddListener(EnableSerie);
                    checkboxes[i].checkboxOff.AddListener(DisableSerie);
                }
            }
            else
            {
                checkboxContent.SetActive(false);
            }

            //LoadDatabaseData();
            //print("asynctest");
        }

        private void Start()
        {
            dataSupplier = GameObject.FindObjectOfType<DataSupplier>();
            dataNameToSerie = new Dictionary<string, int>();
            for(int i = 0; i < dataNames.Count; i++)
            {
                dataNameToSerie[dataNames[i]] = i;
                dataSupplier.Listen(dataNames[i], OnDataDelivery);
            }
        }

        private void Update()
        {
            //if (Time.time - lastTime >= 1f && isDrawingFile && lastSelectedData.type == "file")
            //{
            //    lastTime = Time.time;
            //    AddOneData(count++);
            //}
        }

        void InitData() //初始化表格，删除所有默认是series，添加指定数量series
        {
            chart.ClearSerieData();
            chart.RemoveData();
        }

        void WriteDataDictionary() //将文件数据写入字典
        {
            if (/*transform.name == "Actuator Data"*/ transform.parent.parent.parent.name == "Actuator Screen" || tmpData)
            {
                fileForTheOne = lastSelectedData.name + "_parameter.txt";
            }
            if (transform.parent.parent.parent.name == "Detector Screen" || tmpData)
            {
                fileForTheOne = lastSelectedData.name + "_output.txt";
            }
            data.Clear();
            //string fileInData = Application.dataPath + "../BackEnd/" + fileForTheOne;
            string fileInData = Path.Combine(Path.Combine(Application.dataPath, "../../BackEnd"), fileForTheOne);
            string[] strs = File.ReadAllLines(fileInData);
            maxCache = strs.Length;
            //if (maxCache > 30000)
            //{
            //    maxCache = 30000;
            //}
            chart.SetMaxCache(maxCache);

            for (int i = 0; i < maxCache; i++)
            {
                string[] str = strs[i].Split(",");
                seriesNum = str.Length;
                for (int j = 0; j < str.Length; j++)
                {
                    if (!data.ContainsKey(j))
                    {
                        double[] dataOfOneParameter = new double[maxCache];
                        data.Add(j, dataOfOneParameter);
                    }
                    data[j][i] = double.Parse(str[j]);
                }
            }

            //增加与数据表匹配的series
            for (int i = 0; i < seriesNum; i++)
            {
                string serieName = "参数0" + (i + 1).ToString();
                chart.AddSerie<Line>(serieName);
                Serie serie = chart.GetSerie(i);
                serie.symbol.show = false;
                serie.lineType = LineType.Smooth;
                //serie.animation.enable = true;
                //serie.animation.dataChangeEnable = true;
                //serie.animation.type = AnimationType.AlongPath;
                //serie.animation.dataChangeDuration = 500f;
            }
        }

        void AddOneData(int i = -1)
        {
            if (i >= 0 && i < maxCache)
            {
                foreach (var j in data.Keys)
                {
                    lastValues[j] = data[j][i];
                    chart.AddData(j, lastValues[j]);
                }
                chart.AddXAxisData(i.ToString());
            }
        }

        void RemoveOneData(int i = -1)
        {
            if(i >= 0)
            {
                foreach(var j in data.Keys)
                {
                    
                }
            }
        }

        void DropdownSelectedChange(DataSource selectedData)
        {
            if (selectedData.type == "null" || selectedData == null)
            {
                count = 0;
                InitData();
                //WriteDataDictionary();
                chart.EnsureChartComponent<Title>().subText = "请选择数据源";
                isDrawingFile = false;
            }
            else if (selectedData.name != lastSelectedData.name)
            {
                lastSelectedData = selectedData;
                if (selectedData.type == "db" || selectedData.type == "file" || selectedData.type == "generated")
                {
                    count = 0;
                    InitData();
                    isDrawingFile = false;
                    for (int i = 0; i < dataNames.Count; i++)
                    {
                        string serieName = dataNames[i];
                        chart.AddSerie<Line>(serieName);
                        Serie serie = chart.GetSerie(i);
                        serie.symbol.show = false;
                        serie.lineType = LineType.Normal;
                        //serie.animation.enable = true;
                        //serie.animation.dataChangeEnable = true;
                        //serie.animation.type = AnimationType.AlongPath;
                        //serie.animation.dataChangeDuration = 500f;
                    }
                    chart.EnsureChartComponent<Title>().subText = lastSelectedData.name;
                }
                else
                {
                    //count = 0;
                    //InitData();
                    //WriteDataDictionary();
                    //chart.EnsureChartComponent<Title>().subText = selectedData.name.Replace(".txt", "");
                    //isDrawingFile = true;
                }
            }
            //lastSelectedData = selectedData;
        }

        void OnDataDelivery(DataDeliveryEvent msg)
        {
            if(/*!isDrawingFile && lastSelectedData.type == "db"*/lastSelectedData.type != "null")
            {
                chart.AddData(dataNameToSerie[msg.name], Math.Round(double.Parse(msg.data), 3));
                if(count % dataNames.Count == 0)
                {
                    chart.AddXAxisData((count / dataNames.Count).ToString());
                }
                count++;
            }
        }

        void EnableSerie(BlueCheckbox sender)
        {
            int serieIndex = sender.transform.GetSiblingIndex();
            chart.GetSerie(serieIndex).show = true;
        }

        void DisableSerie(BlueCheckbox sender)
        {
            int serieIndex = sender.transform.GetSiblingIndex();
            chart.GetSerie(serieIndex).show = false;
        }
    }
}
