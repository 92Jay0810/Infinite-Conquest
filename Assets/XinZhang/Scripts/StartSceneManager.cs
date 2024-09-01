using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;

public class StartSceneManager : MonoBehaviourPunCallbacks
{
    private string SceneToLoad;
    string userName= LoginAndRegister.LoggedInUsername;
    [SerializeField] Text welcomeText;

    private void Start()
    {
       welcomeText.text= "您好! "+userName+" \n \n今天要玩什麼模式呢?";
    }
    public void OnclickStartSingle(){
            print("Click單人遊戲");
            SceneManager.LoadScene("ch1Scene");
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
