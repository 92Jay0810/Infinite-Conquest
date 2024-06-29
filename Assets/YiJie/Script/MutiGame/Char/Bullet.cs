using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public PhotonView pv;
    float timer;
    void Start()
    {
        timer = 5f;
        pv = this.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (pv.IsMine)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                pv.RPC("DestroyBullet", RpcTarget.AllBuffered);
            }
        }
    }
    [PunRPC]
    void DestroyBullet()
    {
        if (pv.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
