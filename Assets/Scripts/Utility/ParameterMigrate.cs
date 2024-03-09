using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;
using Npgsql;

#if (UNITY_EDITOR)
public class ParameterMigrate
{
    static string dataMappingConfigFilePath = "../../json/DataMapping/data50.json";

    [MenuItem("Tools/Ç¨ÒÆ²ÎÊý±í")]
    static void MigrateParameterTable()
    {
        //string dataMappingConfigFullPath = Path.GetDirectoryName(Path.Combine(Application.dataPath, dataMappingConfigFilePath));
        string dataMappingConfigFullPath = "D:\\data50.json";
        string dataMappingJson = File.ReadAllText(dataMappingConfigFullPath);
        var mapping = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(dataMappingJson);

        string oldInputParamTableName = "parameter_old";
        string inputParamTableName = "parameter";
        string oldOutputParamTableName = "outputpar_old";
        string outputParamTableName = "outputpar";
        string host = "localhost";
        string port = "12345";
        string database = "digitalfactory";
        string username = "digitalfactory";
        string password = "digital123456";
        string connstr = NpgsqlUtil.buildConnectionString(host, port, database, username, password);
        var conn = NpgsqlUtil.OpenDatabaseConnection(connstr);

        Dictionary<string, string> oldInputIdToNameMapping = GetIdToNameMapping(oldInputParamTableName, conn);
        Dictionary<string, string> inputNameToIdMapping = GetNameToIdMapping(inputParamTableName, conn);
        Dictionary<string, string> oldOutputIdToNameMapping = GetIdToNameMapping(oldOutputParamTableName, conn);
        Dictionary<string, string> outputNameToIdMapping = GetNameToIdMapping(outputParamTableName, conn);

        foreach (var dataName in mapping.Keys)
        {
            var tableName = mapping[dataName]["table"];
            var columnName = mapping[dataName]["column"];

            string inputPrefix = "input";
            string outputPrefix = "output";
            if(columnName.StartsWith(inputPrefix))
            {
                string id = columnName.Substring(inputPrefix.Length);
                string name = oldInputIdToNameMapping[id];
                if(inputNameToIdMapping.ContainsKey(name))
                {
                    string newid = inputNameToIdMapping[name];
                    mapping[dataName]["column"] = inputPrefix + newid;
                }
                else
                {
                    mapping[dataName]["column"] += "??";
                }
            }
            else if(columnName.StartsWith(outputPrefix))
            {
                string id = columnName.Substring(outputPrefix.Length);
                string name = oldOutputIdToNameMapping[id];
                if(outputNameToIdMapping.ContainsKey(name))
                {
                    string newid = outputNameToIdMapping[name];
                    mapping[dataName]["column"] = outputPrefix + newid;
                }
                else
                {
                    mapping[dataName]["column"] += "??";
                }
            }
        }

        string newJson = JsonConvert.SerializeObject(mapping);
        File.WriteAllText(dataMappingConfigFullPath, newJson);
    }
    private static List<(string,string)> GetIdNameTuples(string tableName, NpgsqlConnection conn)
    {
        return NpgsqlUtil.Query(string.Format("SELECT id, name FROM {0};", tableName), conn, (cmd, reader) =>
        {
            return (reader.GetValue(reader.GetOrdinal("id")).ToString(), reader.GetValue(reader.GetOrdinal("name")).ToString());
        });
    }
    private static Dictionary<string, string> GetIdToNameMapping(string tableName, NpgsqlConnection conn)
    {
        List<(string, string)> idNameTuples = GetIdNameTuples(tableName, conn);
        Dictionary<string, string> idToNameMapping = new Dictionary<string, string>();
        foreach(var tuple in  idNameTuples)
        {
            idToNameMapping[tuple.Item1] = tuple.Item2;
        }
        return idToNameMapping;
    }
    private static Dictionary<string, string> GetNameToIdMapping(string tableName, NpgsqlConnection conn)
    {
        List<(string, string)> idNameTuples = GetIdNameTuples(tableName, conn);
        Dictionary<string, string> nameToIdMapping = new Dictionary<string, string>();
        foreach (var tuple in idNameTuples)
        {
            nameToIdMapping[tuple.Item2] = tuple.Item1;
        }
        return nameToIdMapping;
    }
}
#endif