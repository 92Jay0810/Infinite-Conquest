using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    [SerializeField] int hp = 250;
    Text hp_text;
    public Player player;
    void Start()
    {
        hp_text = transform.Find("hp_canva/hp_int").GetComponent<Text>();
        updateHp_text();
    }

    // Update is called once per frame
    void Update()
    {

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
