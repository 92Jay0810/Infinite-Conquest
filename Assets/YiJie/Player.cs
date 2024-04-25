using System.Collections;
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
    private int wood = 0;
    private int rock = 0;
    private int iron = 0;
    private int coin = 0;
    private int food = 0;
    Image button_menu;
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
        button_menu = transform.Find("Camera/create_canva/button_menu").GetComponent<Image>();
        build_menu = transform.Find("Camera/create_canva/build_menu").GetComponent<Image>();
        train_menu = transform.Find("Camera/create_canva/train_menu").GetComponent<Image>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CreateMenuDetected();
        }
        //若 train_menu打開的話，就持續檢查是否達成條件
        if (train_menu.gameObject.activeSelf)
        {
            Train_menu_detect();
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
    public void TrainButton()
    {
        button_menu.gameObject.SetActive(false);
        train_menu.gameObject.SetActive(true);

    }
    private void Train_menu_detect()
    {
        if (food >= 35)
        {
            train_buttons[0].interactable = true;
        }
        else
        {
            train_buttons[0].interactable = false;
        }
        if (coin >= 10 && food >= 50)
        {
            train_buttons[1].interactable = true;
        }
        else
        {
            train_buttons[1].interactable = false;
        }
    }
    public void CreateFarmer()
    {
        ChangeFood(-35);
        Instantiate(train_prefab[0], lastCallMenuPosition, Quaternion.identity, transform);
        train_menu.gameObject.SetActive(false);
    }
    public void CreateMagician()
    {
        ChangeFood(-50);
        ChangeCoin(-10);
        Instantiate(train_prefab[1], lastCallMenuPosition, Quaternion.identity, transform);
        train_menu.gameObject.SetActive(false);
    }
    public void Cross()
    {
        build_menu.gameObject.SetActive(false);
        train_menu.gameObject.SetActive(false);
    }
}
