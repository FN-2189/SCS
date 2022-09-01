using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions input;

    [SerializeField]
    private float mouseSensitivity;

    public Vector3 Stick { get; private set; }
    public float Throttle;
    public float Trigger;

    private void Awake()
    {
        Cursor.visible = false;
        input = new PlayerInputActions();
        input.Enable();

        Stick = Vector3.zero;
    }

    private void Update()
    {
        // mouse controls (pitch + yaw)
        Vector2 pitchAndRoll = input.ManualShipThrusterControl.PitchAndRoll.ReadValue<Vector2>();
        Vector2 pitchAndRollJoystick = input.ManualShipThrusterControl.pitchAndRollJoystick.ReadValue<Vector2>();
        Vector3 stick = this.Stick;
        stick.x += pitchAndRoll.x * mouseSensitivity * Time.deltaTime;
        stick.y += pitchAndRoll.y * mouseSensitivity * Time.deltaTime;
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


        //this.Throttle = input.ManualShipThrusterControl.Throttle.ReadValue<float>();
        this.Throttle = throttle;
        this.Stick = stick;
        this.Trigger = trigger;
        
    }
}
