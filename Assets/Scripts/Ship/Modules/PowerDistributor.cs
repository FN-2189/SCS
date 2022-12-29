using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDistributor : PowerModule
{
    [Header("Power Distributor")]
    public float power = 5;
    public float maxPower;

    new void Update()
    {
        base.Update();
    }
}
