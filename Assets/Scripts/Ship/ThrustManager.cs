using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] public bool flightAssist;

    [SerializeField] [Range(0f, 1f)] private float flighAssistStrength;

    void Awake()
    {
        thrusters = GetComponentsInChildren<Thruster>();
        Debug.Log(thrusters.Length);
    }

    private void Update()
    {


        // get stick input (x,y -> pitch/yaw)

        // pitch
        
        if(Mathf.Abs(input.Stick.y) > deadZone)
        {
            for(int i = 0; i < thrusters.Length; i++)
            {
                if (thrusters[i].isInAxis[Axis.Pitch])
                {
                    thrusters[i].thrustLevel = input.Stick.y * -1;
                }
            }
        }
        
        else
        {
            for (int i = 0; i < thrusters.Length; i++)
            {
                if (thrusters[i].isInAxis[Axis.Pitch])
                {
                    thrusters[i].thrustLevel = 0f;
                }
            }
        }
        
        // yaw

        if (Mathf.Abs(input.Stick.x) > deadZone)
        {
            for (int i = 0; i < thrusters.Length; i++)
            {
                if (thrusters[i].isInAxis[Axis.Yaw])
                {
                    thrusters[i].thrustLevel = input.Stick.x * -1;
                }
            }
        }
        
        else
        {
            for (int i = 0; i < thrusters.Length; i++)
            {
                if (thrusters[i].isInAxis[Axis.Yaw])
                {
                    thrusters[i].thrustLevel = 0f;
                }
            }
        }
        

        //roll
        if (Mathf.Abs(input.Stick.z) > deadZone)
        {
            for (int i = 0; i < thrusters.Length; i++)
            {
                if (thrusters[i].isInAxis[Axis.Roll])
                {
                    thrusters[i].thrustLevel = input.Stick.z * -1;
                }
            }
        }
        
        else
        {
            for (int i = 0; i < thrusters.Length; i++)
            {
                if (thrusters[i].isInAxis[Axis.Roll])
                {
                    thrusters[i].thrustLevel = 0f;
                }
            }
        }
        

        // thrust

        for (int i = 0; i < thrusters.Length; i++)
        {
            if (thrusters[i].isInAxis[Axis.Forward])
            {
                thrusters[i].thrustLevel = input.Throttle;
            }
        }

        if (flightAssist)
        {
            Vector3 angularV = transform.InverseTransformDirection(rb.angularVelocity).normalized * rb.angularVelocity.magnitude;
            // stick x:yaw, y:pitch, z:roll
            Vector3 goalAngularVelocity = new Vector3(0, 0, 0);

            //flight assist pitch
            if (Mathf.Abs(input.Stick.y) < deadZone)
            {
                if (angularV.x < goalAngularVelocity.x)
                {
                    for (int i = 0; i < thrusters.Length; i++)
                    {
                        if (thrusters[i].isInAxis[Axis.Pitch])
                        {
                            thrusters[i].thrustLevel = flighAssistStrength;
                        }
                    }
                }

                if (angularV.x > goalAngularVelocity.x)
                {
                    for (int i = 0; i < thrusters.Length; i++)
                    {
                        if (thrusters[i].isInAxis[Axis.Pitch])
                        {
                            thrusters[i].thrustLevel = -flighAssistStrength;
                        }
                    }
                }
            }

            

            //flight assist yaw
            if (Mathf.Abs(input.Stick.x) < deadZone)
            {
                if (angularV.y < goalAngularVelocity.y)
                {
                    for (int i = 0; i < thrusters.Length; i++)
                    {
                        if (thrusters[i].isInAxis[Axis.Yaw])
                        {
                            thrusters[i].thrustLevel = -flighAssistStrength;
                        }
                    }
                }

                if (angularV.y > goalAngularVelocity.y)
                {
                    for (int i = 0; i < thrusters.Length; i++)
                    {
                        if (thrusters[i].isInAxis[Axis.Yaw])
                        {
                            thrusters[i].thrustLevel = flighAssistStrength;
                        }
                    }
                }
            }

            //flight assist roll
            if (Mathf.Abs(input.Stick.z) < deadZone)
            {
                if (angularV.z < goalAngularVelocity.z)
                {
                    for (int i = 0; i < thrusters.Length; i++)
                    {
                        if (thrusters[i].isInAxis[Axis.Roll])
                        {
                            thrusters[i].thrustLevel = flighAssistStrength;
                        }
                    }
                }

                if (angularV.z > goalAngularVelocity.z)
                {
                    for (int i = 0; i < thrusters.Length; i++)
                    {
                        if (thrusters[i].isInAxis[Axis.Roll])
                        {
                            thrusters[i].thrustLevel = -flighAssistStrength;
                        }
                    }
                }
            }
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

    /*
    bool isActive(Thruster t, List<ThrusterGroup> g)
    {
        foreach(ThrusterGroup group in g)
        {
            if (t.isInGroup[group]) return true;
        }
        return false;
    }
    */
}
