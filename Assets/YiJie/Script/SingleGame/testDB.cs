using System;
using MySql.Data.MySqlClient;
using UnityEngine;

public class testDB : MonoBehaviour
{
    private MySqlConnection connection;

    void Start()
    {
        string server = "localhost";
        string database = "questionnare"; // 替換為你的資料庫名稱
        string user = "root";
        string password = "123"; 

        string connectionString = $"Server={server};Database={database};User ID={user};Password={password};Pooling=false;";

        try
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();

            Debug.Log("Successfully connected to MySQL database!");

            // 示例：執行查詢
            MySqlCommand command = new MySqlCommand("SELECT * FROM questions", connection); // 替換為你的表名
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Debug.Log(reader["Description"].ToString()); // 替換為你想要讀取的列
            }

            reader.Close();
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to connect to MySQL database: " + ex.Message);
        }
        finally
        {
            if (connection != null)
            {
                connection.Close();
            }
        }
    }
}
