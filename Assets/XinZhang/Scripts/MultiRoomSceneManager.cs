using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using Photon.Realtime;

public class MultiRoomSceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text textRoomName;
    [SerializeField] Text textPlayerList;


    [SerializeField] Button buttonStartGame;
    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.CurrentRoom == null){
            //Debug.LogError("No room found. Returning to StartScene.");
            //PhotonNetwork.Disconnect();
            SceneManager.LoadScene("StartScene");
        }else{
            Debug.Log("Current room: " + PhotonNetwork.CurrentRoom.Name);
            textRoomName.text =  "當前房間： " + PhotonNetwork.CurrentRoom.Name;
            UpdatePlayerList();
        }
        buttonStartGame.interactable = PhotonNetwork.IsMasterClient;
        
        
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
    }

    // public void UpdatePlayerList(){
    //     StringBuilder sb = new StringBuilder();
    //     foreach(var kvp  in PhotonNetwork.CurrentRoom.Players){
    //         sb.AppendLine("→ " + kvp.Value.NickName);
    //     }
    //     textPlayerList.text = sb.ToString();
    // }

    public void UpdatePlayerList()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var kvp in PhotonNetwork.CurrentRoom.Players)
        {
            string playerType = kvp.Value.IsMasterClient ? "房主" : "玩家";
            sb.AppendLine($"→ {kvp.Value.NickName} ({playerType})");
        }
        textPlayerList.text = sb.ToString();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public void OnClickLeaveRoom(){
        PhotonNetwork.LeaveRoom();
    }

    public void OnClickStartGame(){
        SceneManager.LoadScene("SampleScene");
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MultiLobbyScene");
    }
}
