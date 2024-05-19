using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Quarry : gainBuilding
{
    protected override void Start()
    {
        hp = 200;
        gainPower = 10;
        gainInterval = 7.0f;
        base.Start();
    }

    protected override void GainResource()
    {
        player.ChangeCoin(gainPower, true, false);
    }
}
