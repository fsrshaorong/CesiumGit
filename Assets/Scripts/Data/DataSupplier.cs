using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

struct DataTableProperties
{
    public uint count;  //数据所在的表的行数
    public float cycle; //可以有不同周期(s)
}

class DataTableDynamicProperties
{
    public float lastUpdateTime;

    public int currentCache;    //目前正在使用的缓存编号
    public int currentRow;  //目前使用的数据行号

    public int loadingCache;    //最后缓存的缓存编号
    public int loadingRow;  //最后缓存时的行号
}

public class DataDeliveryEvent
{
    public string name;
    public string data;
    public int type;
}

public class DataSourceChangedEvent
{

}

[Serializable]
public class DataSource
{
    public string name;
    public string value;    //database
    public string schema;
    public string type;
    public string host;
    public string user;
    public string password;
    public string mapping;
    public string port;
    public string uimapping;
    public string timestamp;
}

public class DataSupplier : MonoBehaviour
{
    /// <summary>
    /// 加入配置文件后，由于原来的一条数据在不同的数据源下可能对应不同的表和列，现在通过数据项名来指定数据
    /// 数据项名与（表，列）的关系保存在配置文件中
    /// 切换数据源时要根据监听的数据项名保留原来的事件和监听
    /// 这个字典中一开始就有SupportedData.json中的项
    /// </summary>
    Dictionary<string, UnityEvent<DataDeliveryEvent>> databaseEventJsonRef;

    /// <summary>
    /// 记录数据项对应的表和列
    /// </summary>
    Dictionary<string, Tuple<string, string>> dataMappings;

    /// <summary>
    /// dataMappings的逆映射，由表和列获得数据项名称，一个表列可能被多个数据项对应
    /// </summary>
    Dictionary<Tuple<string, string>, List<string>> inverseDataMapping;

    /// <summary>
    /// 字典中保存了各个表以及对应数据项发生变化的事件，同一个表和列可能对应多个数据项，因此最后得到的值是List
    /// </summary>
    Dictionary<string, Dictionary<string, List<UnityEvent<DataDeliveryEvent>>>> databaseEvent;

    public UnityEvent<DataSourceChangedEvent> onDataSourceChanged;

    string connString = "Host=localhost;Username=postgres;Password=admin;Database=data55";
    NpgsqlConnection conn;

    const int cacheCount = 2;
    const int cacheSize = 5;
    const float cycle = 1;  //秒
    string timestampCol;
    float speedupFac = 1;
    enum MessageType
    {
        Normal = 0,
        Loading = 1
    }

    Dictionary<string, Dictionary<string, List<string>>>[] dataCaches;

    Dictionary<string, List<string>> listeningDatas;

    //表名 -> 行数，周期
    Dictionary<string, DataTableProperties> dataTableProps;

    Dictionary<string, DataTableDynamicProperties> tableDynamic;

    Dictionary<string, List<bool>> cacheLoadStates;

    CancellationTokenSource databaseSwitchCancellation; //所有在数据库发生切换时需要结束的任务会拿到的token
    //string databaseName;    //切换数据库时如果还有未完成的请求，用数据库名来判断得到的数据是否过时

    DropdownEvents dropdown;

    bool isQuerying;

    string supportedDataFilePath = "../../json/SupportedDatas.json";
    string dataSourceConfigFilePath = "../../json/DataSource.json";

    DataSource currentDataSource;

    List<string> allFileLines;

    UIMappingReader uimapping;

    // Start is called before the first frame update
    void Awake()
    {
        //获取所有可配置的数据项
        databaseEventJsonRef = new Dictionary<string, UnityEvent<DataDeliveryEvent>>();
        /*string supportedDataFileFullPath = Path.Combine(Application.dataPath, supportedDataFilePath);
        string spdjson = File.ReadAllText(supportedDataFileFullPath);*/
        string supportDataFileFullPath = "json/SupportedDatas";
        TextAsset textAsset = Resources.Load<TextAsset>(supportDataFileFullPath);
        String spdjson = textAsset.text;
        List<string> supportedData = JsonConvert.DeserializeObject<List<string>>(spdjson);
        foreach(var dataName in supportedData)
        {
            databaseEventJsonRef[dataName] = new UnityEvent<DataDeliveryEvent>();
        }

        /*dropdown = GameObject.FindObjectOfType<DropdownEvents>();
        dropdown.onChangeSelectedData.AddListener(SelectedDataChanged);*/
        databaseSwitchCancellation = new CancellationTokenSource();
    }

    // Update is called once per frame
    void Update()
    {
        if (tableDynamic == null || dataCaches == null)
        {
            //BroadcastMsg("等待数据库连接");
            return;
        }
        for (int i = 0; i < tableDynamic.Count; i++)
        {
            var tableName = tableDynamic.ElementAt(i).Key;
            tableDynamic[tableName].lastUpdateTime += Time.deltaTime;
            if (tableDynamic[tableName].lastUpdateTime > dataTableProps[tableName].cycle)
            {
                tableDynamic[tableName].lastUpdateTime -= dataTableProps[tableName].cycle;

                int currentCacheSize = dataCaches[tableDynamic[tableName].currentCache][tableName].ElementAt(0).Value.Count;

                foreach (var columnName in databaseEvent[tableName].Keys)
                {
                    
                    if (Math.Floor((tableDynamic[tableName].currentRow / cacheSize).ConvertTo<double>()) % cacheCount != tableDynamic[tableName].currentCache)
                    {
                        //在数据填满的情况下已经使用完现在这块cache
                        cacheLoadStates[tableName][tableDynamic[tableName].currentCache] = false;
                        if (cacheLoadStates[tableName][(tableDynamic[tableName].currentCache + cacheCount - 1) % cacheCount])
                        {
                            UpdateTableCache(tableDynamic[tableName].currentCache, tableName).AttachExternalCancellation(databaseSwitchCancellation.Token);
                            tableDynamic[tableName].currentCache = (tableDynamic[tableName].currentCache + 1) % cacheCount;
                        }
                    }
                    else
                    {
                        //没有超过设定的CacheSize的情况
                        //当前Cache还没使用完
                        if (tableDynamic[tableName].currentRow % cacheSize < currentCacheSize)
                        {
                            //databaseEvent[tableName][dataName].Invoke(dataCaches[currentCache][tableName][dataName][currentRow % cacheSize]);
                        }
                        //实际的cache填不满设定的cacheSize的情况下,已经到所有数据的结尾了
                        else
                        {
                            cacheLoadStates[tableName][tableDynamic[tableName].currentCache] = false;
                            if (cacheLoadStates[tableName][(tableDynamic[tableName].currentCache + cacheCount - 1) % cacheCount])
                            {
                                UpdateTableCache(tableDynamic[tableName].currentCache, tableName).AttachExternalCancellation(databaseSwitchCancellation.Token);
                                tableDynamic[tableName].currentRow = 0;
                                tableDynamic[tableName].currentCache = (tableDynamic[tableName].currentCache + 1) % cacheCount;
                            }
                        }
                    }

                    string invokeMesg;
                    MessageType invokeMesgType;
                    if (cacheLoadStates[tableName][tableDynamic[tableName].currentCache])
                    {
                        invokeMesg = 
                            dataCaches
                            [
                                tableDynamic[tableName].currentCache
                            ]
                            [tableName]
                            [columnName]
                            [
                                tableDynamic[tableName].currentRow % cacheSize
                            ];
                        invokeMesgType = MessageType.Normal;
                    }
                    else
                    {
                        invokeMesg = "等待数据传输";
                        invokeMesgType = MessageType.Loading;
                    }
                    
                    for(int j = 0; j < databaseEvent[tableName][columnName].Count; j++)
                    {
                        DataDeliveryEvent msg = new DataDeliveryEvent()
                        {
                            name = inverseDataMapping[new Tuple<string, string>(tableName, columnName)][j],
                            data = invokeMesg,
                            type = (int)invokeMesgType
                        };
                        try
                        {
                            databaseEvent[tableName][columnName][j].Invoke(msg);
                        }
                        catch(Exception e)
                        {
                            print("监听函数报错：" + e.ToString());
                            //throw e;
                        }
                    }
                }

                if(cacheLoadStates[tableName][tableDynamic[tableName].currentCache])
                {
                    //加载完成后每次经过cycle时间currentRow就会+1，记录当前使用到缓存的第几行
                    tableDynamic[tableName].currentRow++;
                }
            }
        }
    }

    void OnDestroy() 
    {
        databaseSwitchCancellation.Cancel();
        conn?.Close();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataName">数据项名，如“炉温”等</param>
    /// <param name="tableName">数据库中的表名</param>
    /// <param name="columnName">数据库表的列名</param>
    public void SetListeningData(string dataName, string tableName, string columnName)
    {
        if(listeningDatas.ContainsKey(tableName))
        {
            if (!listeningDatas[tableName].Contains(columnName))
            {
                listeningDatas[tableName].Add(columnName);
            }
        }
        else
        {
            //监听的数据项中还没有这个表的信息
            listeningDatas[tableName] = new List<string>();
            if(timestampCol != "")
            {
                listeningDatas[tableName].Add(timestampCol);
                if (!timestampCol.Equals(columnName))
                {
                    listeningDatas[tableName].Add(columnName);
                }
            }
            else
            {
                listeningDatas[tableName].Add(columnName);
            }
        }

        if (!dataTableProps.ContainsKey(tableName))
        {
            dataTableProps[tableName] = new DataTableProperties()
            {
                count = 0,
                cycle = 1
            };
        }

        if(!databaseEvent.ContainsKey(tableName))
        {
            databaseEvent[tableName] = new Dictionary<string, List<UnityEvent<DataDeliveryEvent>>>();
        }
        if (!databaseEvent[tableName].ContainsKey(columnName))
        {
            databaseEvent[tableName][columnName] = new List<UnityEvent<DataDeliveryEvent>>();
        }

        databaseEvent[tableName][columnName].Add(databaseEventJsonRef[dataName]);
    }

    public void Listen(string dataName, UnityAction<DataDeliveryEvent> call)
    {
        databaseEventJsonRef[dataName].AddListener(call);

        //所有的在映射配置中的数据项已经被绑定到相应的事件上了，不存在没有在监听的表列
        //if(dataMappings != null)    //如果dataMappings等于null说明还没切换数据源，只需要简单登记一下
        //{
        //    //运行中添加监听
        //    string tableName = dataMappings[dataName].Item1;
        //    string columnName = dataMappings[dataName].Item2;
        //    SetListeningData(dataName, tableName, columnName);
        //}
    }

    /// <summary>
    /// 获取数据源对应的配置文件中的对应UI数据
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Dictionary<string, string> getUICustomInfo(string key)
    {
        return uimapping.getCustiomData(key);
    }

    public async UniTask SwitchToDataSource(DataSource dataSource)
    {
        currentDataSource = dataSource;

        databaseSwitchCancellation.Cancel();    //先停止正在进行的请求
        databaseSwitchCancellation.Dispose();
        databaseSwitchCancellation = new CancellationTokenSource();

        dataTableProps = new Dictionary<string, DataTableProperties>();
        databaseEvent = new Dictionary<string, Dictionary<string, List<UnityEvent<DataDeliveryEvent>>>>();
        listeningDatas = new Dictionary<string, List<string>>();

        tableDynamic = new Dictionary<string, DataTableDynamicProperties>();    //动态数据，会在SwitchToDatabase中重置
        cacheLoadStates = new Dictionary<string, List<bool>>();

        allFileLines = new List<string>();

        conn = null;
        isQuerying = false;

        timestampCol = dataSource.timestamp;

        if (dataSource.type == "db" || dataSource.type == "generated")
        {
            string connString = NpgsqlUtil.buildConnectionString
                (dataSource.host, dataSource.port, dataSource.value, dataSource.user, dataSource.password);
            //await UniTask.Delay(6000);
            conn = await NpgsqlUtil.OpenDatabaseConnectionAsync(connString);
        }
        else if (dataSource.type == "file")
        {
            string[] tmpAllLines = await File.ReadAllLinesAsync(dataSource.host);
            allFileLines = new List<string>(tmpAllLines);
        }
        else
        {
            throw new Exception("无法使用该数据源：" + dataSource.name + ":" + dataSource.type);
        }

        string dataSourceConfigFullFilePath = Path.GetDirectoryName(Path.Combine(Application.dataPath, dataSourceConfigFilePath));

        //读入ui配置文件
        if(dataSource.type == "generated")
        {
            uimapping = new UIMappingReader();
            uimapping.ResolveMapping(dataSource.uimapping);
            
            /*string uiMappingFileFullPath;
            uiMappingFileFullPath = "json/UIMapping/glass";
            uimapping = new UIMappingReader();
            uimapping.ReadMapping(uiMappingFileFullPath);*/
        }
        else
        {
            string uiMappingFileFullPath;
            /*uiMappingFileFullPath = Path.GetDirectoryName(Path.Combine(Application.dataPath, dataSourceConfigFilePath));
            uiMappingFileFullPath = Path.Combine(dataSourceConfigFullFilePath, dataSource.uimapping);*/
            uiMappingFileFullPath = "json/UIMapping/glass";
            uimapping = new UIMappingReader();
            uimapping.ReadMapping(uiMappingFileFullPath);
        }
        

        //读取当前数据库对应配置文件
        string mappingFileFullPath;
        string mappingJson = "";
        if (dataSource.type == "generated")
        {
            //string sql = String.Format("SELECT \"value\" FROM \"setting\" WHERE \"name\" = 'unitydatamapping';"); ;
            //try
            //{
            //    using (var cmd = new NpgsqlCommand(sql, conn))
            //    using (var reader = await cmd.ExecuteReaderAsync())
            //    {
            //        while (reader.Read())
            //        {
            //            mappingJson = reader.GetValue(reader.GetOrdinal("value")).ToString();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    print("sql语句:" + sql + "->引发如下报错:");
            //    throw ex;
            //}
            
            mappingJson = dataSource.mapping; 
            /*mappingFileFullPath = "json/DataMapping/data50";
            TextAsset textassetcurrent = Resources.Load<TextAsset>(mappingFileFullPath);
            mappingJson = textassetcurrent.text;*/
        }
        else
        {
            mappingFileFullPath = dataSource.mapping;
            TextAsset textassetcurrent = Resources.Load<TextAsset>(mappingFileFullPath);
            mappingJson = textassetcurrent.text;
            /*mappingFileFullPath = Path.Combine(dataSourceConfigFullFilePath, dataSource.mapping);
            mappingJson = File.ReadAllText(mappingFileFullPath);*/
        }

        Dictionary<string, Dictionary<string, string>> mappings = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(mappingJson);
        dataMappings = new Dictionary<string, Tuple<string, string>>();
        inverseDataMapping = new Dictionary<Tuple<string, string>, List<string>>();

        string schema = dataSource.schema;
        foreach (var mappingKey in mappings.Keys)
        {
            string tableName = mappings[mappingKey]["table"];
            string columnName = mappings[mappingKey]["column"];
            if (dataSource.type == "generated")
            {
                tableName = schema + "." + tableName; 
            }
            Tuple<string, string> tcTuple = new Tuple<string, string>(tableName, columnName);
            dataMappings[mappingKey] = tcTuple; //生成数据名到表列的映射

            //生成反映射
            if(!inverseDataMapping.ContainsKey(tcTuple))
            {
                inverseDataMapping[tcTuple] = new List<string>();
            }
            inverseDataMapping[tcTuple].Add(mappingKey);

            //只有被监听时才继续 //似乎没什么必要，databaseEventJsonRef一开始就包含所有的SupportedData.json中的数据项名
            //if (!databaseEventJsonRef.ContainsKey(mappingKey))
            //{
            //    continue;
            //}

            SetListeningData(mappingKey, tableName, columnName);
        }

        foreach (var tableName in listeningDatas.Keys)
        {
            tableDynamic[tableName] = new DataTableDynamicProperties();
            tableDynamic[tableName].lastUpdateTime = 0;
            tableDynamic[tableName].currentCache = 0;
            tableDynamic[tableName].currentRow = 0;

            cacheLoadStates[tableName] = new List<bool>();
            for (int i = 0; i < cacheCount; i++)
            {
                cacheLoadStates[tableName].Add(false);
            }
        }

        //this.databaseName = dataSource.value;

        InitCaches();

        onDataSourceChanged.Invoke(new DataSourceChangedEvent());
    }

    public async UniTaskVoid InitCaches()
    {
        dataCaches = new Dictionary<string, Dictionary<string, List<string>>>[cacheCount];
        for(int i = 0; i < cacheCount; i++)
        {
            dataCaches[i] = new Dictionary<string, Dictionary<string, List<string>>>();
        }
        foreach (var tableName in listeningDatas.Keys)
        {
            foreach(var cache in dataCaches)
            {
                cache[tableName] = new Dictionary<string, List<string>>();
                foreach (var dataName in listeningDatas[tableName])
                {
                    cache[tableName][dataName] = new List<string>(new string[cacheSize]);
                }
            }

            //var handle = WriteDataToCache(dataCaches[0], tableName, tableDynamic[tableName].currentRow)
            //    .AttachExternalCancellation(databaseSwitchCancellation.Token);
            var handle = InitHeadTableCache(tableName)
                .AttachExternalCancellation(databaseSwitchCancellation.Token);

            for (int i = tableDynamic[tableName].loadingCache + 1; 
                i % cacheCount != tableDynamic[tableName].currentCache % cacheCount;
                i = (i + 1) % cacheCount)
            {
                handle = 
                    InitTableCache
                    (
                    i, 
                    tableName, 
                    handle
                    ).AttachExternalCancellation(databaseSwitchCancellation.Token);
            }

            //await handle;
        }
    }

    /// <summary>
    /// 只在初始化中用到
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    async UniTask InitHeadTableCache(string tableName)
    {
        await WriteDataToCache(dataCaches[0], tableName, tableDynamic[tableName].currentRow);
        cacheLoadStates[tableName][0] = true;

        double tmpCyc = cycle;

        if(currentDataSource.timestamp != "")
        {
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd hh:mm:ss";
            DateTime t1 = Convert.ToDateTime(dataCaches[0][tableName][timestampCol][0], dtFormat);
            DateTime t2 = Convert.ToDateTime(dataCaches[0][tableName][timestampCol][1], dtFormat);
            tmpCyc = (t2 - t1).TotalSeconds / speedupFac;
        }
        
        dataTableProps[tableName] = new DataTableProperties()
        {
            count = 0,
            cycle = tmpCyc.ConvertTo<float>()
        };

        tableDynamic[tableName].lastUpdateTime = dataTableProps[tableName].cycle;
    }
    /// <summary>
    /// 只在初始化中用到
    /// </summary>
    /// <param name="cacheIndex"></param>
    /// <param name="tableName"></param>
    /// <param name="prerequisite"></param>
    /// <returns></returns>
    async UniTask InitTableCache(int cacheIndex, string tableName, UniTask prerequisite)
    {
        await prerequisite;
        await UpdateTableCache(cacheIndex, tableName);
    }

    async UniTask UpdateTableCache(int cacheIndex, string tableName)
    {
        int lastCacheCount = dataCaches[(tableDynamic[tableName].loadingCache + cacheCount - 1) % cacheCount][tableName].ElementAt(0).Value.Count;
        tableDynamic[tableName].loadingRow += lastCacheCount;
        if (lastCacheCount < cacheSize)
        {
            tableDynamic[tableName].loadingRow = 0;
        }

        tableDynamic[tableName].loadingCache = (tableDynamic[tableName].loadingCache + 1) % cacheCount;

        bool isEmpty = true;
        while(isEmpty)
        {
            //await UniTask.Delay(10000);   //测试请求时间很长
            await WriteDataToCache(dataCaches[cacheIndex], tableName, tableDynamic[tableName].loadingRow);
            if (dataCaches[cacheIndex][tableName].ElementAt(0).Value.Count > 0 )
            {
                isEmpty = false;
                cacheLoadStates[tableName][cacheIndex] = true;
            }
            else
            {
                tableDynamic[tableName].loadingRow = 0;
            }
        }
    }

    async UniTask WriteDataToCache(Dictionary<string, Dictionary<string, List<string>>> cache, string tableName, int start)
    {
        if(currentDataSource.type == "db" || currentDataSource.type == "generated")
        {
            string dataNameStr;
            StringBuilder dataNamesBuilder = new StringBuilder();
            foreach (var dataName in listeningDatas[tableName])
            {
                dataNamesBuilder.Append("\"");
                dataNamesBuilder.Append(dataName);
                dataNamesBuilder.Append("\"");
                dataNamesBuilder.Append(",");
            }
            dataNamesBuilder.Remove(dataNamesBuilder.Length - 1, 1);
            dataNameStr = dataNamesBuilder.ToString();
            string sql = String.Format("SELECT {0} FROM {1} ORDER BY \"{4}\" LIMIT {3} OFFSET {2};", dataNameStr, tableName, start.ToString(), cacheSize.ToString(), timestampCol);

            await UniTask.WaitUntil(() => !isQuerying);
            isQuerying = true;

            foreach (var dataName in listeningDatas[tableName])
            {
                cache[tableName][dataName].Clear();
            }

            try
            {
                await NpgsqlUtil.ExecuteAsync(sql, conn, (cmd, reader) =>
                {
                    foreach (var dataName in listeningDatas[tableName])
                    {
                        cache[tableName][dataName].Add(reader.GetValue(reader.GetOrdinal(dataName)).ToString());
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                isQuerying = false;
            }
        }
        else if(currentDataSource.type == "file")
        {
            foreach (var dataName in listeningDatas[tableName])
            {
                cache[tableName][dataName].Clear();
            }
            for (int i = start; i < allFileLines.Count && i - start < cacheSize; i++)
            {
                string line = allFileLines[i];
                string[] columns = line.Split(',');
                foreach (var dataName in listeningDatas[tableName])
                {
                    cache[tableName][dataName].Add(columns[int.Parse(dataName)]);
                }
            }
        }
    }

    public void SelectedDataChanged(DataSource selectedData)
    {
        if(selectedData.type == "db" || selectedData.type == "file" || selectedData.type == "generated")
        {
            SwitchToDataSource(selectedData);
        }
        else if(selectedData.type == "null")
        {
            tableDynamic = null;
        }
    }

    void BroadcastMsg(string msg)
    {
        foreach(var tableName in databaseEvent.Keys)
        {
            foreach(var columnName in databaseEvent[tableName].Keys)
            {
                for (int i = 0; i < databaseEvent[tableName][columnName].Count; i++)
                {
                    DataDeliveryEvent m = new DataDeliveryEvent()
                    {
                        name = inverseDataMapping[new Tuple<string, string>(tableName, columnName)][i],
                        data = msg,
                        type = (int)MessageType.Normal
                    };
                    databaseEvent[tableName][columnName][i].Invoke(m);
                }
            }
        }
    }
}
