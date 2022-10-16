using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    private Vector2 _gunAim;
    private Vector2 _currentAim;
    private Vector2 _relativeRotation = Vector2.zero;

    public float rotateSpeed;
    public float rate = 0.5f;
    public float deadZone = 0.01f;
    public float maxShootAngle = 10f;

    public Transform Turret;
    public Transform BarrelMount;
    public Transform Barrel;
    public float barrelLength;
    public Transform Muzzle;

    public Transform target;
    private Rigidbody _targetRb;

    private Vector3 _localTarget;

    public LineRenderer line;
    public LineRenderer laser;

    private Rigidbody rb => GetComponentInParent<Rigidbody>();

    public GameObject bullet;

    public float bulletV;

    public float FireRate = 1500f;
    private float _timeNextShot = 0f;

    private Animator _gunAnimator;
    private ParticleSystem[] _muzzleParticles;

    public Transform leadIndicator;

    private int _shotsFired = 0;
    private float _lastTime;
    public float RPMTimestep = 1f;

    [SerializeField]
    private bool _canHitTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        _targetRb = target.GetComponent<Rigidbody>();
        _gunAnimator = GetComponent<Animator>();
        _muzzleParticles = Muzzle.GetComponentsInChildren<ParticleSystem>();
        _timeNextShot = Time.time;
        _lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDir;
        Vector3? tryGetLead = LeadCalculator.CaculateLead(_targetRb.velocity - rb.velocity, transform.position, target.position, bulletV) - transform.position;
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

        _gunAim.x = -Mathf.Atan2(_localTarget.z, _localTarget.x) * Mathf.Rad2Deg + 90;

        float c = Mathf.Sqrt(_localTarget.x * _localTarget.x + _localTarget.z * _localTarget.z);

        c -= (Turret.forward * BarrelMount.localPosition.z).magnitude;
        _gunAim.y = -Mathf.Atan2(_localTarget.y, c) * Mathf.Rad2Deg;

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
            print(_shotsFired * 60 / RPMTimestep);
            _shotsFired = 0;
        }
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(_relativeRotation.x) > deadZone) _currentAim.x += rotateSpeed * Mathf.Clamp(_relativeRotation.x/(180*rate), -1f, 1f) * Time.fixedDeltaTime;

        if (Mathf.Abs(_relativeRotation.y) > deadZone) _currentAim.y += rotateSpeed * Mathf.Clamp(_relativeRotation.y/(180*rate), -1f, 1f) * Time.fixedDeltaTime;

        // TODO add PISS i mean PID already have P kinda


        //print(_relativeRotation);

        // allowed to shoot
        if(Time.time >= _timeNextShot && _canHitTarget && Mathf.Abs(_relativeRotation.x) <= maxShootAngle && Mathf.Abs(_relativeRotation.y) <= maxShootAngle)
        {
            Shoot();
            _shotsFired++;
            _timeNextShot = Time.time + 60f / FireRate;
        }
        else
        {
            _gunAnimator.SetBool("Shoot", false);
        }

        Turret.localRotation = Quaternion.Euler(0, _currentAim.x, 0);
        BarrelMount.localRotation = Quaternion.Euler(_currentAim.y, 0, 0);
    }

    private void LateUpdate()
    {
        //line.SetPositions(new Vector3[] { transform.position, _localTarget + transform.position });
        //laser.SetPositions(new Vector3[] { Barrel.position, Barrel.position + Barrel.forward * 50000 });
        Debug.DrawLine(Barrel.position, Barrel.position + Barrel.forward * 50000);
    }

    private void Shoot()
    {
        var g = Instantiate(bullet, Barrel.position + Barrel.forward * barrelLength, Barrel.rotation);
        g.GetComponent<Rigidbody>().velocity = rb.velocity + Barrel.forward * bulletV;
        BulletManager.AddToList(g.GetComponent<Bullet>());

        _gunAnimator.SetBool("Shoot", true);
        foreach(var p in _muzzleParticles)
        {
            p.Play();
        }
    }

    
}
