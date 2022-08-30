using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Control : MonoBehaviour
{
    [SerializeField]
    private ThrustManager thrusters;

    private InputManager input;

    [SerializeField] ParticleSystem railgunParticles;
    [SerializeField] ParticleSystem mainEngineParticles;

    private void Awake()
    {
        input = GameObject.Find("Manager").GetComponent<InputManager>();
    }

    private void Update()
    {
        RailgunShot();
        mainEngineParticles.startLifetime = (input.Throttle + 1) * 2;
    }

    public void RailgunShot()
    {
        railgunParticles.Play();
    }
}
