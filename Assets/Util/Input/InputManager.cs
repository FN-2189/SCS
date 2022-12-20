using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static PlayerInputActions input;

    [SerializeField] private float mouseSensitivity;
    [SerializeField] private GameObject thrusterObject;
    [SerializeField] private GameObject shipObject;

    public static Vector3 Stick { get; private set; }
    public static Vector2 Translate { get; private set; }
    public static float Throttle { get; private set; }
    public static float Trigger { get; private set; }
    public static float FAtoggle { get; private set; }
    public static float AAtoggle { get; private set; }
    public static float DeAtoggle { get; private set; }
    public static float CycleZoom { get; private set; }
    public static bool Rangefind { get; private set; }
    public static bool Fire { get; private set; }
    public static bool Escape { get; private set; }
    public static bool Debug { get; private set; }
    public static Vector2 MouseDelta { get; private set; }
    public static Vector2 Walk { get; private set; }
    public static float Sprint { get; private set; }
    public static bool Jump { get; private set; }
    public static bool Sneak { get; private set; }

    public static bool SwitchPilot { get; private set; }
    public static bool SwitchGunner { get; private set; }
    public static bool SwitchCommander { get; private set; }
    public static bool SwitchEngineer { get; private set; }

    private float faTriggerCooldown;

    

    private void Awake()
    {
        //Cursor.visible = false;
        input = new PlayerInputActions();
        input.Enable();
        input.WalkControls.Enable();
    }

    private void Update()
    {
        /*
        // mouse controls (pitch + yaw)
        Vector2 pitchAndRoll = input.PilotControl.PitchAndRoll.ReadValue<Vector2>();
        Vector2 pitchAndRollJoystick = input.PilotControl.pitchAndRollJoystick.ReadValue<Vector2>();
        Vector3 stick = this.Stick;
        stick.z = input.PilotControl.Yaw.ReadValue<float>();
        stick.x = Mathf.Clamp(stick.x, -1, 1);
        stick.y = Mathf.Clamp(stick.y, -1, 1);

        float throttleInput = input.PilotControl.Throttle.ReadValue<float>();
        
        float joyStickRz = input.PilotControl.StickRz.ReadValue<float>();
        stick.x = joyStickRz; 
        stick.y = pitchAndRollJoystick.y * -1;
        stick.z = pitchAndRollJoystick.x;
        
        float throttle = (-throttleInput + 1) / 2;
        

        float trigger = 0f;
        trigger = input.PilotControl.Fire.ReadValue<float>();
        */

        //inputs
        Vector2 pitchAndRollIn = input.PilotControl.pitchAndRollJoystick.ReadValue<Vector2>();
        Vector2 translateIn = input.PilotControl.Translate.ReadValue<Vector2>();
        float yawIn = input.PilotControl.Yaw.ReadValue<float>();
        float throttleIn = input.PilotControl.Throttle.ReadValue<float>();
        float triggerIn = input.PilotControl.Fire.ReadValue<float>();
        //TODO: Make bools
        float flightAssistToggleIn = input.PilotControl.FlightAssistToggle.ReadValue<float>();
        float aimAssistToggleIn = input.PilotControl.AimAissistToggle.ReadValue<float>();
        float decelAssistToggleIn = input.PilotControl.DecelerateAssistToggle.ReadValue<float>();

        float zoom = input.GunnerControl.ToggleZoom.ReadValue<float>();
        bool laser = input.GunnerControl.Laser.ReadValue<float>() > 0f;
        bool fire = input.GunnerControl.Fire.ReadValue<float>() > 0f;

        //general inputs
        bool escape = input.GeneralControls.Escape.triggered;
        bool debug = input.GeneralControls.Debug.triggered;

        Vector2 delta = input.GeneralControls.Delta.ReadValue<Vector2>();

        Vector2 walk = input.WalkControls.Move.ReadValue<Vector2>();
        bool jump = input.WalkControls.Jump.ReadValue<float>() > 0f;
        float sprint = input.WalkControls.Sprint.ReadValue<float>();
        bool sneak = input.WalkControls.Sneak.ReadValue<float>() > 0f;

        SwitchPilot = input.GeneralControls.SwitchtoPilot.ReadValue<float>() > 0f;
        SwitchGunner = input.GeneralControls.SwitchtoGunner.ReadValue<float>() > 0f;
        SwitchCommander = input.GeneralControls.SwitchtoCommander.ReadValue<float>() > 0f;
        SwitchEngineer = input.GeneralControls.SwitchtoEngineer.ReadValue<float>() > 0f;

        Vector3 stick = Vector3.zero;
        stick.x = yawIn;
        stick.y = -pitchAndRollIn.y;
        stick.z = pitchAndRollIn.x;

        //this.Throttle = input.PilotControl.Throttle.ReadValue<float>();
        Throttle = throttleIn;
        Stick = stick;
        Translate = translateIn;
        Trigger = triggerIn;
        FAtoggle = flightAssistToggleIn;
        AAtoggle = aimAssistToggleIn;
        DeAtoggle = decelAssistToggleIn;

        CycleZoom = zoom;
        Rangefind = laser;
        Fire = fire;

        Escape = escape;
        Debug = debug;

        MouseDelta = delta * mouseSensitivity;

        Walk = walk;
        Jump = jump;
        Sprint = sprint;
        Sneak = sneak;

    }
}
