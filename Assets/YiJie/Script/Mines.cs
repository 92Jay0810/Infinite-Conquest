using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Mines : MonoBehaviour
{
    [SerializeField] int hp = 200;
    [SerializeField] protected int gainPower =  10;
    [SerializeField] protected float gainInterval = 5.0f; 
    Text hp_text;
    private Player player;
    private float lastGainCounter;
    private
    void Start()
    {
        hp_text = transform.Find("hp_canva/hp_int").GetComponent<Text>();
        player = transform.parent.GetComponent<Player>();
        updateHp_text();
    }

    void Update()
    {
        lastGainCounter += Time.deltaTime;
        if (lastGainCounter >= gainInterval)
        {
            lastGainCounter = 0.0f;
            GainResource();
        }
    }
    private void GainResource()
    {
        player.ChangeWood(gainPower);
        player.ChangeRock(gainPower);
        player.ChangeIron(gainPower);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "fireball")
        {
            Debug.Log("hp--");
            hp = hp - 15;
            updateHp_text();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "chopping")
        {
            Debug.Log("hp--");
            hp = hp - 7;
            updateHp_text();
            Destroy(collision.gameObject);
        }
    }
    void updateHp_text()
    {
        hp_text.text = hp.ToString();
    }
}
