using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : PowerModule
{
    [Header("Reactor")]
    [SerializeField]
    private GameObject explosionPrefab;

    public FluidTank fuelTank;
    public float maxPowerOutput;
    public float driveconeConsumption;

    [SerializeField]
    private float fuelUnitEfficiency;

    new void Update()
    {
        base.Update();
    }

    public override void ModuleDestroyed()
    {
        base.ModuleDestroyed();
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(explosion, 30f);
    }

    private void FixedUpdate()
    {
        float targetPowerOutput = driveconeConsumption + (distributor.maxPower - distributor.power);

        if (fuelTank.fluid == "Hydrogen Fuel" && fuelTank.fluidLevel > 0 && moduleActive && fuelTank.moduleActive)
        {
            float output = Mathf.Min(targetPowerOutput, maxPowerOutput);
            fuelTank.fluidLevel -= output / fuelUnitEfficiency;
            distributor.power += output - driveconeConsumption;
        }

    }

}
