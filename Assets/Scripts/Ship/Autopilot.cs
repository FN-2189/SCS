using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autopilot : MonoBehaviour
{
    private ThrustManager tm;
    [SerializeField] public bool autopilotActive;
    [SerializeField] public bool decelerateAssistActive;
    [SerializeField] public Vector3 targetVector;
    [SerializeField] public Vector3 targetPosition;
    [SerializeField] public bool targetPosOrTargetVector;
    [SerializeField] private float maxRotationSpeed;
    [SerializeField] private float power;
    [SerializeField] private float tFactor; // temp name
    [SerializeField] private float zFactor; // temp name
    [SerializeField] private float threshold; // temp name

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

            if (targetPosOrTargetVector)
            {
                targetVector = targetPosition - transform.position;
            }

            if (decelerateAssistActive)
            {
                targetVector = -rb.velocity.normalized;
            }

            Vector3 direction = targetVector.normalized - transform.forward;
            testLine.SetPositions(new Vector3[] { transform.position, transform.position + direction });
            targetLine.SetPositions(new Vector3[] { transform.position, transform.position + targetVector.normalized });

            Vector3 localDirection = transform.InverseTransformDirection(direction).normalized;
            Vector3 localAngularVelocity = transform.InverseTransformDirection(rb.angularVelocity).normalized * rb.angularVelocity.magnitude;

            if (!tm.isManual(Axis.Yaw)) tm.SetManual(Axis.Yaw, true);
            if (!tm.isManual(Axis.Pitch)) tm.SetManual(Axis.Pitch, true);

            float maxSpeed = maxRotationSpeed * (Mathf.Abs(localDirection.z) * zFactor);
            float thrust = (-Mathf.Pow(tFactor, 1-direction.magnitude)+2)*power;
            if (Mathf.Abs(localDirection.z) > threshold)
            {
                
                if(localDirection.x > 0f && localAngularVelocity.y < maxSpeed)
                {
                    tm.SetThrust(Axis.Yaw, -thrust);
                }
                else if(localDirection.x < 0f && localAngularVelocity.y > -maxSpeed)
                {
                    tm.SetThrust(Axis.Yaw, thrust);
                }
                else tm.SetThrust(Axis.Yaw, 0f);
                
                if (localDirection.y > 0f && localAngularVelocity.x > -maxSpeed)
                {
                    tm.SetThrust(Axis.Pitch, -thrust);
                }
                else if (localDirection.y < 0f && localAngularVelocity.x < maxSpeed)
                {
                    tm.SetThrust(Axis.Pitch, thrust);
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
