using UnityEngine;
using Assets.Scripts.Objects;
using System;

[RequireComponent(typeof(CannonController))]
public class CannonAimController : MonoBehaviour
{
    private CannonController _controller;


    private Vector2 _targetAim;
    private Vector2 _currentAim;
    public Vector2 RelativeRotation = Vector2.zero;

    private Vector3 LocalTarget;

    public float deadZone = 0f;
    private bool _inBounds = true;

    private const int sampleSize = 5;

    private float[] samplesX = new float[sampleSize], samplesY = new float[sampleSize];

    [SerializeField]
    private float kp = 1f;
    [SerializeField]
    private float ki = 1f;
    [SerializeField]
    private float kd = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        _controller = GetComponent<CannonController>();
    }

    private void FixedUpdate()
    {
        Trackfile target = transform.parent.Find("Tracker").GetComponent<Tracker>().GetFile(1);
        Vector3 relA = target.Acceleration;
        Vector3 relV = target.Velocity;
        Vector3 relPos = target.Position - transform.parent.rotation * transform.localPosition;

        // something wrong
        double a = 0.25d * Vector3.Dot(relA, relA);
        double b = Vector3.Dot(relA, relV);
        double c = Vector3.Dot(relA, relPos) + Vector3.Dot(relV, relV) - (_controller.Type.MuzzleVelocity * _controller.Type.MuzzleVelocity);
        double d = 2f * Vector3.Dot(relV, relPos);
        double e = Vector3.Dot(relPos, relPos);

        float t = (float)MathHelper.GetLowestPositive(MathHelper.SolveQuarticReal(a, b, c, d, e)); // the last thing is needed

        _controller.CanHitTarget = true; // if time is good you may shoot

        if (DebugManager.instance.GetSettingState("gun_log")) Debug.Log($"a: {a},b: {b},c: {c},d: {d},e: {e},t: {t}", this);

        Vector3 leadPoint = -(0.5f * t * t * relA + relV * t) + relPos;

        if (DebugManager.instance.GetSettingState("gun_log")) Debug.DrawRay(transform.position, leadPoint.normalized * 10000f);

        LocalTarget = transform.InverseTransformDirection(leadPoint);

        FindAim();
        if (DebugManager.instance.GetSettingState("gun_snap"))
        {
            SnapGun();
        }
        else
        {
            TurnGun();
        }

        _controller.Turret.localRotation = Quaternion.Euler(0, _currentAim.x, 0);
        _controller.BarrelMount.localRotation = Quaternion.Euler(_currentAim.y, 0, 0);

        if(DebugManager.instance.GetSettingState("gun_log")) Debug.DrawRay(_controller.Barrel.position, _controller.Barrel.forward * 10000f, Color.green);
        
    }

    private void Sample(Vector2 newSample)
    {
        // sampling

        // x values
        for (int i = samplesX.Length - 1; i >= 1; i--)
        {
            samplesX[i] = samplesX[i - 1];
        }
        samplesX[0] = newSample.x;

        string s = "";
        foreach (float x in samplesX) s += $"{x} : ";

        if(DebugManager.instance.GetSettingState("gun_log")) Debug.Log(s, gameObject);

        // y values
        for (int i = samplesY.Length - 1; i >= 1; i--)
        {
            samplesY[i] = samplesY[i - 1];
        }
        samplesY[0] = newSample.y;
    }

    private void TurnGun()
    {

        Sample(_currentAim);

        bool inBoundsX = true, inBoundsY = true;

        if (Mathf.Abs(RelativeRotation.x) > deadZone)
        {
            //PID stuff
            float speedX = MathHelper.PID(kp, ki, kd, samplesX, _targetAim.x, Time.fixedDeltaTime);
            speedX = Mathf.Clamp(speedX, -_controller.Type.traverseSpeed.x, _controller.Type.traverseSpeed.x);

            _currentAim.x += speedX * Time.fixedDeltaTime;

            // clamp rotation to max traverse
            float aimX = _controller.Type.CanRotateAround ? _currentAim.x : Mathf.Clamp(_currentAim.x, -_controller.Type.MaxTraverseLeft, _controller.Type.MaxTraverseRight);
            inBoundsX = aimX != _currentAim.x;
            _currentAim.x = aimX;
        }

        if (Mathf.Abs(RelativeRotation.y) > deadZone)
        {
            // PID stuff
            float speedY = MathHelper.PID(kp, ki, kd, samplesY, _targetAim.y, Time.fixedDeltaTime);
            speedY = Mathf.Clamp(speedY, -_controller.Type.traverseSpeed.y, _controller.Type.traverseSpeed.y);

            _currentAim.y += speedY * Time.fixedDeltaTime;

            // clamp rotation to max traverse
            float aimY = Mathf.Clamp(_currentAim.y, -_controller.Type.MaxTraverseUp, _controller.Type.MaxTraverseDown);
            inBoundsY = aimY != _currentAim.y;
            _currentAim.y = aimY;
        }

        _inBounds = inBoundsX && inBoundsY;
    }

    private void SnapGun()
    {
        _currentAim = _targetAim;

        _currentAim.y = Mathf.Clamp(_currentAim.y, -_controller.Type.MaxTraverseUp, _controller.Type.MaxTraverseDown);
        _currentAim.x = _controller.Type.CanRotateAround ? _currentAim.x : Mathf.Clamp(_currentAim.x, -_controller.Type.MaxTraverseLeft, _controller.Type.MaxTraverseRight);
        _inBounds = true;
    }

    private void FindAim()
    {
        Vector3 localTarget = LocalTarget - _controller.Turret.localRotation * (_controller.BarrelMount.localPosition + new Vector3(_controller.Barrel.localPosition.x, _controller.Barrel.localPosition.y, 0f));

        float x = localTarget.x;
        float y = localTarget.y;
        float z = localTarget.z;

        _targetAim.x = -Mathf.Atan2(z, x) * Mathf.Rad2Deg + 90;

        float c = Mathf.Sqrt(x * x + z * z);

        //c -= (Turret.forward * Barrel.localPosition.z).magnitude;
        _targetAim.y = -Mathf.Atan2(y, c) * Mathf.Rad2Deg;

        RelativeRotation = Vector2.zero;

        if (!_controller.CanHitTarget)
        {
            _targetAim = Vector2.zero;
        }

        RelativeRotation.x = _targetAim.x - _currentAim.x;
        RelativeRotation.y = _targetAim.y - _currentAim.y;


        if (RelativeRotation.x > 180f) RelativeRotation.x -= 360f;
        else if (RelativeRotation.x < -180f) RelativeRotation.x += 360f;
        if (RelativeRotation.y > 180f) RelativeRotation.y -= 360f;
        else if (RelativeRotation.y < -180f) RelativeRotation.y += 360f;
    }
}
