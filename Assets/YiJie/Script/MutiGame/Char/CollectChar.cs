using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CollectChar : MonoBehaviourPunCallbacks
{
    [SerializeField] protected int hp = 20;
    [SerializeField] protected int collectPower = 5;
    [SerializeField] protected float collectRange = 1.0f;
    [SerializeField] protected float moveSpeed = 5.0f;
    [SerializeField] protected float collectInterval = 5.0f;
    protected SpriteRenderer spireRender;
    protected Transform collect_point_left; // 採集發射點
    protected Transform collect_point_right; // 採集發射點
    protected Text hp_text;
    protected Animator am;
    protected bool isSelected = false; //是否有被選中
    protected bool isRun = false;
    protected Vector2 RunTarget;
    protected float collectTimer = 0.0f; // 上次採集時間

    public Player player;
    //photon
    private PhotonView pv;
    virtual protected void Start()
    {
        spireRender = GetComponent<SpriteRenderer>();
        collect_point_left = transform.Find("collect_point_left").GetComponent<Transform>();
        collect_point_right = transform.Find("collect_point_right").GetComponent<Transform>();
        hp_text = transform.Find("hp_canva/hp_int").GetComponent<Text>();
        am = GetComponent<Animator>();
        updateHp_text();
        //photon
        pv = this.gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (pv.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectCharacter();
            }
            if (isRun && !isSelected)
            {
                run();
            }
            //判斷觸發採集動畫，並累積採集時間
            DetectAnimationOfCollect_AccumulationCollectTimer();
            if (!isRun && !isSelected && collectTimer >= collectInterval)
            {
                CollectResource();
                collectTimer = 0.0f;
            }
            if (player != null && !player.alive)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
    protected void SelectCharacter()
    {
        bool change = false;
        //hit角色，角色UI在default
        int charMask = LayerMask.GetMask("Default");
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10f, charMask);
        if (hit.collider != null)
        {
            // 滑鼠點擊到角色就觸發選中或不選中
            if (hit.collider.gameObject == gameObject)
            {
                isSelected = !isSelected;
                change = true;
                if (isSelected)
                {
                    am.SetBool("run", false);
                    am.SetBool("select", true);
                }
                else
                {
                    am.SetBool("select", false);
                }
                Debug.Log(" isSelected：" + isSelected.ToString());
            }
        }
        if (!change)
        {
            //hit場景，場景在layer3
            int layerMask = LayerMask.GetMask("ground");
            RaycastHit2D hitGround = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10f, layerMask);
            if (hitGround.collider != null)
            {
                if (hitGround.collider.gameObject.tag == "ground")
                {
                    // 點擊到場地就移動
                    if (isSelected)
                    {
                        isSelected = false;
                        am.SetBool("select", false);
                        am.SetBool("run", true);
                        Debug.Log("move");
                        RunTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        isRun = true;
                        // 判斷方向來改變 spriteRenderer 的 flipX
                        if (RunTarget.x < transform.position.x)
                        {
                            // 目標在左邊
                            spireRender.flipX = false;
                        }
                        else
                        {
                            // 目標在右邊
                            spireRender.flipX = true;
                        }
                    }
                }
            }
        }
    }
    protected void run()
    {
        // 計算距離
        float distanceToTarget = Vector2.Distance(transform.position, RunTarget);
        if (distanceToTarget > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, RunTarget, moveSpeed * Time.deltaTime);
        }
        else
        {
            isRun = false;
            am.SetBool("run", false);
        }
    }
    protected void CollectResource()
    {
        //射線方向
        Vector2 direction = spireRender.flipX == true ? transform.right : -transform.right;
        Transform collectPoint = spireRender.flipX == true ? collect_point_right : collect_point_left;
        Debug.DrawRay(collectPoint.position, direction * collectRange, Color.red);
        int layerMask = LayerMask.GetMask("Default");
        RaycastHit2D hit = Physics2D.Raycast(collectPoint.position, direction, collectRange, layerMask);
        //有射中東西的話
        if (hit.collider != null)
        {
            //如果射中目標，有對應標籤，就採集
            if (hit.collider.CompareTag("tree"))
            {
                Debug.Log("collect wood");
                player.ChangeWood(collectPower, false, false);
            }
            if (hit.collider.CompareTag("stone"))
            {
                Debug.Log("collect rock");
                player.ChangeRock(collectPower, false, false);
            }
            if (hit.collider.CompareTag("steel"))
            {
                Debug.Log("collect iron");
                player.ChangeIron(collectPower, false, false);
            }
            if (hit.collider.CompareTag("gold"))
            {
                Debug.Log("collect coin");
                player.ChangeCoin(collectPower, false, false);
            }
            if (hit.collider.CompareTag("wheat"))
            {
                Debug.Log("collect food ");
                player.ChangeFood(collectPower, false, false);
            }
        }
    }
    protected void DetectAnimationOfCollect_AccumulationCollectTimer()
    {
        //射線方向
        Vector2 direction = spireRender.flipX == true ? transform.right : -transform.right;
        Transform collectPoint = spireRender.flipX == true ? collect_point_right : collect_point_left;
        int layerMask = LayerMask.GetMask("Default");
        RaycastHit2D hit = Physics2D.Raycast(collectPoint.position, direction, collectRange, layerMask);
        //有射中東西的話
        if (hit.collider != null)
        {
            bool chahge = false;
            //如果射中目標，有對應標籤，就採集
            if (hit.collider.CompareTag("tree") || hit.collider.CompareTag("stone") || hit.collider.CompareTag("steel") || hit.collider.CompareTag("gold") || hit.collider.CompareTag("wheat"))
            {
                chahge = true;
                collectTimer += Time.deltaTime;
            }
            if (chahge)
            {
                am.SetBool("run", false);
                am.SetBool("collect", true);
            }
        }
        else
        {
            am.SetBool("collect", false);
            collectTimer = 0.0f;
        }
    }
    protected void OnCollisionEnter2D(Collision2D collision)
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
    protected void UpdateHealth(int newHp)
    {
        hp = newHp;
        updateHp_text();
    }
    protected void updateHp_text()
    {
        hp_text.text = hp.ToString();
    }

    [PunRPC]
    protected void DestroyBullet(int bulletViewID)
    {
        PhotonView bulletView = PhotonView.Find(bulletViewID);
        if (bulletView != null && bulletView.IsMine)
        {
            PhotonNetwork.Destroy(bulletView.gameObject);
        }
    }
}

