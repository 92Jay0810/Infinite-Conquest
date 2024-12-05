using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;
using MySql.Data.MySqlClient;

public class StartSceneManager : MonoBehaviourPunCallbacks
{
    private string SceneToLoad;
    string userName= LoginAndRegister.LoggedInUsername;
    private int playerid = LoginAndRegister.LoggedInUserID;
    [SerializeField] Text welcomeText;
    [SerializeField] GameObject chapterSelectionPanel;

    //DB
    string server = "34.80.93.104";
    string database = "questionnare"; // 資料庫名稱
    string user = "ss";
    string password = "worinidaya";
    private MySqlConnection connection;
    private void Start()
    {
       welcomeText.text= "您好! "+userName+" \n \n今天要玩什麼模式呢?";
    }
    public void OnclickStartSingle(){
        if (initDB())
        {
            int currentChapter = GetCurrentChapter(playerid);
            // 根據 currentChapter 生成可選擇的關卡按鈕
            ShowChapterSelection(currentChapter);
        }
        else
        {
            welcomeText.text = "開啟資料庫失敗，請確認網路連線在重試";
        }
        print("Click單人遊戲");
    }

    private bool initDB()
    {
        string connectionString = $"Server={server};Database={database};User ID={user};Password={password};Pooling=false;";
        try
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
            return true;  // 連接成功，返回 true
        }
        catch (Exception ex)
        {
            Debug.LogError("開啟資料庫失敗: " + ex.Message);
            return false;  // 連接失敗，返回 false
        }
        finally
        {
            if (connection != null)
            {
                connection.Close();
            }
        }
    }

    private int GetCurrentChapter(int userID)
    {
        if (userID == 0)
        {
            return 1;
        }
        int chapterID = 1; // 預設值為第一章
        string query = "SELECT currentChapter FROM users WHERE id = @UserID";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@UserID", userID);

            try
            {
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        chapterID = Convert.ToInt32(reader["currentChapter"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to get currentChapter: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        return chapterID;
    }

    // 顯示章節選擇按鈕
    private void ShowChapterSelection(int currentChapter)
    {
        if (chapterSelectionPanel != null)
        {
            chapterSelectionPanel.SetActive(true);
            Button returnbutton = chapterSelectionPanel.transform.Find("return").GetComponent<Button>();
            returnbutton.onClick.AddListener(() =>
            {
                chapterSelectionPanel.SetActive(false);
            });
            Button button1 = chapterSelectionPanel.transform.Find("1").GetComponent<Button>();
            Button button2 = chapterSelectionPanel.transform.Find("2").GetComponent<Button>();
            Button button3 = chapterSelectionPanel.transform.Find("3").GetComponent<Button>();
            Button button4 = chapterSelectionPanel.transform.Find("4").GetComponent<Button>();
            Button[] chapterButtons = { button1, button2, button3, button4 };
            foreach (Button chapter_option in chapterButtons)
            {
                chapter_option.interactable = false;
            }
                for (int i = 0; i < currentChapter; i++)
            {
                chapterButtons[i].interactable = true;
                int temp_chapter = i + 1;
                chapterButtons[i].onClick.AddListener(() =>
                {
                    OnChapterSelected(temp_chapter, chapterSelectionPanel);
                });
            }
        }
        else
        {
                Debug.LogError("ChapterSelectionPanel not found in the scene.");
        }
    }

    // 當玩家選擇某一章節
    private void OnChapterSelected(int chapterID, GameObject chapterSelectionPanel )
    {
        // 根據玩家選擇的章節加載對應的場景
        string sceneName = "ch" + chapterID + "Scene";
        chapterSelectionPanel.SetActive(false);
        SceneManager.LoadScene(sceneName);
    }

    public void OnclickStartMulti(){
        PhotonNetwork.LocalPlayer.NickName = userName;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        SceneToLoad = "MultiLobbyScene";
        PlayerPrefs.SetString("PlayerName", userName);
        print("Click多人遊戲");
    }

    public override void OnConnectedToMaster()
    {
        print("connect");
        SceneManager.LoadScene(SceneToLoad);
    }
}
