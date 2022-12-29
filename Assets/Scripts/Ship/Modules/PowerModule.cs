using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerModule : ShipModule
{
    [Header("Power Module")]
    public PowerDistributor distributor;
    public float consumptionMultiplier = 1f;
    public float powerConsumption;
    public bool moduleActive = true;

    private void Awake()
    {
        distributor = transform.parent.GetComponentInChildren<PowerDistributor>();
        if(distributor == null) distributor = transform.parent.parent.GetComponentInChildren<PowerDistributor>();
    }

    new void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        if (distributor.power > powerConsumption && moduleActive)
        {
            distributor.power -= powerConsumption;
        }
        else
        {
            moduleActive = false;
        }
    }
}
