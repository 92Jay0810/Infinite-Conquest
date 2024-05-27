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
           // SceneManager.LoadScene("MultiLobbyScene");
        }
        else
        {
            //initGame();
        }
        initGame();
    }
    void Update()
    {

    }
    public void initGame()
    {
        initGround();
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
        for(int i =0; i <6; i++)
        {
            ground_Sprite[i].sprite = ground_resource[Random.Range(0, 6)];
        }
    }
    private void initPlayer()
    {

    }
}
