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
    private bool grounded;

    [SerializeField]
    private Transform head;

    [SerializeField]
    private float walkSpeed;
    private float moveSpeed;

    [SerializeField]
    private float jumpSpeed;

    [SerializeField]
    private float lookSpeed;
    private float xRotation;
    private float yRotation;

    [SerializeField]
    private float walkThreshhold;
    private float movementThreshhold;

    [SerializeField]
    private float sprintMultiplier;
    [SerializeField]
    private float sneakMultiplier;
    [SerializeField]
    private float airMultiplier;

    public MovementState state;
    public enum MovementState
    {
        sprinting,
        sneaking,
        walking
    }

    [SerializeField]
    private Rigidbody shipRb;
    private Vector3 shipLastVelocity = Vector3.zero;
    private Vector3 relativeSpaceLastVelocity = Vector3.zero;

    [SerializeField]
    private Transform shipTransform;

    [SerializeField]
    private CapsuleCollider playerCollider;

    [SerializeField]
    private Vector3 gravcorrection;
    private Vector3 gravdirection;

    private Rigidbody rb;

    private Vector3 angles;

    private bool jumpCooldown;

    [SerializeField]
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        angles = Vector3.zero;
        jumpCooldown = false;

    }

    private void Update()
    {
        // looking
        //angles = transform.rotation.eulerAngles;
        //angles.x = head.rotation.eulerAngles.x;
        //angles.y += InputManager.MouseDelta.x * lookSpeed * Time.deltaTime;
        //angles.x += -InputManager.MouseDelta.y * lookSpeed * Time.deltaTime;

        float mouseX = InputManager.MouseDelta.x * lookSpeed * Time.fixedDeltaTime;
        float mouseY = InputManager.MouseDelta.y * lookSpeed * Time.fixedDeltaTime;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        head.rotation = Quaternion.Euler(xRotation, yRotation, 0);

    }

    void FixedUpdate()
    {
        ApplyGravity();

        Vector3 forceVector = Vector3.zero;

        Collider[] col;

        //(col = Physics.OverlapSphere(groundCheck.position, .3f)).Length > 0 && Array.Find(col, c => c.name != "B E A N")
        grounded = (col = Physics.OverlapSphere(groundCheck.position, .3f)).Length > 0 && Array.Find(col, c => c.name != "B E A N");
        animator.SetBool("isGrounded", grounded);

        StateHandler();

        if(grounded)
        {

            forceVector.x = InputManager.Walk.x * moveSpeed;
            forceVector.z = InputManager.Walk.y * moveSpeed;

            if (InputManager.Jump && !jumpCooldown)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(jumpSpeed * -gravdirection, ForceMode.Impulse);
                jumpCooldown = true;
                Debug.Log("Jump");
                Invoke(nameof(ResetJump), 0.15f);
            }

            // if nothing pressed and on ground stop Player
            if(forceVector.magnitude == 0f)
            {
                rb.AddForce(-rb.velocity * counterForce);
            }
            else
            {
                animator.SetBool("isWalking", true);
            }

           
        } 
        else{ forceVector.x = InputManager.Walk.x * walkSpeed * airMultiplier;forceVector.z = InputManager.Walk.y * walkSpeed * airMultiplier;}

        if (rb.velocity.magnitude < movementThreshhold && grounded)
        {
            rb.AddForce(transform.rotation * forceVector);
        } 
        else if (!grounded)
        {
            rb.AddForce(transform.rotation * forceVector);
        }

    }

    private void ApplyGravity()
    {
        // compute gravity
        gravity = Quaternion.Euler(-90, 0, 0) * shipTransform.InverseTransformVector(MathHelper.Derive(shipLastVelocity - relativeSpaceLastVelocity, shipRb.velocity - RelativeSpace.CurrentVelocity, Time.deltaTime)) - shipRb.GetComponent<WorldspaceGravityObject>().gravity;

        shipLastVelocity = shipRb.velocity;
        relativeSpaceLastVelocity = RelativeSpace.CurrentVelocity;

        gravdirection = gravity.normalized;

        // simulate gravity
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    private void ResetJump()
    {
        jumpCooldown = false;
    }

    private void StateHandler()
    {


        if (InputManager.Sprint > 0f && !InputManager.Sneak)
        {
            state = MovementState.sprinting;
            moveSpeed = walkSpeed * sprintMultiplier;
            movementThreshhold = walkThreshhold * sprintMultiplier;
            playerCollider.height = 1.85f;
            groundCheck.localPosition = new Vector3(groundCheck.localPosition.x, -0.7f, groundCheck.localPosition.z);
            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", true);
        }
        else if (InputManager.Sneak)
        {
            if (state != MovementState.sneaking)
            {
                rb.AddForce(gravity * 0.5f, ForceMode.Impulse);
            }
            state = MovementState.sneaking;
            moveSpeed = walkSpeed * sneakMultiplier;
            movementThreshhold = walkThreshhold * sneakMultiplier;
            playerCollider.height = 0.8f;
            groundCheck.localPosition = new Vector3(groundCheck.localPosition.x, -0.2f, groundCheck.localPosition.z);
            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", false);
        }
        else
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            movementThreshhold = walkThreshhold;
            playerCollider.height = 1.85f;
            groundCheck.localPosition = new Vector3(groundCheck.localPosition.x, -0.7f, groundCheck.localPosition.z);
            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", false);
        }

    }
}
