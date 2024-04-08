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

    }
   public  void changeWood(int number)
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
}
