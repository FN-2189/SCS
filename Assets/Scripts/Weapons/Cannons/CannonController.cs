using UnityEngine;
using Assets.Scripts.Objects;
using System;

public class CannonController : MonoBehaviour
{
    private Vector2 _gunAim;
    private Vector2 _currentAim;
    private Vector2 _relativeRotation = Vector2.zero;

    public Cannon type;

    public float rate = 0.5f;
    public float deadZone = 0.01f;
    public float maxShootAngle = 10f;


    private Transform _turret;
    private Transform _barrelMount;
    private Transform _barrel;
    private Transform _muzzle;

    public Transform target;
    private Rigidbody _targetRb;

    private Vector3 _localTarget;

    private Rigidbody rb;

    private float _timeNextShot = 0f;

    private Animator _gunAnimator;
    private ParticleSystem[] _muzzleParticles;

    private int _shotsFired = 0;
    private float _lastTime;
    public float RPMTimestep = 1f;

    private bool _canHitTarget = false;
    private bool _inBounds = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        _targetRb = target.GetComponent<Rigidbody>();
        _gunAnimator = GetComponent<Animator>();

        _turret =       Array.Find(GetComponentsInChildren<Transform>(),                t => t.name == "Turret");
        _barrelMount =  Array.Find(_turret.GetComponentsInChildren<Transform>(),        t => t.name == "BarrelMount");
        _barrel =       Array.Find(_barrelMount.GetComponentsInChildren<Transform>(),   t => t.name == "Barrel");
        _muzzle =       Array.Find(_barrel.GetComponentsInChildren<Transform>(),        t => t.name == "Muzzle");

        _muzzleParticles = _muzzle.GetComponentsInChildren<ParticleSystem>();
        _timeNextShot = Time.time;
        _lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDir;
        Vector3? tryGetLead = LeadCalculator.CaculateLead(_targetRb.velocity - rb.velocity, transform.position, target.position, type.MuzzleVelocity) - transform.position;
        if(tryGetLead == null)
        {
            print("Can't reach target!");
            _canHitTarget = false;
            targetDir = Vector3.zero;
        }
        else
        {
            targetDir = (Vector3)tryGetLead;
            _canHitTarget = true;
        }

        //leadIndicator.position = targetDir + transform.position;

        _localTarget = transform.InverseTransformDirection(targetDir);

        // TODO fix target not being where it should be (Offset of Barrel)
        _localTarget -= _turret.localRotation * (_barrelMount.localPosition + new Vector3(_barrel.localPosition.x, _barrel.localPosition.y, 0f));

        float x = _localTarget.x;
        float y = _localTarget.y;
        float z = _localTarget.z;

        _gunAim.x = -Mathf.Atan2(z, x) * Mathf.Rad2Deg + 90;

        float c = Mathf.Sqrt(x * x + z * z);

        //c -= (Turret.forward * Barrel.localPosition.z).magnitude;
        _gunAim.y = -Mathf.Atan2(y, c) * Mathf.Rad2Deg;

        _relativeRotation = Vector2.zero;

        if (!_canHitTarget)
        {
            _gunAim = Vector2.zero;
        }

        _relativeRotation.x = _gunAim.x - _currentAim.x;
        _relativeRotation.y = _gunAim.y - _currentAim.y;


        if (_relativeRotation.x > 180f) _relativeRotation.x -= 360f;
        else if (_relativeRotation.x < -180f) _relativeRotation.x += 360f;
        if (_relativeRotation.y > 180f) _relativeRotation.y -= 360f;
        else if (_relativeRotation.y < -180f) _relativeRotation.y += 360f;

        if(_lastTime + RPMTimestep < Time.time)
        {
            _lastTime = Time.time;
            //print(_shotsFired * 60 / RPMTimestep);
            _shotsFired = 0;
        }
    }

    private void FixedUpdate()
    {
        bool inBoundsX = true, inBoundsY = true;

        if (Mathf.Abs(_relativeRotation.x) > deadZone)
        {
            _currentAim.x += type.traverseSpeed.x * Mathf.Clamp(_relativeRotation.x / (180 * rate), -1f, 1f) * Time.fixedDeltaTime;
            
            
            float aimX = type.CanRotateAround ? _currentAim.x : Mathf.Clamp(_currentAim.x, -type.MaxTraverseLeft, type.MaxTraverseRight);
            inBoundsX = aimX != _currentAim.x;
            _currentAim.x = aimX;
        }

        if (Mathf.Abs(_relativeRotation.y) > deadZone)
        {
            _currentAim.y += type.traverseSpeed.y * Mathf.Clamp(_relativeRotation.y / (180 * rate), -1f, 1f) * Time.fixedDeltaTime;
            float aimY = Mathf.Clamp(_currentAim.y, -type.MaxTraverseUp, type.MaxTraverseDown);
            inBoundsY = aimY != _currentAim.y;
            _currentAim.y = aimY;
        }

        _inBounds = inBoundsX && inBoundsY;

        // TODO add PISS i mean PID already have P kinda


        //print(_relativeRotation);

        // allowed to shoot
        if (Time.time >= _timeNextShot /*&& _inBounds*/ && _canHitTarget && Mathf.Abs(_relativeRotation.x) <= maxShootAngle && Mathf.Abs(_relativeRotation.y) <= maxShootAngle)
        {
            if (InputManager.Fire)
            {
                Shoot();
                _shotsFired++;
                _timeNextShot = Time.time + 60f / type.FireRate;
            }
        }

        _turret.localRotation = Quaternion.Euler(0, _currentAim.x, 0);
        _barrelMount.localRotation = Quaternion.Euler(_currentAim.y, 0, 0);
    }

    private void LateUpdate()
    {
        //line.SetPositions(new Vector3[] { transform.position, _localTarget + transform.position });
        //laser.SetPositions(new Vector3[] { Barrel.position, Barrel.position + Barrel.forward * 50000 });
        //Debug.DrawLine(Barrel.position, Barrel.position + Barrel.forward * 50000);
        // Remove Debug code
    }

    private void Shoot()
    {
        var g = Instantiate(type.BulletType, _barrel.position + _barrel.forward * type.BulletSpawnOffset, _barrel.rotation);
        //g.GetComponent<Rigidbody>().velocity = rb.velocity + Barrel.forward * type.MuzzleVelocity;
        g.GetComponent<Bullet>().SendIt(rb.velocity + _barrel.forward * type.MuzzleVelocity);
        BulletManager.AddToList(g.GetComponent<Bullet>());

        _gunAnimator.StopPlayback();
        _gunAnimator.Play("ShootAnim");
        foreach (var p in _muzzleParticles)
        {
            p.Play();
        }
    }

    
}
