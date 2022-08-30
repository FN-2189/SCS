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
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Camera thirdpersonview;

    public Vector3 ThirdPersonCameraPosition { get; private set; }

    int RailgunFired = 0;

    private void Awake()
    {
        input = GameObject.Find("Manager").GetComponent<InputManager>();

        ThirdPersonCameraPosition = thirdpersonview.transform.position;

    }

    private void Update()
    {
        switch (RailgunFired) 
        {
            case 0:
                if (input.Trigger == 1)
                {
                    RailgunShot();
                    RailgunFired = 1;
                }
                break;
            case 1:
                if (input.Trigger == 0)
                {
                    RailgunFired = 0;
                }
                break;
        }



        mainEngineParticles.startLifetime = input.Throttle * 3;

    }

    public void RailgunShot()
    {
        railgunParticles.startSpeed = rigidbody.velocity.magnitude + 1000;
        railgunParticles.Play();
    }

    private void CameraMovement()
    {

    }
}
