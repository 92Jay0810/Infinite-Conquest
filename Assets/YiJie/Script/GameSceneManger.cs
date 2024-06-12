using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class GameSceneManger : MonoBehaviour
{
    [SerializeField] Sprite[] ground_resource;
    SpriteRenderer[] ground_Sprite;
    void Start()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("MultiLobbyScene");
        }
        else
        {
            initGame();
        }
        //initGame();
    }
    void Update()
    {

    }
    public void initGame()
    {
        initGround();
        initPlayer();
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
        for (int i = 0; i < 6; i++)
        {
            ground_Sprite[i].sprite = ground_resource[Random.Range(0, 6)];
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
}
