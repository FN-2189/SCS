using Assets.Scripts.Objects;
using System;
using UnityEngine;

public class CannonController : PowerModule
{
    public Cannon Type;

    [SerializeField]
    private Trackfile target;
    public Transform Turret { get; private set; }
    public Transform BarrelMount { get; private set; }
    public Transform Barrel { get; private set; }
    public Transform Muzzle { get; private set; }

    [SerializeField]
    private GameObject explosionPrefab;


    public Rigidbody rb;

    public Vector3 LocalTarget { get; private set; }
    public Vector2 RelativeRotation { get; private set; }
    public bool CanHitTarget { get;  set; }

    // Start is called before the first frame update
    void Start()
    {
        Turret = Array.Find(GetComponentsInChildren<Transform>(), t => t.name == "Turret");
        BarrelMount = Array.Find(Turret.GetComponentsInChildren<Transform>(), t => t.name == "BarrelMount");
        Barrel = Array.Find(BarrelMount.GetComponentsInChildren<Transform>(), t => t.name == "Barrel");
        Muzzle = Array.Find(Barrel.GetComponentsInChildren<Transform>(), t => t.name == "Muzzle");

        rb = GetComponentInParent<Rigidbody>();
    }

    new void Update()
    {
        base.Update();
    }

    public override void ModuleDestroyed()
    {
        base.ModuleDestroyed();
        Instantiate(explosionPrefab, transform);
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
        /*
        Trackfile target = transform.parent.Find("Tracker").GetComponent<Tracker>().GetFile(1);
        Vector3 relA = target.Acceleration;
        Vector3 relV = target.Velocity;
        Vector3 relPos = target.Position - transform.parent.rotation * transform.localPosition;

        // something wrong
        double a = 0.25d * Vector3.Dot(relA, relA);
        double b = Vector3.Dot(relA, relV);
        double c = Vector3.Dot(relA, relPos) + Vector3.Dot(relV, relV) - (Type.MuzzleVelocity * Type.MuzzleVelocity);
        double d = 2f * Vector3.Dot(relV, relPos);
        double e = Vector3.Dot(relPos, relPos);

        float t = (float)MathHelper.GetLowestPositive(MathHelper.SolveQuarticReal(a, b, c, d, e));

        CanHitTarget = true; // if time is good you may shoot

        if(DebugManager.instance.GetSettingState("gun_log")) Debug.Log($"a: {a},b: {b},c: {c},d: {d},e: {e},t: {t}", this);
        
        Vector3 leadPoint = -0.5f * t * t * relA - relV * t + relPos;

        if (DebugManager.instance.GetSettingState("gun_log")) Debug.DrawRay(transform.position, leadPoint.normalized * 10000f);

        LocalTarget = transform.InverseTransformDirection(leadPoint);
        */
        //LocalTarget = (Vector3)LeadCalculator.CaculateLead(target.Velocity, transform.position, transform.parent.position + target.Position, Type.MuzzleVelocity);
        //Debug.DrawRay(transform.position, LocalTarget);
    //}
}
