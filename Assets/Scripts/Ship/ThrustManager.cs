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

        // thrust

        for (int i = 0; i < thrusters.Length; i++)
        {
            if (thrusters[i].isInAxis[Axis.Forward])
            {
                thrusters[i].thrustLevel = input.Throttle;
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
