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

//     [SerializeField] Button roomNamePrefab;

    
//     [SerializeField] Transform roomListParent;
    
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

//     //chat魔改搜尋功能
//     public void OnClickSearchRoomName(){
//         string searchText = GetRoomName();

//         if (searchText.Length == 0)
//         {
//             Debug.Log("input沒得到");
//             // 如果搜索文字為空，不進行搜索
//             return;
//         }

//         // 創建 TypedLobby 對象
//         TypedLobby typedLobby = new TypedLobby("MultiLobbyScene", LobbyType.SqlLobby);

//         // 創建 SQL-like filter 字符串
//         //string sqlLobbyFilter = "your SQL-like filter";
//         string sqlLobbyFilter = "RoomName LIKE '%" + searchText + "%'";

//         // 獲取自定義房間列表
//         bool success = PhotonNetwork.GetCustomRoomList(typedLobby, sqlLobbyFilter);
//         if (!success){
//         Debug.Log("Failed to get custom room list.");
//         }

//         // // 遍歷房間列表，查找包含搜索文字的房間
//         // List<RoomInfo> rooms = new List<RoomInfo>(PhotonNetwork.GetCustomRoomList());
//         // List<RoomInfo> matchingRooms = new List<RoomInfo>();

//         // foreach (RoomInfo room in rooms)
//         // {
//         //     if (room.Name.Contains(searchText))
//         //     {
//         //         matchingRooms.Add(room);
//         //     }
//         // }

//         // // 更新搜索結果
//         // UpdateSearchResult(matchingRooms);
//     }

    
//     //chat魔改，更新搜索結果
//     private void UpdateSearchResult(List<RoomInfo> roomlist)
//     {
//         // 將搜索結果顯示在 UI 上
//         StringBuilder sb = new StringBuilder();

//         foreach (RoomInfo roomInfo in roomlist)
//         {
//             sb.AppendLine("Room Name: " + roomInfo.Name + " | Player Count: " + roomInfo.PlayerCount);
//         }

//         textRoomList.text = sb.ToString();
//     }

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

//     public void OnClickImageJoinRoom(string roomName){
//         Debug.Log("按鈕被典籍");
        
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


//     //chat魔改，有bug，搜尋功能
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
//         if(roomList != null) {// 檢查 roomList 是否為空
//             print("update");
//             StringBuilder sb = new StringBuilder();
//             foreach (RoomInfo roomInfo in roomList){
//                 Debug.Log(roomInfo.PlayerCount);
//                  if (roomInfo.PlayerCount == 0){
//                     // 在 roomListParent 下找到对应的 Prefab，然后删除
//                     Transform roomPrefab = roomListParent.Find(roomInfo.Name);
//                     if (roomPrefab != null){
//                         Destroy(roomPrefab.gameObject);
//                     }
//                 }else{
//                     // 房间有玩家，检查是否已经存在对应的 Prefab
//                     Transform existingRoomPrefab = roomListParent.Find(roomInfo.Name);
//                     if (existingRoomPrefab == null){
//                         // 如果不存在，创建对应的 Prefab
//                         Button roomImage = Instantiate(roomNamePrefab, roomListParent);
//                         // 设置房间名字
//                         Text roomNameText = roomImage.GetComponentInChildren<Text>();
//                         if (roomNameText != null){
//                             roomNameText.text = roomInfo.Name;
//                         }else{
//                             Debug.Log("RoomName Text not found in room prefab.");
//                         }
//                         // 设置房间的名字作为 Prefab 的名字，以便后续查找和删除
//                         roomImage.name = roomInfo.Name;
//                         roomImage.onClick.AddListener(() => OnClickImageJoinRoom(roomNameText.text));
//                     }
//                 }

//                 // if(roomInfo.PlayerCount > 0){
//                 //     sb.AppendLine("→ " + roomInfo.Name + ": " +roomInfo.PlayerCount);
//                 //     GameObject roomImage = Instantiate(roomNamePrefab, roomListParent);
                
//                 //     // 在新房間圖片中找到用於顯示房間名的 Text，並將其設置為房間名
//                 //     Text roomNameText = roomImage.GetComponentInChildren<Text>();
//                 //     if (roomNameText != null){
//                 //         roomNameText.text = roomInfo.Name;
//                 //     }
//                 // } 
                
//                 // // 使用預置件生成新的房間圖片
//                 // GameObject roomImage = Instantiate(roomNamePrefab, roomListParent);
                
//                 // // 在新房間圖片中找到用於顯示房間名的 Text，並將其設置為房間名
//                 // Text roomNameText = roomImage.GetComponentInChildren<Text>();
//                 // if (roomNameText != null){
//                 //     roomNameText.text = roomInfo.Name;
//                 // }
//             }
//             textRoomList.text = sb.ToString();
//         }else{
//             Debug.LogError("Room list is null."); // 如果 roomList 為空，則打印錯誤信息
//         }
//     }
    
//     //版2能生成不能更新
//     // public override void OnRoomListUpdate(List<RoomInfo> roomList)
//     // {
//     //     if(roomList != null) {// 檢查 roomList 是否為空
//     //         print("update");
//     //         StringBuilder sb = new StringBuilder();
//     //         foreach (RoomInfo roomInfo in roomList){
//     //             // 使用預置件生成新的房間圖片
//     //             GameObject roomImage = Instantiate(roomNamePrefab, roomListParent);
                
//     //             // 在新房間圖片中找到用於顯示房間名的 Text，並將其設置為房間名
//     //             Text roomNameText = roomImage.GetComponentInChildren<Text>();
//     //             if (roomNameText != null){
//     //                 roomNameText.text = roomInfo.Name;
//     //             }
//     //             else{
//     //                 Debug.LogError("RoomName Text not found in room prefab.");
//     //             }
//     //         }
//     //         textRoomList.text = sb.ToString();
//     //     }else{
//     //         Debug.LogError("Room list is null."); // 如果 roomList 為空，則打印錯誤信息
//     //     }
//     // }

//     //原版
//     // public override void OnRoomListUpdate(List<RoomInfo> roomList)
//     // {
//     //     if(roomList != null) // 檢查 roomList 是否為空
//     //     {
//     //         print("update");
//     //         StringBuilder sb = new StringBuilder();
//     //         foreach(RoomInfo roomInfo in roomList)
//     //         {
//     //             if(roomInfo.PlayerCount > 0)
//     //             {
//     //                 sb.AppendLine("→ " + roomInfo.Name + ": " +roomInfo.PlayerCount);
//     //             } 
//     //         }
//     //         textRoomList.text = sb.ToString();
//     //     }else{
//     //         Debug.LogError("Room list is null."); // 如果 roomList 為空，則打印錯誤信息
//     //     }
//     // }



// }
