using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soilder : AttackChar
{
    protected override void Start()
    {
         hp = 40;
         attackRange = 0.3f;
         attackInterval = 3f;
        moveSpeed = 4.0f;
        bulletPrefab_Name = "chopping";
        base.Start();
    }

}
