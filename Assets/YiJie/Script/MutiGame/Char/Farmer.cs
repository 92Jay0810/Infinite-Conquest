using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Farmer : CollectChar
{

    protected override void Start()
    {
        hp = 30;
        collectPower = 15;
        collectRange = 1.0f;
        moveSpeed = 7.0f;
        collectInterval = 5.0f;
        base.Start();
    }
}

