using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCS_System : PowerModule
{
    [SerializeField]
    private Thruster[] thrusters;

    new void Update()
    {
        base.Update();

        foreach (Thruster t in thrusters)
        {
            t.isActive = moduleActive;
        }
     }
}
