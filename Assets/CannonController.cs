using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public Vector2 GunAim;
    public Vector2 currentAim;
    private Vector2 RelativeRotation = Vector2.zero;

    public float rotateSpeed;
    public float rate = 0.5f;
    public float deadZone = 0.01f;

    public Transform Turret;
    public Transform BarrelMount;
    public Transform Barrel;

    public Transform target;

    public Vector3 localTarget;

    public LineRenderer line;
    public LineRenderer laser;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 targetDir = target.position - transform.position;

        localTarget = transform.InverseTransformDirection(targetDir);



        line.SetPositions(new Vector3[] { transform.position, localTarget + transform.position });
        laser.SetPositions(new Vector3[] { Barrel.position, Barrel.position + Barrel.forward * 50000 });


        GunAim.x = -Mathf.Atan2(localTarget.z, localTarget.x) * Mathf.Rad2Deg + 90;

        float c = Mathf.Sqrt(localTarget.x * localTarget.x + localTarget.z * localTarget.z);

        c -= (Turret.forward * BarrelMount.localPosition.z).magnitude;
        GunAim.y = -Mathf.Atan2(localTarget.y, c) * Mathf.Rad2Deg;

        RelativeRotation = Vector2.zero;

        RelativeRotation.x = GunAim.x - currentAim.x;
        RelativeRotation.y = GunAim.y - currentAim.y;
        if (RelativeRotation.x > 180f) RelativeRotation.x -= 360f;
        else if (RelativeRotation.x < -180f) RelativeRotation.x += 360f;
        if (RelativeRotation.y > 180f) RelativeRotation.y -= 360f;
        else if (RelativeRotation.y < -180f) RelativeRotation.y += 360f;


    }

    private void FixedUpdate()
    {

        

        if (RelativeRotation.x > deadZone || RelativeRotation.x < -deadZone) currentAim.x += rotateSpeed * Mathf.Clamp(RelativeRotation.x/(180*rate), -1f, 1f) * Time.fixedDeltaTime;

        if (RelativeRotation.y < -deadZone || RelativeRotation.y > deadZone) currentAim.y += rotateSpeed * Mathf.Clamp(RelativeRotation.y/(180*rate), -1f, 1f) * Time.fixedDeltaTime;

        // TODO add PISS i mean PID already have P


        print(RelativeRotation);

        Turret.localRotation = Quaternion.Euler(0, currentAim.x, 0);
        BarrelMount.localRotation = Quaternion.Euler(currentAim.y, 0, 0);
    }
}
