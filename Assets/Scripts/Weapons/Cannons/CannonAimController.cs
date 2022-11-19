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

    public float rate = 0.5f;
    public float deadZone = 0.01f;

    //public Transform target;
    //private Rigidbody _targetRb;

    //private Vector3 _localTarget;

    //private Rigidbody rb;

    //private float _timeNextShot = 0f;

    //private Animator _gunAnimator;
    //private ParticleSystem[] _muzzleParticles;

    //private int _shotsFired = 0;
    //private float _lastTime;
    //public float RPMTimestep = 1f;

    //private bool _canHitTarget = false;
    private bool _inBounds = true;

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
        bool inBoundsX = true, inBoundsY = true;

        if (Mathf.Abs(RelativeRotation.x) > deadZone)
        {
            _currentAim.x += _controller.Type.traverseSpeed.x * Mathf.Clamp(RelativeRotation.x / (180 * rate), -1f, 1f) * Time.fixedDeltaTime;
            
            
            float aimX = _controller.Type.CanRotateAround ? _currentAim.x : Mathf.Clamp(_currentAim.x, -_controller.Type.MaxTraverseLeft, _controller.Type.MaxTraverseRight);
            inBoundsX = aimX != _currentAim.x;
            _currentAim.x = aimX;
        }

        if (Mathf.Abs(RelativeRotation.y) > deadZone)
        {
            _currentAim.y += _controller.Type.traverseSpeed.y * Mathf.Clamp(RelativeRotation.y / (180 * rate), -1f, 1f) * Time.fixedDeltaTime;
            float aimY = Mathf.Clamp(_currentAim.y, -_controller.Type.MaxTraverseUp, _controller.Type.MaxTraverseDown);
            inBoundsY = aimY != _currentAim.y;
            _currentAim.y = aimY;
        }

        _inBounds = inBoundsX && inBoundsY;

        // TODO add PISS i mean PID already have P kinda


        //print(_relativeRotation);


        _controller.Turret.localRotation = Quaternion.Euler(0, _currentAim.x, 0);
        _controller.BarrelMount.localRotation = Quaternion.Euler(_currentAim.y, 0, 0);
    }

    private void LateUpdate()
    {
        //line.SetPositions(new Vector3[] { transform.position, _localTarget + transform.position });
        //laser.SetPositions(new Vector3[] { Barrel.position, Barrel.position + Barrel.forward * 50000 });
        //Debug.DrawLine(Barrel.position, Barrel.position + Barrel.forward * 50000);
        // Remove Debug code
    }
}
