using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Missile : MonoBehaviour
{
    public float thrust;
    public float torque;
    public Transform target;
    Rigidbody rb;
    Vector3 currentTorque;
    public float maxSpin;
    public float threshold;
    float thrustLevel = 0;

    Thruster engine;
    [SerializeField]
    ThrusterType type;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        engine = GetComponentInChildren<Thruster>();
        engine.type = this.type;
    }

    private void Update()
    {
        Vector3 dir = (target.position - transform.position).normalized - transform.forward;
        Vector3 localDir = transform.InverseTransformDirection(dir);
        Vector3 rotDist = Quaternion.LookRotation(localDir).eulerAngles;
        if (rotDist.x > 180) rotDist.x -= 360;
        if (rotDist.y > 180) rotDist.y -= 360;
        currentTorque = rotDist/180 * torque;

        Vector3 localAngularV = transform.InverseTransformVector(rb.angularVelocity).normalized * rb.angularVelocity.magnitude;

        if(localAngularV.y > maxSpin && currentTorque.y > 0) currentTorque.y = 0;
        else if(localAngularV.y < -maxSpin && currentTorque.y < 0) currentTorque.y = 0;

        if (localAngularV.x > maxSpin && currentTorque.x > 0) currentTorque.x = 0;
        else if (localAngularV.x < -maxSpin && currentTorque.x < 0) currentTorque.x = 0;

        if (localDir.magnitude < threshold) engine.thrustLevel = 1;
        else engine.thrustLevel = 0;

        //Debug.Log("E: " + localDir + " " + currentTorque);
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * engine.thrust);
        rb.AddTorque(transform.TransformVector(currentTorque));
        //transform.rotation = Quaternion.LookRotation((target.position - transform.position).normalized);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit " + collision.gameObject.name);
    }
}
