using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
public class GameSceneManger : MonoBehaviourPunCallbacks
{
    [SerializeField] Sprite[] ground_resource;
    SpriteRenderer[] ground_Sprite;
    private PhotonView pv;
    public Dictionary<Photon.Realtime.Player, bool> alivePlayerMap = new Dictionary<Photon.Realtime.Player, bool>();
    void Start()
    {
        pv = this.gameObject.GetComponent<PhotonView>();
        if (PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("MultiLobbyScene");
        }
        else
        {
            initGame();
        }
    }
    void Update()
    {

    }
    public void initGame()
    {
        initGround();
        initPlayer();
        foreach(var kvp in PhotonNetwork.CurrentRoom.Players)
        {
            alivePlayerMap[kvp.Value] = true;
        }
    }
    private void initGround()
    {
        ground_Sprite = new SpriteRenderer[6];
        ground_Sprite[0] = transform.Find("ground1").GetComponent<SpriteRenderer>();
        ground_Sprite[1] = transform.Find("ground2").GetComponent<SpriteRenderer>();
        ground_Sprite[2] = transform.Find("ground3").GetComponent<SpriteRenderer>();
        ground_Sprite[3] = transform.Find("ground4").GetComponent<SpriteRenderer>();
        ground_Sprite[4] = transform.Find("ground5").GetComponent<SpriteRenderer>();
        ground_Sprite[5] = transform.Find("ground6").GetComponent<SpriteRenderer>();

        if (PhotonNetwork.IsMasterClient)
        {
            // MasterClient 隨機選擇地面資源
            int[] spriteIndices = new int[6];
            for (int i = 0; i < 6; i++)
            {
                spriteIndices[i] = Random.Range(0, ground_resource.Length);
            }

            // 通過 RPC 同步給所有客戶端
            pv.RPC("SyncGroundSprite", RpcTarget.All, spriteIndices);
        }
    }
    [PunRPC]
    private void SyncGroundSprite(int[] spriteIndices)
    {
        for (int i = 0; i < 6; i++)
        {
            ground_Sprite[i].sprite = ground_resource[spriteIndices[i]];
        }
    }
    private void initPlayer()
    {
        Vector3 player_position = new Vector3(Random.Range(-180f, 180f), Random.Range(-60f, 180f), 0);
        Vector3 char_position;
        if (player_position.x + 10f <= 180f)
        {
            char_position = new Vector3(player_position.x + 10f, player_position.y, player_position.z);
        }
        else
        {
            char_position = new Vector3(player_position.x - 10f, player_position.y, player_position.z);
        }
        GameObject player_gameObject = PhotonNetwork.Instantiate("player", player_position, Quaternion.identity);
        Player player = player_gameObject.GetComponent<Player>();
        GameObject castle_gameObject = PhotonNetwork.Instantiate("castle", player_position, Quaternion.identity);
        Castle castle = castle_gameObject.GetComponent<Castle>();
        castle.player = player;
        GameObject farmer1 = PhotonNetwork.Instantiate("farmer", char_position, Quaternion.identity);
        GameObject farmer2 = PhotonNetwork.Instantiate("farmer", char_position, Quaternion.identity);
        Farmer farmer1_component = farmer1.GetComponent<Farmer>();
        Farmer farmer2_component = farmer2.GetComponent<Farmer>();
        farmer1_component.player = player;
        farmer2_component.player = player;
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        alivePlayerMap[newPlayer] = true;
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (alivePlayerMap.ContainsKey(otherPlayer))
        {
            alivePlayerMap.Remove(otherPlayer);
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MultiLobbyScene");
    }
    public void CallPlayerDead()
    {
        pv.RPC("RPCPlayerDead", RpcTarget.All);
    }
    [PunRPC]
    void RPCPlayerDead(PhotonMessageInfo info)
    {
        if (alivePlayerMap.ContainsKey(info.Sender))
        {
            alivePlayerMap[info.Sender] = false;
        }
        /* if(PhotonNetwork.IsMasterClient && CheackGameOver())
        {
            讓結果顯示或是讓player自己呼叫CheackGameOver 來展示結果
        } */
    }
    public bool CheackGameOver()
    {
        int aliveCount = 0;
        foreach (var kvp in alivePlayerMap)
        {
            if (kvp.Value) aliveCount++;
        }
        return aliveCount <= 1;
    }
}
