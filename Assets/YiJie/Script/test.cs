using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower;

public class test : MonoBehaviour
{
    FlowerSystem fs;
    void Start()
    {
        fs = FlowerManager.Instance.CreateFlowerSystem("default", false);
        fs.SetupDialog();
        fs.ReadTextFromResource("SingleMode/test");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fs.Next();
        }
    }
}
