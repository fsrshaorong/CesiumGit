
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;
using System.IO;

namespace XChartsDemo
{
    [DisallowMultipleComponent]
    //[ExecuteInEditMode]
    public class Simplified00_SimplifiedLineChart_ReadData : MonoBehaviour
    {
        [SerializeField] 
        private int maxCache = 0;
        private BaseChart chart;
        private float lastTime = 0;
        private int count = 0;
        private Dictionary<int, double> lastValues = new Dictionary<int, double>();
        private Dictionary<int, double[]> data = new Dictionary<int, double[]>();
        void Awake()
        {
            chart = GetComponent<BaseChart>();
            chart.SetMaxCache(maxCache);
            //chart.RemoveAllSerie();
            InitData();
        }

        private void Update()
        {
            if (count >= maxCache)
            {

                if (Time.time - lastTime >= 0.2f)
                {
                    lastTime = Time.time;
                    //AddOneData(count++);
                }
            }
            else
            {
                for (int i = 0; i < 50; i++)
                {
                   // AddOneData(count++);
                }
            }
        }

        void InitData()
        {
            var serie = chart.GetSerie(0);
            serie.symbol.show = false;
           
            string[] strs = File.ReadAllLines("Assets\\Data\\parameter.txt");
            maxCache = strs.Length;
            if (maxCache > 30000)
            {
                maxCache = 30000;
            }
            chart.SetMaxCache(maxCache);
            //chart.AddSerie<SimplifiedLine>();

            
            int data_num = 0;
            for (int i = 0; i < maxCache; i++)
            {
                string[] str = strs[i].Split(',');
                data_num = str.Length;
                for (var j = 0; j < str.Length; j++)
                {
                    if (!data.ContainsKey(j))
                    {   
                        double[] d = new double[maxCache];
                        data.Add(j, d);
                    }
                 data[j][i] = double.Parse(str[j]);
                }

            }
            //最多12条数据，删除多余的
            for (int i = data_num; i <12; i++)
            {
                chart.GetSerie(i).ClearData();
            }

            for (int i = 0; i < maxCache; i++)
            {
               AddOneData(count++);
            }
        }

        void AddOneData(int i = -1)
        {
            foreach (var j in data.Keys)
            {
                if (i>=0 && i < maxCache)
                {
                    lastValues[j] = data[j][i];
                }            

                chart.AddData(j, lastValues[j]);
            }
            chart.AddXAxisData(i.ToString());
        }
    }
}
