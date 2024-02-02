using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class DropdownEvents : MonoBehaviour, IPointerEnterHandler
{
    public TMP_Dropdown dropdown;
    public string selectedData;

    List<string> filesList = new List<string>();
    TMP_Dropdown.OptionData tempData;

    // Start is called before the first frame update
    void Start()
    {
        FindFiles();
        InitDropdown();
        dropdown.onValueChanged.AddListener(Change);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        filesList.Clear();
        FindFiles();
        InitDropdown();
    }


    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    filesList.Clear();
    //    FindFiles();
    //    InitDropdown();
    //}

    void Change(int indexOption)
    {
        FindFiles();
        InitDropdown();
        for (int i = 0; i < filesList.Count + 1; i++)
        {
            if (i == indexOption - 1)
            {
                selectedData = filesList[i];
                //print(selectedData);
            }
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
    }


    public void FindFiles()
    {
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
}
