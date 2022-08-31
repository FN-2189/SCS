using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Control : MonoBehaviour
{
    [SerializeField]
    private ThrustManager thrusters;

    private InputManager input;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] Rigidbody rb;
    [SerializeField] Camera thirdpersonview;
    [SerializeField] private GameObject railgunObject;

    private Railgun railgun;

    int RailgunFired = 0;

    public Vector3 ThirdPersonCameraPosition { get; private set; }

    private void Awake()
    {
        input = GameObject.Find("Manager").GetComponent<InputManager>();
        railgun = railgunObject.GetComponent<Railgun>();

        ThirdPersonCameraPosition = thirdpersonview.transform.position;

    }

    private void Update()
    {
        switch (RailgunFired) 
        {
            case 0:
                if (input.Trigger == 1)
                {
                    railgun.StartRailgunShot();
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

    private void CameraMovement()
    {

    }
}
