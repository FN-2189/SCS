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
    int faTriggerCooldown;

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

        //Flight assist toggle
        switch (faTriggerCooldown)
        {
            case 0:
                if (input.FAtoggle == 1)
                {
                    thrusters.flightAssistOn = !thrusters.flightAssistOn;
                    faTriggerCooldown = 1;
                }
                break;
            case 1:
                if (input.FAtoggle == 0)
                {
                    faTriggerCooldown = 0;
                }
                break;
        }

        //Autoaim toggle
        switch (input.AAtoggle)
        {
            case 0:
                GetComponent<Autopilot>().autopilotActive = false;
                break;
            case 1:
                GetComponent<Autopilot>().autopilotActive = true;
                GetComponent<Autopilot>().targetPosOrTargetVector = true;
                break;
        }

        mainEngineParticles.startLifetime = input.Throttle * 3;

    }

    private void CameraMovement()
    {

    }
}
