using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    [SerializeField] int hp = 250;
    [SerializeField] Text hp_text;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            Debug.Log("hp--");
            hp = hp - 15;
            updateHp_text();
            Destroy(collision.gameObject);
        }
    }
    void updateHp_text()
    {
        hp_text.text = hp.ToString();
    }
}
