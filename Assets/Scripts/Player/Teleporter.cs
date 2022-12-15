using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private Vector3[] targetPositions;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // oh no i'm becoming yanderedev
        if (InputManager.SwitchPilot)
        {
            transform.position = targetPositions[0];
        }
        else if (InputManager.SwitchGunner)
        {
            transform.position = targetPositions[1];
        }
        else if (InputManager.SwitchCommander)
        {
            transform.position = targetPositions[2];
        }
        else if (InputManager.SwitchEngineer)
        {
            transform.position = targetPositions[3];
        }
    }
}
