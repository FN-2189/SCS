using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CannonController), typeof(CannonAimController))]
public class CannonShootController : MonoBehaviour
{
    private CannonController _controller;
    private CannonAimController _aimController;

    private Animator _gunAnimator;
    private ParticleSystem[] _muzzleParticles;

    public float maxShootAngle = 0.5f;

    private float _timeNextShot;

    public float RPMTimestep = 1f;

    private void Awake()
    {
        _controller = GetComponent<CannonController>();
        _aimController = GetComponent<CannonAimController>();
        _gunAnimator = GetComponent<Animator>();
        _timeNextShot = Time.time;
        if (_controller.Muzzle == null) return;
        _muzzleParticles = _controller.Muzzle.GetComponentsInChildren<ParticleSystem>();

    }

    private void FixedUpdate()
    {
        // allowed to shoot
        if (Time.time >= _timeNextShot /*&& _inBounds*/ && _controller.CanHitTarget && Mathf.Abs(_aimController.RelativeRotation.x) <= maxShootAngle && Mathf.Abs(_aimController.RelativeRotation.y) <= maxShootAngle)
        {
            if (InputManager.Fire)
            {
                Shoot();
                _timeNextShot = Time.time + 60f / _controller.Type.FireRate;
            }
        }
    }

    private void Shoot()
    {
        // wait if controller not initalized
        if (_controller.Muzzle != null && _muzzleParticles == null)
        {
            _muzzleParticles = _controller.Muzzle.GetComponentsInChildren<ParticleSystem>();
            return;
        }
        
        
        var g = Instantiate(_controller.Type.BulletType, _controller.Barrel.position + _controller.Barrel.forward * _controller.Type.BulletSpawnOffset, _controller.Barrel.rotation);
        //g.GetComponent<Rigidbody>().velocity = rb.velocity + Barrel.forward * type.MuzzleVelocity;
        g.GetComponent<Bullet>().SendIt(_controller.rb.velocity + _controller.Barrel.forward * _controller.Type.MuzzleVelocity);
        BulletManager.AddToList(g.GetComponent<Bullet>());

        _gunAnimator.StopPlayback();
        _gunAnimator.Play("ShootAnim");
        foreach (var p in _muzzleParticles)
        {
            p.Play();
        }
    }
}
