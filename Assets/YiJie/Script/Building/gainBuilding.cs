using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gainBuilding : MonoBehaviour
{
    [SerializeField] protected int hp = 200;
    [SerializeField] protected int gainPower = 10;
    [SerializeField] protected float gainInterval = 5.0f;
    protected Text hp_text;
    protected Player player;
    protected float lastGainCounter;
    virtual protected void Start()
    {
        hp_text = transform.Find("hp_canva/hp_int").GetComponent<Text>();
        player = transform.parent.GetComponent<Player>();
        updateHp_text();
    }

    protected void Update()
    {
        lastGainCounter += Time.deltaTime;
        if (lastGainCounter >= gainInterval)
        {
            lastGainCounter = 0.0f;
            GainResource();
        }
    }
    virtual protected void GainResource()
    {
       
    }
    protected void OnCollisionEnter2D(Collision2D collision)
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
    protected void updateHp_text()
    {
        hp_text.text = hp.ToString();
    }
}
