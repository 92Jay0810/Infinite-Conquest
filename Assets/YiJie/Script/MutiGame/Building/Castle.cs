using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Castle : MonoBehaviourPunCallbacks
{
    [SerializeField] int hp = 250;
    Text hp_text;
    public Player player;
    private PhotonView pv;
    void Start()
    {
        hp_text = transform.Find("hp_canva/hp_int").GetComponent<Text>();
        updateHp_text();
        pv = this.gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            if (hp <= 0)
            {
                player.Dead();
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (pv.IsMine)
        {
            int damage = 0;
            //判斷是否為子彈
            switch (collision.gameObject.tag)
            {
                case "fireball":
                    damage = 15;
                    break;
                case "chopping":
                    damage = 7;
                    break;
            }
            if (damage > 0)
            {
                //不是我的子彈，代表被攻擊
                Bullet bullet = collision.gameObject.GetComponent<Bullet>();
                if (!bullet.pv.IsMine)
                {
                    Debug.Log("hp--");
                    pv.RPC("UpdateHealth", RpcTarget.AllBuffered, hp - damage);
                    // 銷毀子彈
                    pv.RPC("DestroyBullet", RpcTarget.AllBuffered, collision.gameObject.GetPhotonView().ViewID);
                }
            }
        }
    }
    [PunRPC]
    void UpdateHealth(int newHp)
    {
        hp = newHp;
        updateHp_text();
    }
    void updateHp_text()
    {
        hp_text.text = hp.ToString();
    }
    [PunRPC]
    void DestroyBullet(int bulletViewID)
    {
        PhotonView bulletView = PhotonView.Find(bulletViewID);
        if (bulletView != null && bulletView.IsMine)
        {
            PhotonNetwork.Destroy(bulletView.gameObject);
        }
    }
}
