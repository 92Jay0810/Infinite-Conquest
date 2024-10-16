﻿using System;
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginAndRegister : MonoBehaviour
{
    [SerializeField] GameObject Login;
     InputField Login_userNameInput;
     InputField Login_passwordInput;
   

    [SerializeField] GameObject Register;
    InputField Register_userNameInput;
    InputField Register_passwordInput;
    InputField Register_emailInput;

    [SerializeField] Text feedbackText;
    [SerializeField] Button SwitchLoginAndRegisterButton;

    //DB
    string server = "34.80.93.104";
    string database = "questionnare"; // 資料庫名稱
    string user = "ss";
    string password = "worinidaya";

    // 用於在其他場景中訪問使用者名稱
    public static string LoggedInUsername { get; private set; } = "";
    public static int LoggedInUserID { get; private set; } = 0;

    private void Start()
    {
        Login_userNameInput = Login.transform.Find("UserName").GetComponent<InputField>();
        Login_passwordInput = Login.transform.Find("Password").GetComponent<InputField>();

        Register_userNameInput = Register.transform.Find("UserName").GetComponent<InputField>();
        Register_passwordInput = Register.transform.Find("Password").GetComponent<InputField>();
        Register_emailInput = Register.transform.Find("email").GetComponent<InputField>();
        Text SwitchLoginAndRegister_Text = SwitchLoginAndRegisterButton.transform.Find("Text").GetComponent<Text>();
        SwitchLoginAndRegisterButton.onClick.AddListener(() =>
        {
            if (Login.activeSelf)
            {
                SwitchLoginAndRegister_Text.text = "前往登錄";
            }
            else
            {
                SwitchLoginAndRegister_Text.text = "前往註冊";
            }
            Login.SetActive(!Login.activeSelf);
            Register.SetActive(!Register.activeSelf);
            Login_userNameInput.text = "";
            Login_passwordInput.text = "";
            Register_userNameInput.text = "";
            Register_passwordInput.text = "";
        });

    }

    public void OnLoginButtonClick()
    {
        string Login_username = Login_userNameInput.text;
        string Login_password = Login_passwordInput.text;

        if (string.IsNullOrEmpty(Login_username) || string.IsNullOrEmpty(Login_password))
        {
            feedbackText.text = "請輸入帳號和密碼";
            return;
        }
        string connectionString = $"Server={server};Database={database};User ID={user};Password={password};Pooling=false;";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT id, password FROM users WHERE username = @username", connection);
                cmd.Parameters.AddWithValue("@username", Login_username);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int userID = reader.GetInt32("id");
                        string storedPassword = reader.GetString("password");

                        if (storedPassword == Login_password) // 建議在實際應用中使用哈希比對
                        {
                            feedbackText.text = "登錄成功";
                            LoggedInUsername = Login_username; // 保存使用者名稱
                            LoggedInUserID = userID; // 保存userID
                            Debug.Log("  LoggedInUserID:" + LoggedInUserID);
                            SceneManager.LoadScene("StartScene"); // 跳轉到下一個場景
                        }
                        else
                        {
                            feedbackText.text = "密碼錯誤";
                        }
                    }
                    else
                    {
                        feedbackText.text = "帳號不存在";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("開啟資料庫失敗: " + ex.Message);
                feedbackText.text = "開啟資料庫失敗，請確認網路連線在重試";
            }
        }
    }
    public void OnRegisterButtonClick()
    {
        string Register_username = Register_userNameInput.text;
        string Register_password = Register_passwordInput.text;
        string Register_email = Register_emailInput.text;

        if (string.IsNullOrEmpty(Register_username) || string.IsNullOrEmpty(Register_password))
        {
            feedbackText.text = "請輸入帳號和密碼";
            return;
        }
        // 檢查 email 是否包含 @ 符號
        if (string.IsNullOrEmpty(Register_email) || !Register_email.Contains("@"))
        {
            feedbackText.text = "請輸入有效的 Email";
            return;
        }
        string connectionString = $"Server={server};Database={database};User ID={user};Password={password};Pooling=false;";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {

            try
            {
                connection.Open();

                // 檢查帳號是否存在
                MySqlCommand checkCmd = new MySqlCommand("SELECT COUNT(*) FROM users WHERE username = @username", connection);
                checkCmd.Parameters.AddWithValue("@username", Register_username);
                int userExists = int.Parse(checkCmd.ExecuteScalar().ToString());

                if (userExists > 0)
                {
                    feedbackText.text = "帳號已存在，請使用其他帳號";
                }
                else
                {
                    // 註冊新帳號
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO users (username, password, email) VALUES (@username, @password, @register_email)", connection);
                    cmd.Parameters.AddWithValue("@username", Register_username);
                    cmd.Parameters.AddWithValue("@password", Register_password);
                    cmd.Parameters.AddWithValue("@register_email", Register_email);

                    cmd.ExecuteNonQuery();
                    feedbackText.text = "註冊成功";
                    Login.SetActive(true);
                    Register.SetActive(false);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("開啟資料庫失敗: " + ex.Message);
                feedbackText.text = "開啟資料庫失敗，請確認網路連線在重試";
            }
        }
    }
}
