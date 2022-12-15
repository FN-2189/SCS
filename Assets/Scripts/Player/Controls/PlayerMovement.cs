using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Vector3 gravity = new(0f, -9.81f, 0f);

    [SerializeField]
    private float counterForce = 100f;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private Transform head;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float jumpSpeed;

    [SerializeField]
    private float lookSpeed;

    private Rigidbody rb;

    private Vector3 angles;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        angles = Vector3.zero;
    }

    private void Update()
    {
        // looking
        angles = transform.rotation.eulerAngles;
        angles.x = head.rotation.eulerAngles.x;
        angles.y += InputManager.MouseDelta.x * lookSpeed * Time.deltaTime;
        angles.x += InputManager.MouseDelta.y * lookSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(new Vector3(0f, angles.y, 0f));
        head.localRotation = Quaternion.Euler(new Vector3(angles.x, 0f, 0f));
    }

    void FixedUpdate()
    {
        // simulate gravity
        rb.AddForce(gravity, ForceMode.Acceleration);
        Vector3 forceVector = Vector3.zero;

        Collider[] col;

        // if on floor
        // very bad practice oh no
        if((col = Physics.OverlapSphere(groundCheck.position, .3f)).Length > 0 && Array.Find(col, c => c.name != "B E A N"))
        {
            forceVector.x = InputManager.Walk.x * moveSpeed;
            forceVector.z = InputManager.Walk.y * moveSpeed;

            forceVector.y = InputManager.Jump ? jumpSpeed : 0f;

            // if nothing pressed and on ground stop Player
            if(forceVector.magnitude == 0f)
            {
                rb.AddForce(-rb.velocity * counterForce);
            }
        }

        rb.AddForce(transform.rotation * forceVector);
    }
}
