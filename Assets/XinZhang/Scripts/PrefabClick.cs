using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PrefabClick : MonoBehaviour
{
    public void JoinRoomOnClick()
    {
        string roomName = transform.parent.name; // 获取父对象的名字，即房间名字
        string playerName = PlayerPrefs.GetString("PlayerName");
        if (roomName.Length > 0 && playerName.Length > 0)
        {
            PhotonNetwork.JoinRoom(roomName);
            PhotonNetwork.LocalPlayer.NickName = playerName;
            print("Room joined");
        }
        else
        {
            Debug.LogError("Invalid RoomName or PlayerName");
        }
    }
}
