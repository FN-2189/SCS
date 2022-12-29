using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidTank : PowerModule
{
    [Header("Fluid Tank")]
    [SerializeField]
    private GameObject leakPrefab;

    [SerializeField]
    public float maxFluidLevel;

    public double fluidLevel;
    public string fluid;

    new void Update()
    {
        base.Update();   
    }

    public override void ModuleDestroyed()
    {
        base.ModuleDestroyed();
        GameObject leak = Instantiate(leakPrefab, transform);

        Destroy(leak);
    }
}
