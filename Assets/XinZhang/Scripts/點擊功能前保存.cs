// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Photon.Pun;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;
// using Photon.Realtime;
// using System.Text;


// public class MultiLobbySceneManager : MonoBehaviourPunCallbacks
// {
//     [SerializeField] InputField inputRoomName;
//     // [SerializeField] InputField inputFindRoomName;
//     [SerializeField] Text textRoomList;
//     [SerializeField] Text WelcomePlayerName;
//     [SerializeField] int roomNumberLength = 5;
//     // Start is called before the first frame update
//     void Start()
//     {
//         if(PhotonNetwork.IsConnected == false){
//             SceneManager.LoadScene("StartScene");
//         }else{
//              if(PhotonNetwork.CurrentLobby == null){//null代表從start進來
//                 PhotonNetwork.JoinLobby();
//                 }
//              if(PlayerPrefs.HasKey("PlayerName")) {
//                 string playerName = PlayerPrefs.GetString("PlayerName");
//                 Debug.Log("PlayerName retrieved: " + playerName);
//                 WelcomePlayerName.text = "歡迎: " + playerName;
//              }else{
//                 Debug.Log("PlayerName not found in PlayerPrefs.");
//              }
//              Debug.Log("MultiLobbySceneManager started.");
//              inputRoomName.characterLimit = 5;
            
//         }
//     }


//     public override void OnConnectedToMaster()
//     {
//         print("connected to master!");
//         PhotonNetwork.JoinLobby();
//     }

//     public override void OnJoinedLobby()
//     {
//         print("Lobby joined");
//     }

//     //好的鮮隱藏
//     // public string GetRoomName(){
//     //     string roomName = inputRoomName.text;
//     //     return roomName.Trim();//擷取調空白字元
//     // }

//     public string GetRoomName(){

//         if (inputRoomName != null){
//             string roomName = inputRoomName.text;
//             return roomName.Trim();
//         }else{
//             Debug.LogError("InputRoomName is not initialized!");
//             return ""; // 或者返回其他適當的默認值
//             }
//     }

    
//     //統一用getroomname先隱藏
//     // public string GetFindRoomName(){
//     //     string roomName = inputRoomName.text;
//     //     return roomName.Trim();//擷取調空白字元
        
//     // }

    
// //chat給的
// //     public string GetFindRoomName(){
// //     if(inputFindRoomName != null){
// //         string roomName = inputFindRoomName.text;
// //         return roomName.Trim();//擷取調空白字元
// //     } else {
// //         Debug.LogError("InputFindRoomName is not initialized!");
// //         return string.Empty; // 或者返回 null 或其他適當的值
// //     }
// // }


//     public string GetPlayerName(){
//         string playerName = PlayerPrefs.GetString("PlayerName");
//         return playerName.Trim();//擷取調空白字元
//     }

//     //chat魔改
//     // public void OnClickSearchRoomName(){
//     //     string searchText = GetRoomName();

//     //     if (searchText.Length == 0)
//     //     {
//     //         // 如果搜索文字為空，不進行搜索
//     //         return;
//     //     }

//     //     // 創建 TypedLobby 對象
//     //     TypedLobby typedLobby = new TypedLobby("MultiLobbyScene", LobbyType.SqlLobby);

//     //     // 創建 SQL-like filter 字符串
//     //     //string sqlLobbyFilter = "your SQL-like filter";
//     //     string sqlLobbyFilter = "RoomName LIKE '%" + searchText + "%'";

//     //     // 獲取自定義房間列表
//     //     bool success = PhotonNetwork.GetCustomRoomList(typedLobby, sqlLobbyFilter);
//     //     if (!success){
//     //     Debug.LogError("Failed to get custom room list.");
//     //     }

//     //     // // 遍歷房間列表，查找包含搜索文字的房間
//     //     // List<RoomInfo> rooms = new List<RoomInfo>(PhotonNetwork.GetCustomRoomList());
//     //     // List<RoomInfo> matchingRooms = new List<RoomInfo>();

//     //     // foreach (RoomInfo room in rooms)
//     //     // {
//     //     //     if (room.Name.Contains(searchText))
//     //     //     {
//     //     //         matchingRooms.Add(room);
//     //     //     }
//     //     // }

//     //     // // 更新搜索結果
//     //     // UpdateSearchResult(matchingRooms);
//     // }

//     //chat魔改
//     // 更新搜索結果
//     // private void UpdateSearchResult(List<RoomInfo> roomlist)
//     // {
//     //     // 將搜索結果顯示在 UI 上
//     //     StringBuilder sb = new StringBuilder();

//     //     foreach (RoomInfo roomInfo in roomlist)
//     //     {
//     //         sb.AppendLine("Room Name: " + roomInfo.Name + " | Player Count: " + roomInfo.PlayerCount);
//     //     }

//     //     textRoomList.text = sb.ToString();
//     // }

//     public void OnClickCreatRoom()
//     {
//         string roomName = GenerateRoomName();
//         string playerName = GetPlayerName();
//         if(roomName.Length > 0 && playerName.Length > 0){
//         PhotonNetwork.CreateRoom(roomName);
//         print("Room created"+ roomName);
//         PhotonNetwork.LocalPlayer.NickName = playerName;  
//        }else{
//         print("Invalid RoomName or PlayerName");
//        }  
//     }

//     public void OnClickJoinRoom(){
//         string roomName = GetRoomName();
//         string playerName = GetPlayerName();
//         if(roomName.Length > 0 && playerName.Length > 0){
//         PhotonNetwork.JoinRoom(roomName);
//         PhotonNetwork.LocalPlayer.NickName = playerName;  
//        }else{
//         print("Invalid RoomName or PlayerName");
//        }
//     }
    

//     public override void OnJoinedRoom()
//     {
//         print("Room joined");
//         SceneManager.LoadScene("MultiRoomScene");
//     }

//     public void OnClickBackStart()
//     {
//         PhotonNetwork.Disconnect();
//         print("photon disconnect");
//         SceneManager.LoadScene("StartScene");
//     }

//     private string GenerateRoomName()
//     {
//         StringBuilder sb = new StringBuilder();
//         System.Random random = new System.Random();
//         for (int i = 0; i < roomNumberLength; i++)
//         {
//             sb.Append(random.Next(0, 10)); // Append random digit between 0 and 9
//         }
//         return sb.ToString();
//     }


//     //chat魔改，有bug
//     // public override void OnRoomListUpdate(List<RoomInfo> roomList)
//     // {
//     //     string roomName = GetRoomName();
//     //     // 如果搜索框中有文字，則執行搜尋房間的功能
//     //     if (!string.IsNullOrEmpty(roomName)){
//     //     OnClickSearchRoomName();
//     //     return;
//     //     }

//     //     // 如果搜索框中沒有文字，則正常顯示房間列表
//     //     print("update");
//     //     StringBuilder sb = new StringBuilder();
//     //     foreach(RoomInfo roomInfo in roomList){
//     //         if(roomInfo.PlayerCount > 0){
//     //             sb.AppendLine("→ " + roomInfo.Name + ": " +roomInfo.PlayerCount);
//     //         }
//     //     }
//     //     textRoomList.text = sb.ToString();
//     //     // if (textRoomList != null) {
//     //     //     textRoomList.text = sb.ToString();
//     //     //     } else {
//     //     //         Debug.LogError("textRoomList is null!");
//     //     //     }

//     // }

    
//     public override void OnRoomListUpdate(List<RoomInfo> roomList)
//     {
//         if(roomList != null) // 檢查 roomList 是否為空
//         {
//             print("update");
//             StringBuilder sb = new StringBuilder();
//             foreach(RoomInfo roomInfo in roomList)
//             {
//                 if(roomInfo.PlayerCount > 0)
//                 {
//                     sb.AppendLine("→ " + roomInfo.Name + ": " +roomInfo.PlayerCount);
//                 } 
//             }
//             textRoomList.text = sb.ToString();
//         }else{
//             Debug.LogError("Room list is null."); // 如果 roomList 為空，則打印錯誤信息
//         }
//     }


// }
