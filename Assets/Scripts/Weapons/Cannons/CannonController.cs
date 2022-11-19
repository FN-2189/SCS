using Assets.Scripts.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem.XR;

public class CannonController : MonoBehaviour
{
    public Cannon Type;

    public Transform Turret { get; private set; }
    public Transform BarrelMount { get; private set; }
    public Transform Barrel { get; private set; }
    public Transform Muzzle { get; private set; }


    public Rigidbody rb;


    public Transform target;
    private Rigidbody _targetRb;

    public Vector3 LocalTarget { get; private set; }
    public Vector2 RelativeRotation { get; private set; }
    public bool CanHitTarget { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Turret = Array.Find(GetComponentsInChildren<Transform>(), t => t.name == "Turret");
        BarrelMount = Array.Find(Turret.GetComponentsInChildren<Transform>(), t => t.name == "BarrelMount");
        Barrel = Array.Find(BarrelMount.GetComponentsInChildren<Transform>(), t => t.name == "Barrel");
        Muzzle = Array.Find(Barrel.GetComponentsInChildren<Transform>(), t => t.name == "Muzzle");

        rb = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Lead Calculation
        _targetRb = target.GetComponent<Rigidbody>();
        Vector3 targetDir;
        Vector3 ? tryGetLead = LeadCalculator.CaculateLead(_targetRb.velocity - rb.velocity, transform.position, target.position, Type.MuzzleVelocity) - transform.position;
        if (tryGetLead == null)
        {
            print("Can't reach target!");
            CanHitTarget = false;
            targetDir = Vector3.zero;
        }
        else
        {
            targetDir = (Vector3)tryGetLead;
            CanHitTarget = true;
        }

        LocalTarget = transform.InverseTransformDirection(targetDir);
    }
}
