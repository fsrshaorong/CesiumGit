using Cysharp.Threading.Tasks;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpgsqlUtil
{
    public static string buildConnectionString(string host, string port, string database, string username, string password)
    {
        return String.Format
        (
            "Host={0};Username={1};Password={2};Database={3};Port={4}",
            host,
            username,
            password,
            database,
            port
        );
    }

    public async static UniTask<NpgsqlConnection> OpenDatabaseConnectionAsync(string connStr)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connStr);
        var dataSource = dataSourceBuilder.Build();

        var conn = await dataSource.OpenConnectionAsync();

        return conn;
    }

    public static NpgsqlConnection OpenDatabaseConnection(string connStr)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connStr);
        var dataSource = dataSourceBuilder.Build();

        var conn = dataSource.OpenConnection();

        return conn;
    }

    public static List<T> Query<T>(string query, NpgsqlConnection connection, Func<NpgsqlCommand, NpgsqlDataReader, T> func)
    {
        List<T> result = new List<T>();
        try
        {
            using (var cmd = new NpgsqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(func(cmd, reader));
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("sql语句:" + query + "->引发如下报错:");
            throw ex;
        }

        return result;
    }

    public static async UniTask<List<T>> QueryAsync<T>(string query, NpgsqlConnection connection, Func<NpgsqlCommand, NpgsqlDataReader, T> func)
    {
        List<T> result = new List<T>();
        try
        {
            using (var cmd = new NpgsqlCommand(query, connection))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    result.Add(func(cmd, reader));
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("sql语句:" + query + "->引发如下报错:");
            throw ex;
        }

        return result;
    }

    public static void Execute(string exec, NpgsqlConnection connection, Action<NpgsqlCommand, NpgsqlDataReader> func)
    {
        try
        {
            using (var cmd = new NpgsqlCommand(exec, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    func(cmd, reader);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("sql语句:" + exec + "->引发如下报错:");
            throw ex;
        }
    }

    public static async UniTask ExecuteAsync(string exec, NpgsqlConnection connection, Action<NpgsqlCommand, NpgsqlDataReader> func)
    {
        try
        {
            using (var cmd = new NpgsqlCommand(exec, connection))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    func(cmd, reader);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("sql语句:" + exec + "->引发如下报错:" + ex.StackTrace);
            throw ex;
        }
    }
}
