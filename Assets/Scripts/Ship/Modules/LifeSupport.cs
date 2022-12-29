using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSupport : PowerModule
{
    [Header("Life Support")]
    [SerializeField]
    private FluidTank o2Tank;
    [SerializeField]
    private FluidTank h2Tank;
    [SerializeField]
    private float efficiency;

    public float maxAtmosphereVolume;
    public float atmosphereVolume;

    // Update is called once per frame
    new void FixedUpdate()
    {
        if (atmosphereVolume < maxAtmosphereVolume && moduleActive)
        {
            if (o2Tank.fluid == "Oxygen" && o2Tank.fluidLevel > 0 && h2Tank.fluid == "Hydrogen" && h2Tank.fluidLevel > 0)
            {
                float correction = Mathf.Min(0.01f, maxAtmosphereVolume - atmosphereVolume);
                atmosphereVolume += correction;
                o2Tank.fluidLevel -= correction * efficiency;
                h2Tank.fluidLevel -= correction * efficiency;
            }
        }
    }
}
