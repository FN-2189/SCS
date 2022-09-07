using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ThrustManager : MonoBehaviour
{
    public Thruster[] thrusters;

    private Vector3 thrustVector;
    private Vector3 thrustPosition;
    [SerializeField]
    [Range(0f, 1f)]
    private float deadZone = 0.15f;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private InputManager input;

    [SerializeField] public bool flightAssistOn;

    public bool velFlightAssistOn;

    [SerializeField] [Range(0f, 1f)] private float flighAssistStrength;

    private Dictionary<Axis, bool> inManualMode;

    [SerializeField] private Axis[] manualAxes;

    void Awake()
    {
        thrusters = GetComponentsInChildren<Thruster>();
        inManualMode = new Dictionary<Axis, bool>();
        Array array = Enum.GetValues(typeof(Axis));
        foreach(Axis a in array) inManualMode[a] = false;
        foreach (Axis a in manualAxes) inManualMode[a] = true;
        Debug.Log(thrusters.Length);
    }

    private void Update()
    {
        //pitch
        for (int i = 0; i < thrusters.Length; i++)
        {
            if (thrusters[i].isInAxis[Axis.Pitch] && !inManualMode[Axis.Pitch])
            {
                if (Mathf.Abs(input.Stick.y) > deadZone) thrusters[i].thrustLevel = -input.Stick.y;
                else thrusters[i].thrustLevel = 0f;
            }
        }

        //yaw
        for (int i = 0; i < thrusters.Length; i++)
        {
            if (thrusters[i].isInAxis[Axis.Yaw] && !inManualMode[Axis.Yaw])
            {
                if (Mathf.Abs(input.Stick.x) > deadZone) thrusters[i].thrustLevel = -input.Stick.x;
                else thrusters[i].thrustLevel = 0f;
            }
        }

        //roll
        for (int i = 0; i < thrusters.Length; i++)
        {
            if (thrusters[i].isInAxis[Axis.Roll] && !inManualMode[Axis.Roll])
            {
                if (Mathf.Abs(input.Stick.z) > deadZone) thrusters[i].thrustLevel = -input.Stick.z;
                else thrusters[i].thrustLevel = 0f;
            }
        }

        //horizontal
        for (int i = 0; i < thrusters.Length; i++)
        {
            if (thrusters[i].isInAxis[Axis.Horizontal] && !inManualMode[Axis.Horizontal])
            {
                if (Mathf.Abs(input.Translate.x) > deadZone) thrusters[i].thrustLevel = -input.Translate.x;
                else thrusters[i].thrustLevel = 0f;
            }
        }

        //vertical
        for (int i = 0; i < thrusters.Length; i++)
        {
            if (thrusters[i].isInAxis[Axis.Vertical] && !inManualMode[Axis.Vertical])
            {
                if(Mathf.Abs(input.Translate.y) > deadZone) thrusters[i].thrustLevel = -input.Translate.y;
                else thrusters[i].thrustLevel = 0f;
            }
        }

        // thrust
        for (int i = 0; i < thrusters.Length; i++)
        {
            if (thrusters[i].isInAxis[Axis.Forward] && !inManualMode[Axis.Forward])
            {
                thrusters[i].thrustLevel = input.Throttle;
            }
        }

        if (flightAssistOn)
        {
            flightAssist();
        }
    }

    void FixedUpdate()
    {
        thrustVector = Vector3.zero;
        thrustPosition = Vector3.zero;
        for(int i = 0; i < thrusters.Length; i++)
        {
            Transform thrusterTransform = thrusters[i].transform;
            Quaternion rotation = thrusterTransform.rotation;
            float thrust = thrusters[i].thrust;
            rb.AddForceAtPosition(rotation * Vector3.forward * thrust, thrusterTransform.position);
            //rb.AddForceAtPosition((thrusters[i].rotation * transform.forward) * thrusters[i].thrust, (transform.rotation * thrusters[i].position) + transform.position);
        }
    }

    private void flightAssist()
    {
        // stick x:yaw, y:pitch, z:roll
        Vector3 angularV = transform.InverseTransformDirection(rb.angularVelocity).normalized * rb.angularVelocity.magnitude;
        Vector3 goalAngularVelocity = Vector3.zero;

        Vector3 v = transform.InverseTransformVector(rb.velocity);
        Vector3 goalV = Vector3.zero;

        //flight assist pitch
        if (Mathf.Abs(input.Stick.y) < deadZone && angularV.magnitude > 0.01)
        {
            if (angularV.x < goalAngularVelocity.x)
            {
                for (int i = 0; i < thrusters.Length; i++)
                {
                    if (thrusters[i].isInAxis[Axis.Pitch] && !inManualMode[Axis.Pitch])
                    {
                        thrusters[i].thrustLevel = flighAssistStrength;
                    }
                }
            }

            if (angularV.x > goalAngularVelocity.x)
            {
                for (int i = 0; i < thrusters.Length; i++)
                {
                    if (thrusters[i].isInAxis[Axis.Pitch] && !inManualMode[Axis.Pitch])
                    {
                        thrusters[i].thrustLevel = -flighAssistStrength;
                    }
                }
            }
        }

        //flight assist yaw
        if (Mathf.Abs(input.Stick.x) < deadZone && angularV.magnitude > 0.01)
        {
            if (angularV.y < goalAngularVelocity.y)
            {
                for (int i = 0; i < thrusters.Length; i++)
                {
                    if (thrusters[i].isInAxis[Axis.Yaw] && !inManualMode[Axis.Yaw])
                    {
                        thrusters[i].thrustLevel = -flighAssistStrength;
                    }
                }
            }

            if (angularV.y > goalAngularVelocity.y)
            {
                for (int i = 0; i < thrusters.Length; i++)
                {
                    if (thrusters[i].isInAxis[Axis.Yaw] && !inManualMode[Axis.Yaw])
                    {
                        thrusters[i].thrustLevel = flighAssistStrength;
                    }
                }
            }
        }

        //flight assist roll
        if (Mathf.Abs(input.Stick.z) < deadZone && angularV.magnitude > 0.01)
        {
            if (angularV.z < goalAngularVelocity.z)
            {
                for (int i = 0; i < thrusters.Length; i++)
                {
                    if (thrusters[i].isInAxis[Axis.Roll] && !inManualMode[Axis.Roll])
                    {
                        thrusters[i].thrustLevel = flighAssistStrength;
                    }
                }
            }

            if (angularV.z > goalAngularVelocity.z)
            {
                for (int i = 0; i < thrusters.Length; i++)
                {
                    if (thrusters[i].isInAxis[Axis.Roll] && !inManualMode[Axis.Roll])
                    {
                        thrusters[i].thrustLevel = -flighAssistStrength;
                    }
                }
            }
        }

        //Velocity Flight assist
        if (velFlightAssistOn)
        {
            //flight assist Horizontal
            if (Mathf.Abs(input.Translate.x) < deadZone)
            {
                if (v.x < goalV.x)
                {
                    for (int i = 0; i < thrusters.Length; i++)
                    {
                        if (thrusters[i].isInAxis[Axis.Horizontal] && !inManualMode[Axis.Horizontal])
                        {
                            thrusters[i].thrustLevel = -flighAssistStrength;
                        }
                    }
                }

                if (v.x > goalV.x)
                {
                    for (int i = 0; i < thrusters.Length; i++)
                    {
                        if (thrusters[i].isInAxis[Axis.Horizontal] && !inManualMode[Axis.Horizontal])
                        {
                            thrusters[i].thrustLevel = flighAssistStrength;
                        }
                    }
                }
            }

            //flight assist Vertical
            if (Mathf.Abs(input.Translate.y) < deadZone)
            {
                if (v.y < goalV.y)
                {
                    for (int i = 0; i < thrusters.Length; i++)
                    {
                        if (thrusters[i].isInAxis[Axis.Vertical] && !inManualMode[Axis.Vertical])
                        {
                            thrusters[i].thrustLevel = -flighAssistStrength;
                        }
                    }
                }

                if (v.y > goalV.y)
                {
                    for (int i = 0; i < thrusters.Length; i++)
                    {
                        if (thrusters[i].isInAxis[Axis.Vertical] && !inManualMode[Axis.Vertical])
                        {
                            thrusters[i].thrustLevel = flighAssistStrength;
                        }
                    }
                }
            }
        }
        

        Debug.Log(v.magnitude);
    }

    public void SetManual(Axis axis, bool manual)
    {
        inManualMode[axis] = manual;
        Debug.Log(axis + " set to " + manual);
    }

    public bool isManual(Axis axis)
    {
        return inManualMode[axis];
    }

    public void SetThrust(Axis axis, float thrust)
    {
        for(int i = 0; i < thrusters.Length; i++)
        {
            if (thrusters[i].isInAxis[axis]) thrusters[i].thrustLevel = thrust;
        }
    }
}
