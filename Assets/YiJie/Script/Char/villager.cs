using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class villager : CollectChar
{

    protected override void Start()
    {
        hp = 20;
        collectPower = 5;
        collectRange = 1.0f;
        moveSpeed = 5.0f;
        collectInterval = 5.0f;
        base.Start();
    }
}

