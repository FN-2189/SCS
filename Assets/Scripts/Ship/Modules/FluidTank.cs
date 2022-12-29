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

    public float fluidLevel;
    public string fluid;

    void Start()
    {

    }

    new void Update()
    {
        base.Update();   
    }

    public override void ModuleDestroyed()
    {
        GameObject leak = Instantiate(leakPrefab, transform);

        Destroy(leak);
    }
}
