using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : PowerModule
{
    [Header("Reactor")]
    [SerializeField]
    private GameObject explosionPrefab;

    public FluidTank fuelTank;
    public float powerOutput;
    public float driveconeConsumption;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public override void ModuleDestroyed()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(explosion, 30f);
    }

    private void FixedUpdate()
    {
        if (fuelTank.fluid == "Hydrogen Fuel" && fuelTank.fluidLevel > 0 && distributor.power < distributor.maxPower && moduleActive)
        {
            fuelTank.fluidLevel -= 0.001f;
            distributor.power += powerOutput - driveconeConsumption;
        }
    }

}
