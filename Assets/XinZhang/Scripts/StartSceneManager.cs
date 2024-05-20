using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;

public class StartSceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField inputPlayerName;
    private string SceneToLoad;

    public string GetPlayerName(){
        string playerName = inputPlayerName.text;
        return playerName.Trim();//擷取調空白字元
    }

    public void OnclickStartSingle(){
        string playerName = GetPlayerName();
        if(playerName.Length > 0){
        PhotonNetwork.LocalPlayer.NickName = playerName;
        PhotonNetwork.ConnectUsingSettings();
        SceneToLoad = "SingleLobbyScene";
        print("Click單人遊戲");
       }else{
        print("Invalid RoomName or PlayerName");
       }
    }

    public void OnclickStartMulti(){
        string playerName = GetPlayerName();
        if(playerName.Length > 0){
        PhotonNetwork.LocalPlayer.NickName = playerName;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        SceneToLoad = "MultiLobbyScene";
        PlayerPrefs.SetString("PlayerName", playerName);
        print("Click多人遊戲");
       }else{
        print("Invalid RoomName or PlayerName");
       }
    }
    // public void OnclickStartMulti(){
    //     PhotonNetwork.ConnectUsingSettings();
    //     SceneToLoad = "MultiLobbyScene";
    //     print("Click多人遊戲");
        
    // }

    public override void OnConnectedToMaster()
    {
        print("connect");
        SceneManager.LoadScene(SceneToLoad);
    }

}
