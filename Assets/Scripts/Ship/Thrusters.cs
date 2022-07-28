using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrusters : MonoBehaviour
{
    public Thruster[] thrusters;

    private Vector3 thrustVector;
    private Vector3 thrustPosition;

    [SerializeField]
    private Rigidbody rb;

    void Awake()
    {
        thrusters = GetComponentsInChildren<Thruster>();
        Debug.Log(thrusters.Length);
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
}
