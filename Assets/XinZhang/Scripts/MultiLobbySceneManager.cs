using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;
using System.Text;


public class MultiLobbySceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField inputRoomName;
    // [SerializeField] InputField inputFindRoomName;
    [SerializeField] Text textRoomInformation;

    [SerializeField] Text WelcomePlayerName;

    [SerializeField] int roomNumberLength = 5;

    [SerializeField] Button roomNamePrefab;

    
    [SerializeField] Transform roomListParent;

    List<RoomInfo> currentroomList;
    
    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsConnected == false){
            SceneManager.LoadScene("StartScene");
        }else{
             if(PhotonNetwork.CurrentLobby == null){//null代表從start進來
                PhotonNetwork.JoinLobby();
                }
             if(PlayerPrefs.HasKey("PlayerName")) {
                string playerName = PlayerPrefs.GetString("PlayerName");
                Debug.Log("PlayerName retrieved: " + playerName);
                WelcomePlayerName.text = "歡迎: " + playerName;
             }else{
                Debug.Log("PlayerName not found in PlayerPrefs.");
             }
             Debug.Log("MultiLobbySceneManager started.");
             inputRoomName.characterLimit = 5;
            
        }
    }


    public override void OnConnectedToMaster()
    {
        print("connected to master!");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("Lobby joined");
    }


    public string GetRoomName(){

        if (inputRoomName != null){
            string roomName = inputRoomName.text;
            return roomName.Trim();
        }else{
            Debug.LogError("InputRoomName is not initialized!");
            return ""; // 或者返回其他適當的默認值
            }
    }


    public string GetPlayerName(){
        string playerName = PlayerPrefs.GetString("PlayerName");
        return playerName.Trim();//擷取調空白字元
    }

    public void OnClickSearchRoomName(){
        string searchText = GetRoomName();

        if (searchText.Length == 0){
            Debug.Log("Input not received");
            return;
        }

        if(currentroomList != null){
            StringBuilder sb = new StringBuilder();
            foreach(RoomInfo roomInfo in currentroomList)
            {
                if(roomInfo.PlayerCount > 0 && roomInfo.Name == searchText){
                    string hostNickname = roomInfo.CustomProperties.ContainsKey("Host") ? (string)roomInfo.CustomProperties["Host"] : "Unknown";
                    sb.AppendLine("Room Name: "+ roomInfo.Name +"\n房主: " + hostNickname+ "\n房間人數: " +roomInfo.PlayerCount+"/"+roomInfo.MaxPlayers);
                } 
            }
            textRoomInformation.text = sb.ToString();
        }
        
    }

    private void HandleSearchResults(List<RoomInfo> roomList)
    {
        string searchText = GetRoomName();
        if (string.IsNullOrEmpty(searchText))
        {
            return;
        }

        bool foundMatchingRoom = false;
        StringBuilder sb = new StringBuilder();

        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.PlayerCount > 0 && roomInfo.Name.Contains(searchText))
            {
                string roomName = roomInfo.Name;
                int playerCount = roomInfo.PlayerCount;
                int maxPlayers = roomInfo.MaxPlayers;
                string hostNickname = roomInfo.CustomProperties.ContainsKey("Host") ? (string)roomInfo.CustomProperties["Host"] : "";

                sb.AppendLine($"Room Name: {roomName} | Players: {playerCount}/{maxPlayers} | Host: {hostNickname}");
                foundMatchingRoom = true;
                break;
            }
        }

        if (!foundMatchingRoom)
        {
            sb.AppendLine("No matching room found.");
        }

        textRoomInformation.text = sb.ToString();
    }


    public void OnClickCreatRoom()
    {
        string roomName = GenerateRoomName();
        string playerName = GetPlayerName();
        if(roomName.Length > 0 && playerName.Length > 0){
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "Host", playerName } };
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "Host" };
            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
            print("Room created"+ roomName);
            PhotonNetwork.LocalPlayer.NickName = playerName;  
       }else{
        print("Invalid RoomName or PlayerName");
       }  
    }

    public void OnClickJoinRoom(){
        string roomName = GetRoomName();
        string playerName = GetPlayerName();
        if(roomName.Length > 0 && playerName.Length > 0){
        PhotonNetwork.JoinRoom(roomName);
        PhotonNetwork.LocalPlayer.NickName = playerName;  
       }else{
        print("Invalid RoomName or PlayerName");
       }
    }

    public void OnClickImageJoinRoom(string roomName){
        Debug.Log("按鈕被典籍");
        
        string playerName = GetPlayerName();
        if(roomName.Length > 0 && playerName.Length > 0){
        PhotonNetwork.JoinRoom(roomName);
        PhotonNetwork.LocalPlayer.NickName = playerName;  
       }else{
        print("Invalid RoomName or PlayerName");
       }
    }

    
    public override void OnJoinedRoom()
    {
        print("Room joined");
        SceneManager.LoadScene("MultiRoomScene");
    }

    public void OnClickBackStart()
    {
        PhotonNetwork.Disconnect();
        print("photon disconnect");
        SceneManager.LoadScene("StartScene");
    }

    private string GenerateRoomName()
    {
        StringBuilder sb = new StringBuilder();
        System.Random random = new System.Random();
        for (int i = 0; i < roomNumberLength; i++)
        {
            sb.Append(random.Next(0, 10)); // Append random digit between 0 and 9
        }
        return sb.ToString();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(roomList != null) {// 檢查 roomList 是否為空
            print("update");
            currentroomList = roomList;
            StringBuilder sb = new StringBuilder();
            foreach (RoomInfo roomInfo in roomList){
                Debug.Log(roomInfo.PlayerCount);
                 if (roomInfo.PlayerCount == 0){
                    // 在 roomListParent 下找到对应的 Prefab，然后删除
                    Transform roomPrefab = roomListParent.Find(roomInfo.Name);
                    if (roomPrefab != null){
                        Destroy(roomPrefab.gameObject);
                    }
                }else{
                    // 房间有玩家，检查是否已经存在对应的 Prefab
                    Transform existingRoomPrefab = roomListParent.Find(roomInfo.Name);
                    if (existingRoomPrefab == null){
                        // 如果不存在，创建对应的 Prefab
                        Button roomImage = Instantiate(roomNamePrefab, roomListParent);
                        // 设置房间名字
                        Text roomNameText = roomImage.GetComponentInChildren<Text>();
                        if (roomNameText != null){
                            roomNameText.text = roomInfo.Name;
                        }else{
                            Debug.Log("RoomName Text not found in room prefab.");
                        }
                        // 设置房间的名字作为 Prefab 的名字，以便后续查找和删除
                        roomImage.name = roomInfo.Name;
                        roomImage.onClick.AddListener(() => OnClickImageJoinRoom(roomNameText.text));
                    }
                }
            }
        }else{
            Debug.LogError("Room list is null."); // 如果 roomList 為空，則打印錯誤信息
        }
    }
  
}
