using UnityEngine;
using Assets.Scripts.Objects;
using System;

[RequireComponent(typeof(CannonController))]
public class CannonAimController : MonoBehaviour
{
    private CannonController _controller;


    private Vector2 _gunAim;
    private Vector2 _currentAim;
    public Vector2 RelativeRotation = Vector2.zero;

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

    // Update is called once per frame
    void Update()
    {
        // TODO fix target not being where it should be (Offset of Barrel)
        Vector3 localTarget = _controller.LocalTarget - _controller.Turret.localRotation * (_controller.BarrelMount.localPosition + new Vector3(_controller.Barrel.localPosition.x, _controller.Barrel.localPosition.y, 0f));

        float x = localTarget.x;
        float y = localTarget.y;
        float z = localTarget.z;

        _gunAim.x = -Mathf.Atan2(z, x) * Mathf.Rad2Deg + 90;

        float c = Mathf.Sqrt(x * x + z * z);

        //c -= (Turret.forward * Barrel.localPosition.z).magnitude;
        _gunAim.y = -Mathf.Atan2(y, c) * Mathf.Rad2Deg;

        RelativeRotation = Vector2.zero;

        if (!_controller.CanHitTarget)
        {
            _gunAim = Vector2.zero;
        }

        RelativeRotation.x = _gunAim.x - _currentAim.x;
        RelativeRotation.y = _gunAim.y - _currentAim.y;


        if (RelativeRotation.x > 180f) RelativeRotation.x -= 360f;
        else if (RelativeRotation.x < -180f) RelativeRotation.x += 360f;
        if (RelativeRotation.y > 180f) RelativeRotation.y -= 360f;
        else if (RelativeRotation.y < -180f) RelativeRotation.y += 360f;
    }

    private void FixedUpdate()
    {
        Sample(_currentAim);

        bool inBoundsX = true, inBoundsY = true;

        if (Mathf.Abs(RelativeRotation.x) > deadZone)
        {
            //PID stuff
            float speedX = MathHelper.PID(kp, ki, kd, samplesX, _gunAim.x, Time.fixedDeltaTime);
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
            float speedY = MathHelper.PID(kp, ki, kd, samplesY, _gunAim.y, Time.fixedDeltaTime);
            speedY = Mathf.Clamp(speedY, -_controller.Type.traverseSpeed.y, _controller.Type.traverseSpeed.y);

            _currentAim.y += speedY * Time.fixedDeltaTime;

            // clamp rotation to max traverse
            float aimY = Mathf.Clamp(_currentAim.y, -_controller.Type.MaxTraverseUp, _controller.Type.MaxTraverseDown);
            inBoundsY = aimY != _currentAim.y;
            _currentAim.y = aimY;
        }

        _inBounds = inBoundsX && inBoundsY;

        // TODO add PISS i mean PID already have P kinda

        _controller.Turret.localRotation = Quaternion.Euler(0, _currentAim.x, 0);
        _controller.BarrelMount.localRotation = Quaternion.Euler(_currentAim.y, 0, 0);
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

        // y values
        for (int i = samplesY.Length - 1; i >= 1; i--)
        {
            samplesY[i] = samplesY[i - 1];
        }
        samplesY[0] = newSample.y;
    }
}
