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

    [SerializeField]
    private float movementThreshhold;

    [SerializeField]
    private float sprintMultiplier;

    [SerializeField]
    private Rigidbody shipRb;
    private Vector3 shipRbVelocity;
    private Vector3 shipLastVelocity = Vector3.zero;
    private Vector3 relativeSpaceLastVelocity = Vector3.zero;

    [SerializeField]
    private Transform shipTransform;

    [SerializeField]
    private Vector3 gravcorrection;

    private Rigidbody rb;

    private Vector3 angles;

    private bool jumpCooldown;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        angles = Vector3.zero;
        jumpCooldown = false;

    }

    private void Update()
    {
        // looking
        angles = transform.rotation.eulerAngles;
        angles.x = head.rotation.eulerAngles.x;
        angles.y += InputManager.MouseDelta.x * lookSpeed * Time.deltaTime;
        angles.x += -InputManager.MouseDelta.y * lookSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(new Vector3(0f, angles.y, 0f));
        head.localRotation = Quaternion.Euler(new Vector3(angles.x, 0f, 0f));

    }

    void FixedUpdate()
    {
        // if (shipRb.velocity.magnitude != 0) { shipRbVelocity = shipRb.velocity; } else { shipRbVelocity = new Vector3(0.1f, 0.1f, 0.1f); }
        // shipRb.rotation.eulerAngles.x + 90, -shipRb.rotation.eulerAngles.y, -shipRb.rotation.eulerAngles.z
        gravity = Quaternion.Euler(-90, 0, 0) * shipTransform.InverseTransformVector(MathHelper.Derive(shipLastVelocity - relativeSpaceLastVelocity ,shipRb.velocity - RelativeSpace.CurrentVelocity, Time.deltaTime));
        
        Debug.Log("Gravity: " + gravity + " Last Velocity " + (shipLastVelocity + relativeSpaceLastVelocity) + "Current Velocity:" + (shipRb.velocity + RelativeSpace.CurrentVelocity));
        shipLastVelocity = shipRb.velocity;
        relativeSpaceLastVelocity = RelativeSpace.CurrentVelocity;



        // simulate gravity
        rb.AddForce(gravity, ForceMode.Acceleration);
        Vector3 forceVector = Vector3.zero;

        Collider[] col;

        // if on floor
        // very bad practice oh no
        if((col = Physics.OverlapSphere(groundCheck.position, .3f)).Length > 0 && Array.Find(col, c => c.name != "B E A N"))
        {

            forceVector.x = InputManager.Walk.x * moveSpeed;
            forceVector.z = InputManager.Walk.y * moveSpeed * (1 + sprintMultiplier * InputManager.Sprint);

            // Debug.Log(InputManager.Jump + " " + jumpCooldown);
            if (InputManager.Jump && !jumpCooldown)
            {
                forceVector.y = jumpSpeed;
                jumpCooldown = true;
                Debug.Log("Jump");
            }

            // Debug.Log("Force Vector:" + forceVector + " Input WS " + InputManager.Walk.y + InputManager.Walk.x);

            // if nothing pressed and on ground stop Player
            if(forceVector.magnitude == 0f)
            {
                rb.AddForce(-rb.velocity * counterForce);
            }
        }

        if (rb.velocity.magnitude < movementThreshhold * (1 + sprintMultiplier * InputManager.Sprint))
        {
            rb.AddForce(transform.rotation * forceVector);
        }

        if (!InputManager.Jump && jumpCooldown)
        {
            jumpCooldown = false;
            Debug.Log("Jump Cooldown reset");
        }
    }
}
