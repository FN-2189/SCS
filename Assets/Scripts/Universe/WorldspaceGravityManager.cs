using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldspaceGravityManager : MonoBehaviour
{
    [SerializeField]
    private List<Rigidbody> affectedObjects;
    [SerializeField]
    private Planet[] gravitationalObjects;

    private const float G = 0.00000000000667f;

    // Update is called once per frame
    void FixedUpdate()
    {
        
        foreach (Rigidbody rb in affectedObjects)
        {
            Vector3 worldspaceGravity = Vector3.zero;
            foreach (Planet gravObject in gravitationalObjects)
            {
                double r = (gravObject.transform.position - rb.transform.position).magnitude;
                double a = (gravObject.mass / (r*r)) * G;

                worldspaceGravity += ((float)a) * (gravObject.transform.position - rb.transform.position).normalized;
            }

            rb.AddForce(worldspaceGravity, ForceMode.Acceleration);

            Debug.Log(rb + ": " + worldspaceGravity);
        }

    }
}