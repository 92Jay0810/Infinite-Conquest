﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Text wood_text;
    Text rock_text;
    Text iron_text;
    Text coin_text;
    Text food_text;
    Text research_text;
    RawImage current_generation_Image;
    [SerializeField] Sprite[] generations_Image;
    private int wood = 0;
    private int rock = 0;
    private int iron = 0;
    private int coin = 0;
    private int food = 0;
    private int research = 0;
    private int generation = 1;
    Image button_menu;
    Button update_generation_button;
    Image build_menu;
    Image train_menu;
    [SerializeField] Button[] train_buttons;
    [SerializeField] GameObject[] train_prefab;
    private Vector3 lastCallMenuPosition;

    void Start()
    {
        wood_text = transform.Find("Camera/resource_canva/wood/wood_int").GetComponent<Text>();
        rock_text = transform.Find("Camera/resource_canva/rock/rock_int").GetComponent<Text>();
        iron_text = transform.Find("Camera/resource_canva/iron/iron_int").GetComponent<Text>();
        coin_text = transform.Find("Camera/resource_canva/coin/coin_int").GetComponent<Text>();
        food_text = transform.Find("Camera/resource_canva/food/food_int").GetComponent<Text>();
        research_text = transform.Find("Camera/resource_canva/research/research_int").GetComponent<Text>();
        current_generation_Image = transform.Find("Camera/resource_canva/research/generation").GetComponent<RawImage>();
        button_menu = transform.Find("Camera/create_canva/button_menu").GetComponent<Image>();
        update_generation_button = transform.Find("Camera/create_canva/button_menu/UpdateGenerationButton").GetComponent<Button>();
        build_menu = transform.Find("Camera/create_canva/build_menu").GetComponent<Image>();
        train_menu = transform.Find("Camera/create_canva/train_menu").GetComponent<Image>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CreateMenuDetected();
        }
        //若 build_menu打開的話，就持續檢查是否達成升級世代的條件
        if (button_menu.gameObject.activeSelf)
        {
            UpdateGenerationButtonDetected();
        }
        //若 train_menu打開的話，就持續檢查是否達成條件
        if (train_menu.gameObject.activeSelf)
        {
            Train_menu_detect();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeWood(500);
            ChangRock(500);
            ChangeIron(500);
            ChangeCoin(500);
            ChangeFood(500);
            ChangeResearch(500);
        }
    }
    public void ChangeWood(int number)
    {
        wood += number;
        wood_text.text = wood.ToString();
    }
    public void ChangRock(int number)
    {
        rock += number;
        rock_text.text = rock.ToString();
    }
    public void ChangeIron(int number)
    {
        iron += number;
        iron_text.text = iron.ToString();
    }
    public void ChangeCoin(int number)
    {
        coin += number;
        coin_text.text = coin.ToString();
    }
    public void ChangeFood(int number)
    {
        food += number;
        food_text.text = food.ToString();
    }
    public void ChangeResearch(int number)
    {
        research += number;
        research_text.text = research.ToString();
    }

    void CreateMenuDetected()
    {
        Vector3 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //不要hit到Default層的場景
        int charMask = LayerMask.GetMask("Default");
        RaycastHit2D hit = Physics2D.Raycast(inputPosition, Vector2.zero, 10f, charMask);
        if (!hit)
        {
            //hit場景，場景在layer3
            int layerMask = LayerMask.GetMask("ground");
            RaycastHit2D hitGround = Physics2D.Raycast(inputPosition, Vector2.zero, 10f, layerMask);
            if (hitGround.collider != null)
            {
                if (hitGround.collider.tag == "ground")
                {
                    //點擊到地板，就顯示按鈕
                    button_menu.gameObject.transform.position = new Vector3(inputPosition.x, inputPosition.y, 0f);
                    button_menu.gameObject.SetActive(!button_menu.gameObject.activeSelf);
                    //並更新開啟菜單位置
                    lastCallMenuPosition = new Vector3(inputPosition.x, inputPosition.y, 0f);
                }
            }
        }
    }
    public void BuildButton()
    {
        button_menu.gameObject.SetActive(false);
        build_menu.gameObject.SetActive(true);

    }
    void UpdateGenerationButtonDetected()
    {
        bool update_generation = false;
        switch (generation)
        {
            case 1:
                if (food >= 500)
                {
                    update_generation = true;
                }
                break;
            case 2:
                if (coin >= 200 && food >= 800)
                {
                    update_generation = true;
                }
                break;
            case 3:
                if (coin >= 800 && food >= 1000)
                {
                    update_generation = true;
                }
                break;
            case 4:
                if (coin >= 1500 && food >= 1200)
                {
                    update_generation = true;
                }
                break;
            case 5:
                break;
            default:
                Debug.Log("時代按鈕有錯");
                break;
        }
        if (update_generation)
        {
            update_generation_button.interactable = true;
        }
        else
        {
            update_generation_button.interactable = false;
        }
    }
    public void UpdateGenerationButton()
    {
        switch (generation)
        {
            case 1:
                ChangeFood(-500);
                current_generation_Image.texture = generations_Image[1].texture;
                break;
            case 2:
                ChangeCoin(-200);
                ChangeFood(-800);
                current_generation_Image.texture = generations_Image[2].texture;
                break;
            case 3:
                ChangeCoin(-800);
                ChangeFood(-1000);
                current_generation_Image.texture = generations_Image[3].texture;
                break;
            case 4:
                ChangeCoin(-1500);
                ChangeFood(-1200);
                current_generation_Image.texture = generations_Image[4].texture;
                break;
            default:
                Debug.Log("時代有錯");
                break;
        }
        generation += 1;
        update_generation_button.interactable = false;
        button_menu.gameObject.SetActive(false);
    }
    public void TrainButton()
    {
        button_menu.gameObject.SetActive(false);
        train_menu.gameObject.SetActive(true);
    }
    private void Train_menu_detect()
    {
        //士兵
        if (food >= 30)
        {
            train_buttons[0].interactable = true;
        }
        else
        {
            train_buttons[0].interactable = false;
        }
        //農民
        if (food >= 35 && generation >= 2)
        {
            train_buttons[1].interactable = true;
        }
        else
        {
            train_buttons[1].interactable = false;
        }
        //魔法師
        if (coin >= 10 && food >= 50 && generation >= 3)
        {
            train_buttons[2].interactable = true;
        }
        else
        {
            train_buttons[2].interactable = false;
        }
    }
    public void CreateFarmer()
    {
        ChangeFood(-35);
        Instantiate(train_prefab[1], lastCallMenuPosition, Quaternion.identity, transform);
        train_menu.gameObject.SetActive(false);
    }
    public void CreateMagician()
    {
        ChangeFood(-50);
        ChangeCoin(-10);
        Instantiate(train_prefab[2], lastCallMenuPosition, Quaternion.identity, transform);
        train_menu.gameObject.SetActive(false);
    }
    public void CreateSoldier()
    {
        ChangeFood(-30);
        Instantiate(train_prefab[0], lastCallMenuPosition, Quaternion.identity, transform);
        train_menu.gameObject.SetActive(false);
    }
    public void Cross()
    {
        build_menu.gameObject.SetActive(false);
        train_menu.gameObject.SetActive(false);
    }
}