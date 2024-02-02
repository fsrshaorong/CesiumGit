using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Buttonevent:MonoBehaviour
{
    public Text uitext;
    public DataSupplier dataSupplier;
    public DataSource dataSource;
    private GameObject dataSwitch;
    private CanvasGroup canvasdisappear; 
    CancellationTokenSource databaseSwitchCancellation;

    private RemoteDataSourceLoader remoteDataSourceLoader;
    private List<DataSource> dataSources;

    private void Start()
    {
        uitext.text= "当前数据库为remote50";
        /*dataSource = new DataSource("db data50", "remote50", "db", "123.207.10.251:15432", "unity", "u123456",
                "DataMapping/data50", "15432");*/

        remoteDataSourceLoader = new RemoteDataSourceLoader(); 
        
        dataSources=remoteDataSourceLoader.GetDataSourceListByClientType("pico");
        dataSource = dataSources[0];
        /*dataSource = new DataSource()
        {
            name = "db remote50",
            value = "digitalfactory",
            type = "generated",
            host = "test.ebim.com",
            user = "digitalfactory",
            password = "digital123456",
            mapping = "json/DataMapping/data50",
            port = "15432",
            uimapping = "UIMapping/glass",
            timestamp = "date"
        };*/
        
        dataSupplier.SelectedDataChanged(dataSource);
    }
    public void decideDB50()
    {
        if (dataSource.value!= "remote50")
        {
            /*dataSource = new DataSource("db data50", "remote50", "db", "123.207.10.251:15432", "unity", "u123456",
                "DataMapping/data50","15432");*/
            dataSource = new DataSource()
            {
                name = "db remote50",
                value = "remote50",
                type = "db",
                host = "123.207.10.251:15432",
                user = "unity",
                password = "u123456",
                mapping = "DataMapping/data50",
                port = "15432",
                uimapping = "UIMapping/glass",
                timestamp = "date"
            };
            dataSupplier.SelectedDataChanged(dataSource);
            uitext.text = "当前数据库为" + dataSource.value;
        }
    }

    public void decideDB55()
    {
        if (dataSource.value != "data55")
        {
            /*dataSource = new DataSource("db data55", "data55", "db", "localhost", "postgres", "1",
                "./DataMapping/data55.json","5432");*/
            dataSource = new DataSource()
            {
                name = "db data55",
                value = "data55",
                type = "db",
                host = "localhost",
                user = "postgres",
                password = "1",
                mapping = "DataMapping/data55",
                port = "5050",
                uimapping = "UIMapping/glass",
                timestamp = "date"
            };
            dataSupplier.SelectedDataChanged(dataSource);
            uitext.text = "当前数据库为" + dataSource.value;
        }
    }
    //暂无
    public void file()
    {
        if (dataSource.value != "export")
        {
            /*dataSource = new DataSource("file export", "export", "file", "../BackEnd/lisi2023-04-08_output.txt", "", "",
                "./DataMapping/export.json","5432");*/
            dataSupplier.SelectedDataChanged(dataSource);
            uitext.text = "当前数据库为" + dataSource.name;
        }
    }

    public void disappear()
    {
        dataSwitch = GameObject.Find("dataSwitch");
        canvasdisappear = dataSwitch.GetComponent<CanvasGroup>();
        canvasdisappear.alpha = 0.0f;
    }
}