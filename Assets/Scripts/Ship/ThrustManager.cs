using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustManager : MonoBehaviour
{
    public Thruster[] thrusters;
    [SerializeField]
    private List<ThrusterGroup> activeGroups = new List<ThrusterGroup>();

    private Vector3 thrustVector;
    private Vector3 thrustPosition;

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
        float yInput = input.Stick.y;
        activeGroups.Clear();
        if (yInput > 0.15) activeGroups.Add(ThrusterGroup.PitchUp);
        else if (yInput < -0.15) activeGroups.Add(ThrusterGroup.PitchDown);

        float xInput = input.Stick.x;
        if (xInput > 0.15) activeGroups.Add(ThrusterGroup.YawRight);
        else if (xInput < -0.15) activeGroups.Add(ThrusterGroup.YawLeft);

        float zInput = input.Throttle;
        if (zInput > 0f) activeGroups.Add(ThrusterGroup.Forward);

        float inputMagnitude = input.Stick.magnitude;

        for (int i = 0; i < thrusters.Length; i++)
        {
            if(isActive(thrusters[i], activeGroups))
            {
                thrusters[i].thrustLevel = inputMagnitude;
            }
            else
            {
                thrusters[i].thrustLevel = 0f;
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

    bool isActive(Thruster t, List<ThrusterGroup> g)
    {
        foreach(ThrusterGroup group in g)
        {
            if (t.isInGroup[group]) return true;
        }
        return false;
    }
}
