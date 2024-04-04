using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float attackRange = 5.0f;
    [SerializeField] GameObject bulletPrefab; // 子彈物件
    [SerializeField] Transform firePoint; // 子彈發射點
    [SerializeField] float fireInterval = 1f;
    private SpriteRenderer spireRender;
    private Animator am;
    private float lastFireTime = 0.0f; // 上次開火時間
    private bool isSelected = false; //是否有被選中
    private bool isRun = false;
    private Vector2 RunTarget;
    void Start()
    {
        spireRender = GetComponent<SpriteRenderer>();
        am = GetComponent<Animator>();
    }

    void Update()
    {
        lastFireTime += Time.deltaTime;
        //不在選取狀態下才檢測攻擊
        if (lastFireTime >= fireInterval && !isSelected && !isRun)
        {
            AttackDetect();
            lastFireTime = 0.0f;
        }
        SelectCharacter();
        if (isRun && !isSelected)
        {
            run();
        }
    }

    void AttackDetect()
    {
        //射線方向
        Vector2 direction = spireRender.flipX == true ? transform.right : -transform.right;
        Debug.DrawRay(firePoint.position, direction * attackRange, Color.red);
        int layerMask = LayerMask.GetMask("Default");
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, attackRange, layerMask);
        //有射中東西的話
        if (hit.collider != null)
        {
            //如果射中目標，有castle標籤，就攻擊
            if (hit.collider.CompareTag("castle"))
            {
                Debug.Log("attack");
                am.SetBool("run", false);
                am.SetBool("attack", true);
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(direction * 10f, ForceMode2D.Impulse);
            }
        }
    }
    void SelectCharacter()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bool change = false;
            //hit角色，角色UI在default
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
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
                        am.SetBool("attack", false);
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
                            am.SetBool("attack", false);
                            am.SetBool("run", true);
                            Debug.Log("move");
                            RunTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            isRun = true;
                        }
                    }
                }
            }
        }
    }
    void run()
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
}
