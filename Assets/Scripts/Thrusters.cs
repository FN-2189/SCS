using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrusters : MonoBehaviour
{
    [SerializeField]
    private Thruster[] thrusters;

    private Vector3 thrustVector;
    private Vector3 thrustPosition;

    [SerializeField]
    private Rigidbody rb;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        thrustVector = Vector3.zero;
        thrustPosition = Vector3.zero;
        foreach(Thruster t in thrusters)
        {
            thrustVector += t.rotation * t.thrust;
            thrustPosition += t.position * t.thrust;
        }

        rb.AddForceAtPosition(thrustVector, thrustPosition);
    }
}
