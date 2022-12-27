using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldspaceGravityObject : MonoBehaviour
{
    public Vector3 gravity;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(gravity, ForceMode.Acceleration);
    }
}
