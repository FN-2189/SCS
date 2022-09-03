using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions input;

    [SerializeField] private float mouseSensitivity;
    [SerializeField] private GameObject thrusterObject;
    [SerializeField] private GameObject shipObject;

    public Vector3 Stick { get; private set; }
    public Vector2 Translate { get; private set; }
    public float Throttle { get; private set; }
    public float Trigger { get; private set; }
    public float FAtoggle { get; private set; }
    public float AAtoggle { get; private set; }

    private float faTriggerCooldown;

    private void Awake()
    {
        Cursor.visible = false;
        input = new PlayerInputActions();
        input.Enable();
    }

    private void Update()
    {
        /*
        // mouse controls (pitch + yaw)
        Vector2 pitchAndRoll = input.ManualShipThrusterControl.PitchAndRoll.ReadValue<Vector2>();
        Vector2 pitchAndRollJoystick = input.ManualShipThrusterControl.pitchAndRollJoystick.ReadValue<Vector2>();
        Vector3 stick = this.Stick;
        stick.z = input.ManualShipThrusterControl.Yaw.ReadValue<float>();
        stick.x = Mathf.Clamp(stick.x, -1, 1);
        stick.y = Mathf.Clamp(stick.y, -1, 1);

        float throttleInput = input.ManualShipThrusterControl.Throttle.ReadValue<float>();
        
        float joyStickRz = input.ManualShipThrusterControl.StickRz.ReadValue<float>();
        stick.x = joyStickRz; 
        stick.y = pitchAndRollJoystick.y * -1;
        stick.z = pitchAndRollJoystick.x;
        
        float throttle = (-throttleInput + 1) / 2;
        

        float trigger = 0f;
        trigger = input.ManualShipThrusterControl.Fire.ReadValue<float>();
        */

        //inputs
        Vector2 pitchAndRollIn = input.ManualShipThrusterControl.pitchAndRollJoystick.ReadValue<Vector2>();
        Vector2 translateIn = input.ManualShipThrusterControl.Translate.ReadValue<Vector2>();
        float yawIn = input.ManualShipThrusterControl.Yaw.ReadValue<float>();
        float throttleIn = input.ManualShipThrusterControl.Throttle.ReadValue<float>();
        float triggerIn = input.ManualShipThrusterControl.Fire.ReadValue<float>();
        float flightAssistToggleIn = input.ManualShipThrusterControl.FlightAssistToggle.ReadValue<float>();
        float aimAssistToggleIn = input.ManualShipThrusterControl.AimAissistToggle.ReadValue<float>();

        Vector3 stick = Vector3.zero;
        stick.x = yawIn;
        stick.y = -pitchAndRollIn.y;
        stick.z = pitchAndRollIn.x;

        //this.Throttle = input.ManualShipThrusterControl.Throttle.ReadValue<float>();
        this.Throttle = throttleIn;
        this.Stick = stick;
        this.Translate = translateIn;
        this.Trigger = triggerIn;
        this.FAtoggle = flightAssistToggleIn;
        this.AAtoggle = aimAssistToggleIn;

    }
}
