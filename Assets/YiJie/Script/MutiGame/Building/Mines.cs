using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Mines : gainBuilding
{
    protected override void Start()
    {
        hp = 200;
        gainPower = 10;
        gainInterval = 5.0f;
        base.Start();
    }
    protected override void GainResource()
    {
        player.ChangeWood(gainPower, true, false);
        player.ChangeRock(gainPower, true, false);
        player.ChangeIron(gainPower, true, false);
    }
}
