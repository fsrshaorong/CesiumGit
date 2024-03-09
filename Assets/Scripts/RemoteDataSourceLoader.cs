using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RemoteDataSourceLoader
{
    private string host;
    private string port;
    private string database;
    private string username;
    private string password;

    private string configPath = "../../json/config.json";

    private NpgsqlConnection connection;

    public RemoteDataSourceLoader() 
    {
        string configFullPath = Path.Combine(Application.dataPath, configPath);
        string configJson = File.ReadAllText(configFullPath);
        Dictionary<string, string> config = JsonConvert.DeserializeObject<Dictionary<string, string>>(configJson);

        host = config["host"];
        port = config["port"];
        database = config["database"];
        username = config["username"];
        password = config["password"];

        string connStr = NpgsqlUtil.buildConnectionString(host, port, database, username, password);

        connection = NpgsqlUtil.OpenDatabaseConnection(connStr);
    }

    public List<DataSource> GetDataSourceListByClientType(string clientType)
    {
        string sql = string.Format
            (
                "SELECT name, schema, mapping, uimapping " +
                "FROM unitydatasource " +
                "WHERE clienttype = \'{0}\'", 
                clientType
            );

        List<DataSource> list = NpgsqlUtil.Query(sql, connection, (cmd, reader) =>
        {
            DataSource dataSource = new DataSource();
            dataSource.name = reader.GetValue(reader.GetOrdinal("name")).ToString();
            dataSource.value = database;
            dataSource.schema = reader.GetValue(reader.GetOrdinal("schema")).ToString();
            dataSource.type = "generated";
            dataSource.host = host;
            dataSource.port = port;
            dataSource.user = username;
            dataSource.password = password;
            dataSource.mapping = reader.GetValue(reader.GetOrdinal("mapping")).ToString();
            dataSource.uimapping = reader.GetValue(reader.GetOrdinal("uimapping")).ToString();
            dataSource.timestamp = "date";
            return dataSource;
        });

        return list;
    }

    public async UniTask<List<DataSource>> GetDataSourceListByClientTypeAsync(string clientType)
    {
        string sql = string.Format
            (
                "SELECT name, schema, mapping, uimapping " +
                "FROM unitydatasource " +
                "WHERE clienttype = \'{0}\'",
                clientType
            );

        List<DataSource> list = await NpgsqlUtil.QueryAsync(sql, connection, (cmd, reader) =>
        {
            DataSource dataSource = new DataSource();
            dataSource.name = reader.GetValue(reader.GetOrdinal("name")).ToString();
            dataSource.value = database;
            dataSource.schema = reader.GetValue(reader.GetOrdinal("schema")).ToString();
            dataSource.type = "generated";
            dataSource.host = host;
            dataSource.port = port;
            dataSource.user = username;
            dataSource.password = password;
            dataSource.mapping = reader.GetValue(reader.GetOrdinal("mapping")).ToString();
            dataSource.uimapping = reader.GetValue(reader.GetOrdinal("uimapping")).ToString();
            dataSource.timestamp = "date";
            return dataSource;
        });

        return list;
    }
}
