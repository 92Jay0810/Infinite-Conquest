using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackChar : MonoBehaviour
{
    [SerializeField] protected int hp = 40;    
    [SerializeField] protected float attackRange = 1.0f;
    [SerializeField] protected float attackInterval = 3f;    
    [SerializeField] protected float moveSpeed = 4.0f;
    [SerializeField] protected  GameObject bulletPrefab; // 子彈物件
    protected SpriteRenderer spireRender;
    protected Transform firePoint_left; // 子彈發射點
    protected Transform firePoint_right; // 子彈發射點
    protected Text hp_text;
    protected Animator am;
    protected float lastFireTime = 0.0f; // 上次開火時間
    protected bool isSelected = false; //是否有被選中
    protected bool isRun = false;
    protected Vector2 RunTarget;
    virtual protected void Start()
    {
        spireRender = GetComponent<SpriteRenderer>();
        firePoint_left = transform.Find("fire_point_left").GetComponent<Transform>();
        firePoint_right = transform.Find("fire_point_right").GetComponent<Transform>();
        hp_text = transform.Find("hp_canva/hp_int").GetComponent<Text>();
        am = GetComponent<Animator>();
        updateHp_text();
    }

    protected void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectCharacter();
        }
        lastFireTime += Time.deltaTime;
        //不在選取狀態下才檢測攻擊
        if (lastFireTime >= attackInterval && !isSelected && !isRun)
        {
            AttackDetect();
        }
        if (isRun && !isSelected)
        {
            run();
        }
    }

    protected void AttackDetect()
    {
        //射線方向
        Vector2 direction = spireRender.flipX == true ? transform.right : -transform.right;
        Transform firePoint = spireRender.flipX == true ? firePoint_right : firePoint_left;
        Debug.DrawRay(firePoint.position, direction * attackRange, Color.red);
        int layerMask = LayerMask.GetMask("Default");
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, attackRange, layerMask);
        //有射中東西的話
        if (hit.collider != null)
        {
            //如果射中目標，有castle標籤，就攻擊
            if (hit.collider.CompareTag("castle") || hit.collider.CompareTag("soldier"))
            {
                Debug.Log("attack");
                lastFireTime = 0.0f;
                am.SetBool("run", false);
                am.SetBool("attack", true);
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(direction * 10f, ForceMode2D.Impulse);
            }
        }
    }
    protected  void SelectCharacter()
    {
        bool change = false;
        //hit角色，角色會在default
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
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            Debug.Log("hp--");
            hp = hp - 15;
            updateHp_text();
            Destroy(collision.gameObject);
        }
    }
    protected void updateHp_text()
    {
        hp_text.text = hp.ToString();
    }
}