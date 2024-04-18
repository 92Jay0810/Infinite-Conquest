using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] Text wood_text;
    [SerializeField] Text stone_text;
    [SerializeField] Text iron_text;
    [SerializeField] Text coin_text;
    [SerializeField] Text food_text;
    [SerializeField] Button buildButton;
    [SerializeField] Image train_menu;
    private int wood = 0;
    private int stone = 0;
    private int iron = 0;
    private int coin = 0;
    private int food = 0;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CreateMenuDetected();
        }
    }
    public void changeWood(int number)
    {
        wood += number;
        wood_text.text = wood.ToString();
    }
    public void changStone(int number)
    {
        stone += number;
        stone_text.text = stone.ToString();
    }
    public void changeIron(int number)
    {
        iron += number;
        iron_text.text = iron.ToString();
    }
    public void changeCoin(int number)
    {
        coin += number;
        coin_text.text = coin.ToString();
    }
    public void changeFood(int number)
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
                    buildButton.gameObject.transform.position = new Vector3(inputPosition.x, inputPosition.y, 0f);
                    buildButton.gameObject.SetActive(!buildButton.gameObject.activeSelf);
                }
            }
        }
    }
    public void CreateButton()
    {
        buildButton.gameObject.SetActive(false);
        train_menu.gameObject.SetActive(true);
    }
}
