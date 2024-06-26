﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float collectRange = 1.0f;
    [SerializeField] float collectInterval = 2.0f;
    [SerializeField] Transform collectPoint; // 採集發射點
    private SpriteRenderer spireRender;
    private Animator am;
    private bool isSelected = false; //是否有被選中
    private bool isRun = false;
    private Vector2 RunTarget;
    private float collectTimer = 0.0f; // 上次採集時間
    void Start()
    {
        spireRender = GetComponent<SpriteRenderer>();
        am = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SelectCharacter();
        if (isRun && !isSelected)
        {
            run();
        }
        collectTimer += Time.deltaTime;
        if (!isRun && !isSelected && collectTimer >= collectInterval)
        {
            CollectDetect();
            collectTimer = 0.0f;
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
    void CollectDetect()
    {
        //射線方向
        Vector2 direction = spireRender.flipX == true ? transform.right : -transform.right;
        Debug.DrawRay(collectPoint.position, direction * collectRange, Color.red);
        int layerMask = LayerMask.GetMask("Default");
        RaycastHit2D hit = Physics2D.Raycast(collectPoint.position, direction, collectRange, layerMask);
        //有射中東西的話
        if (hit.collider != null)
        {
            //如果射中目標，有food標籤，就採集
            if (hit.collider.CompareTag("food"))
            {
                Debug.Log("collect");
                am.SetBool("run", false);
                am.SetBool("collect", true);
            }
        }
        else
        {
            am.SetBool("collect", false);
        }
    }
}
