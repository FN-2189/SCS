using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustManager : MonoBehaviour
{
    public Thruster[] thrusters;

    private Vector3 thrustVector;
    private Vector3 thrustPosition;
    private float deadZone = 0.15f;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private InputManager input;

    [SerializeField] public bool flightAssist;

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
            Vector3 goalAngularVelocity = new Vector3(0, 0, 0);

            //flight assist pitch
            if (Mathf.Abs(input.Stick.y) < deadZone)
            {
                if (rb.angularVelocity.x < goalAngularVelocity.x)
                {
                    for (int i = 0; i < thrusters.Length; i++)
                    {
                        if (thrusters[i].isInAxis[Axis.Pitch])
                        {
                            thrusters[i].thrustLevel = 1;
                        }
                    }
                }

                if (rb.angularVelocity.x > goalAngularVelocity.x)
                {
                    for (int i = 0; i < thrusters.Length; i++)
                    {
                        if (thrusters[i].isInAxis[Axis.Pitch])
                        {
                            thrusters[i].thrustLevel = -1;
                        }
                    }
                }
            }

            //flight assist yaw
            if (Mathf.Abs(input.Stick.x) < deadZone)
            {
                if (rb.angularVelocity.y < goalAngularVelocity.y)
                {
                    for (int i = 0; i < thrusters.Length; i++)
                    {
                        if (thrusters[i].isInAxis[Axis.Yaw])
                        {
                            thrusters[i].thrustLevel = -1;
                        }
                    }
                }

                if (rb.angularVelocity.y > goalAngularVelocity.y)
                {
                    for (int i = 0; i < thrusters.Length; i++)
                    {
                        if (thrusters[i].isInAxis[Axis.Yaw])
                        {
                            thrusters[i].thrustLevel = 1;
                        }
                    }
                }
            }

            //flight assist roll
            if (Mathf.Abs(input.Stick.z) < deadZone)
            {
                if (rb.angularVelocity.z < goalAngularVelocity.z)
                {
                    for (int i = 0; i < thrusters.Length; i++)
                    {
                        if (thrusters[i].isInAxis[Axis.Roll])
                        {
                            thrusters[i].thrustLevel = 1;
                        }
                    }
                }

                if (rb.angularVelocity.z > goalAngularVelocity.z)
                {
                    for (int i = 0; i < thrusters.Length; i++)
                    {
                        if (thrusters[i].isInAxis[Axis.Roll])
                        {
                            thrusters[i].thrustLevel = -1;
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
            rb.AddForceAtPosition((thrusters[i].rotation * transform.forward) * thrusters[i].thrust, (transform.rotation * thrusters[i].position) + transform.position);
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
