using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveCone : PowerModule
{
    [Header("Drive Cone")]
    [SerializeField]
    private Reactor reactor;
    [SerializeField]
    private Thruster driveCone;
    [SerializeField]
    private float maxDriveConeConsumption;

    new void Update()
    {
        if (!reactor.moduleActive || destroyed) { moduleActive = false; }

        base.Update();
        if (moduleActive)
        {
            reactor.driveconeConsumption = maxDriveConeConsumption * driveCone.thrustLevel;
        }
        else
        {
            reactor.driveconeConsumption = 0;
        }
        

        driveCone.isActive = moduleActive;
    }

}
