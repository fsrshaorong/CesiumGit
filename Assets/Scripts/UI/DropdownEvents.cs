using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.Events;
using System;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using Unity.VisualScripting;
//using UnityEditor.UI;

public class DropdownEvents : MonoBehaviour, IPointerEnterHandler
{
    public TMP_Dropdown dropdown;

    [NonSerialized]
    public DataSource selectedData = new DataSource() { type = "null" };

    //List<string> filesList;
    List<string> filesList = new List<string>();
    TMP_Dropdown.OptionData tempData;

    DataSourceJsonData localDataSources;
    string dataSourceConfig = "../../json/DataSource.json";

    List<DataSource> dataSources;

    RemoteDataSourceLoader remoteDSLoader;

    public UnityEvent<DataSource> onChangeSelectedData;

    // Start is called before the first frame update
    void Start()
    {
        remoteDSLoader = new RemoteDataSourceLoader();

        //FindFiles();
        //localDataSources = new DataSourceJsonData();
        //localDataSources.postgresql = new List<DataSource>();
        GetDataSources(dataSourceConfig);
        //Debug.Log("异步");
        //InitDropdown();
        dropdown.onValueChanged.AddListener(Change);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //filesList.Clear();
        //FindFiles();
        //Debug.Log("读数据开始");
        GetDataSources(dataSourceConfig);
        //Debug.Log("异步");
        //InitDropdown();
    }


    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    filesList.Clear();
    //    FindFiles();
    //    InitDropdown();
    //}

    void Change(int indexOption)
    {
        //FindFiles();
        //GetDataSources(dataSourceConfig);
        if(indexOption == 0)
        {
            selectedData = new DataSource()
            {
                name = "",
                type = "null"
            };
            onChangeSelectedData.Invoke(selectedData);
        }
        else
        {
            //selectedData = dropdown.options[indexOption].text;
            if(indexOption > filesList.Count)
            {
                selectedData = dataSources[indexOption - filesList.Count - 1];
            }
            else
            {
                selectedData = new DataSource()
                {
                    name = filesList[indexOption - 1],
                    type = "file"
                };
            }
           
            onChangeSelectedData.Invoke(selectedData);
        }
    }

    private void InitDropdown()
    {
        //dropdown.transform.GetChild(0).GetComponent<TMP_Text>().text = "请选择数据源";
        //清空默认节点
        dropdown.options.Clear();

        tempData = new TMP_Dropdown.OptionData();
        dropdown.options.Add(tempData);

        //初始化
        foreach (var file in filesList)
        {
            tempData = new TMP_Dropdown.OptionData();
            tempData.text = file.Replace(".txt", "");
            dropdown.options.Add(tempData);
        }

        foreach(var dataSource in dataSources)
        {
            tempData = new TMP_Dropdown.OptionData();
            tempData.text = dataSource.name;
            dropdown.options.Add(tempData);
        }
    }

    public void FindFiles()
    {
        return;
        //string dataPath = Application.dataPath + "../BackEnd";  //路径
        string dataPath = Path.Combine(Application.dataPath, "../../BackEnd");
        //print(dataPath);
        if (!Directory.Exists(dataPath))
        {
            //创建
            Directory.CreateDirectory(dataPath);
        }

        //获取指定路径下面的所有资源文件  
        if (Directory.Exists(dataPath))//using System.IO;
        {
            DirectoryInfo direction = new DirectoryInfo(dataPath);
            FileInfo[] files = direction.GetFiles("*.txt", SearchOption.TopDirectoryOnly);

            //print(files.Length);

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))//.asset
                {
                    continue;
                }
                if (!filesList.Contains(files[i].Name.Split("_")[0]))
                {
                    filesList.Add(files[i].Name.Split("_")[0]);
                }
            }
        }
    }

    /// <summary>
    /// 从DataSource.json中读取数据源
    /// </summary>
    /// <summary>
    /// 从DataSource.json中读取数据源
    /// </summary>
    async UniTask GetDataSources(string filePath)
    {
        var localDSReadingTask = ReadDataSourceConfigFile(filePath);
        var remoteDSReadingTask = remoteDSLoader.GetDataSourceListByClientTypeAsync("unity");

        dataSources = new List<DataSource>();

        await localDSReadingTask;
        try
        {
            dataSources.AddRange(localDataSources.postgresql);
        }
        catch(Exception ex)
        {
            throw ex;
        }

        dataSources.AddRange(await remoteDSReadingTask);

        InitDropdown();
    }

    async UniTask ReadDataSourceConfigFile(string filePath)
    {
        string DataSourceJson = Path.Combine(Application.dataPath, filePath);
        string strs = await File.ReadAllTextAsync(DataSourceJson);

        localDataSources = JsonUtility.FromJson<DataSourceJsonData>(strs);
    }
}

[Serializable]
class DataSourceJsonData
{
    public List<DataSource> postgresql;
}
