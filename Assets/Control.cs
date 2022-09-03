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

    int railgunfiredCooldown = 0;
    int faTriggerCooldown;
    int decelerateAssistCooldown = 0;
    bool railgunCool;

    public Vector3 ThirdPersonCameraPosition { get; private set; }

    private void Awake()
    {
        input = GameObject.Find("Manager").GetComponent<InputManager>();
        railgun = railgunObject.GetComponent<Railgun>();

        ThirdPersonCameraPosition = thirdpersonview.transform.position;

    }

    private void Update()
    {
        switch (railgunfiredCooldown) 
        {
            case 0:
                if (input.Trigger == 1)
                {
                    railgun.StartRailgunShot();
                    railgunCool = false;
                    StartCoroutine(RailgunDelay());
                    railgunfiredCooldown = 1;
                }
                break;
            case 1:
                if (input.Trigger == 0 && railgunCool)
                {
                    railgunfiredCooldown = 0;
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

        //Autoaim toggle, doesn't disengage autopilot if in Decelerate assist mode
        switch (input.AAtoggle)
        {
            case 0:
                if (decelerateAssistCooldown != 1)
                {
                    GetComponent<Autopilot>().autopilotActive = false;
                }
                break;
            case 1:
                GetComponent<Autopilot>().autopilotActive = true;
                GetComponent<Autopilot>().targetPosOrTargetVector = true;
                break;
        }

        //Decelerate assist
        if (input.DeAtoggle == 1 && decelerateAssistCooldown == 0)
        {
            StartCoroutine(DecelerateAssist());
            decelerateAssistCooldown = 1;
        }

        mainEngineParticles.startLifetime = input.Throttle * 3;

    }

    private void CameraMovement()
    {

    }

    private IEnumerator DecelerateAssist()
    {
        thrusters.SetManual(Axis.Forward, true);
        thrusters.SetThrust(Axis.Forward, 0f);
        GetComponent<Autopilot>().decelerateAssistActive = true;
        GetComponent<Autopilot>().autopilotActive = true;

        yield return new WaitForSeconds(5);
        //This could use actual Vector verification but it's to late rn lul

        thrusters.SetManual(Axis.Forward, false);
        GetComponent<Autopilot>().autopilotActive = false;
        GetComponent<Autopilot>().decelerateAssistActive = false;
        Debug.Log("Flip complete");
        decelerateAssistCooldown = 0;

    }

    private IEnumerator RailgunDelay()
    {
        yield return new WaitForSeconds(7);
        railgunCool = true;
    }
}
