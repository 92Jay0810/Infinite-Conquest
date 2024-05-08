using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magician : AttackChar
{
    protected override void Start()
    {
        hp = 35;
        attackRange = 5f;
        attackInterval = 4f;
        moveSpeed = 5.0f;
        base.Start();
    }

}