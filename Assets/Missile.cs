using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Missile : MonoBehaviour
{
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

    public LineRenderer l;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        engine = GetComponentInChildren<Thruster>();
        engine.type = this.type;
    }

    private void Update()
    {
        Vector3 targetV = target.GetComponent<Rigidbody>().velocity;

        Vector3 targetLead = Vector3.zero;
        l.SetPositions(new Vector3[] { transform.position, targetLead });
        print(target.GetComponent<Rigidbody>().velocity);

        Vector3 dir = (targetLead - transform.position).normalized - transform.forward;
        Vector3 localDir = transform.InverseTransformDirection(dir);

        Vector3 rotDist = Quaternion.LookRotation(localDir).eulerAngles;

        if (rotDist.x > 180) rotDist.x -= 360;
        if (rotDist.y > 180) rotDist.y -= 360;

        currentTorque = rotDist/180 * torque;

        float maxRotationSpeed = maxSpin * localDir.magnitude;

        Vector3 localAngularV = transform.InverseTransformVector(rb.angularVelocity).normalized * rb.angularVelocity.magnitude;

        if(localAngularV.y > maxRotationSpeed && currentTorque.y > 0) currentTorque.y = 0;
        else if(localAngularV.y < -maxRotationSpeed && currentTorque.y < 0) currentTorque.y = 0;

        if (localAngularV.x > maxRotationSpeed && currentTorque.x > 0) currentTorque.x = 0;
        else if (localAngularV.x < -maxRotationSpeed && currentTorque.x < 0) currentTorque.x = 0;

        if (localDir.magnitude < threshold) engine.thrustLevel = 1;
        else engine.thrustLevel = 0;

        //Debug.Log("E: " + localDir + " " + currentTorque);
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * engine.thrust, ForceMode.Acceleration);
        rb.AddTorque(transform.TransformVector(currentTorque), ForceMode.Acceleration);
        //transform.rotation = Quaternion.LookRotation((target.position - transform.position).normalized);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit " + collision.gameObject.name);
    }

    Vector3 GetHitPoint(Vector3 targetPosition, Vector3 targetSpeed, Vector3 targetAccel, Vector3 attackerPosition, float bulletSpeed)
    {
        Vector3 q = targetPosition - attackerPosition;

        //solving quadratic ecuation from t*t(Vx*Vx + Vy*Vy - S*S) + 2*t*(Vx*Qx)(Vy*Qy) + Qx*Qx + Qy*Qy = 0

        //float a = Vector3.Dot(targetSpeed, targetSpeed) - (bulletSpeed * bulletSpeed); //Dot is basicly (targetSpeed.x * targetSpeed.x) + (targetSpeed.y * targetSpeed.y)
        float a = 0;
        float b = 2 * Vector3.Dot(targetSpeed, q); //Dot is basicly (targetSpeed.x * q.x) + (targetSpeed.y * q.y)
        float c = Vector3.Dot(q, q); //Dot is basicly (q.x * q.x) + (q.y * q.y)

        //Discriminant
        float D = Mathf.Sqrt((b * b) - 4 * a * c);

        float t1 = (-b + D) / (2 * a);
        float t2 = (-b - D) / (2 * a);

        Debug.Log("t1: " + t1 + " t2: " + t2);

        float time = Mathf.Max(t1, t2);

        Vector3 ret = targetPosition + targetSpeed * time;
        return ret;
    }
}
