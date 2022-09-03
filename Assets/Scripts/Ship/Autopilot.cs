using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autopilot : MonoBehaviour
{
    private ThrustManager tm;
    [SerializeField] private bool autopilotActive;
    [SerializeField] private Vector3 targetVector;
    [SerializeField] private float maxRotationSpeed;
    [SerializeField] private float zFactor; // temp name

    private Rigidbody rb;

    [SerializeField] private LineRenderer testLine;
    [SerializeField] private LineRenderer targetLine;

    private void Awake()
    {
        tm = GetComponentInChildren<ThrustManager>();
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        testLine.startColor = Color.blue;
        testLine.endColor = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        if (autopilotActive)
        {
            Vector3 direction = targetVector.normalized - transform.forward;
            testLine.SetPositions(new Vector3[] { transform.position, transform.position + direction });
            targetLine.SetPositions(new Vector3[] { transform.position, transform.position + targetVector.normalized });

            Vector3 localDirection = transform.InverseTransformDirection(direction).normalized;
            Vector3 localAngularVelocity = transform.InverseTransformDirection(rb.angularVelocity).normalized * rb.angularVelocity.magnitude;
            Debug.Log(localDirection);
            if (!tm.isManual(Axis.Yaw)) tm.SetManual(Axis.Yaw, true);
            if (!tm.isManual(Axis.Pitch)) tm.SetManual(Axis.Pitch, true);

            float maxSpeed = maxRotationSpeed * (Mathf.Abs(localDirection.z) * zFactor);

            if(Mathf.Abs(localDirection.z) > 0f)
            {
                
                if(localDirection.x > 0f && localAngularVelocity.y < maxSpeed)
                {
                    tm.SetThrust(Axis.Yaw, -1f);
                }
                else if(localDirection.x < 0f && localAngularVelocity.y > -maxSpeed)
                {
                    tm.SetThrust(Axis.Yaw, 1f);
                }
                else tm.SetThrust(Axis.Yaw, 0f);
                
                if (localDirection.y > 0f && localAngularVelocity.x > -maxSpeed)
                {
                    tm.SetThrust(Axis.Pitch, -1f);
                }
                else if (localDirection.y < 0f && localAngularVelocity.x < maxSpeed)
                {
                    tm.SetThrust(Axis.Pitch, 1f);
                }
                else tm.SetThrust(Axis.Pitch, 0f);

            }
        }
        else
        {
            if (tm.isManual(Axis.Yaw)) tm.SetManual(Axis.Yaw, false);
            if (tm.isManual(Axis.Pitch)) tm.SetManual(Axis.Pitch, false);
        }
    }
}
