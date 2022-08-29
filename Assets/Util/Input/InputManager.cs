using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions input;

    [SerializeField]
    private float mouseSensitivity;

    public Vector2 Stick { get; private set; }
    public float Throttle;

    private void Awake()
    {
        Cursor.visible = false;
        input = new PlayerInputActions();
        input.Enable();

        Stick = Vector2.zero;
    }

    private void Update()
    {
        // mouse controls (pitch + yaw)
        Vector2 pitchAndYaw = input.ManualShipThrusterControl.PitchAndYaw.ReadValue<Vector2>();
        Vector2 stick = this.Stick;
        stick.x += pitchAndYaw.x * mouseSensitivity * Time.deltaTime;
        stick.y += pitchAndYaw.y * mouseSensitivity * Time.deltaTime;
        stick.x = Mathf.Clamp(stick.x, -1, 1);
        stick.y = Mathf.Clamp(stick.y, -1, 1);


        float joyStickX = input.ManualShipThrusterControl.StickX.ReadValue<float>();
        float joyStickY = input.ManualShipThrusterControl.StickY.ReadValue<float>();
        float joyStickRz = input.ManualShipThrusterControl.StickRz.ReadValue<float>();
        float joyStickZ = input.ManualShipThrusterControl.StickZ.ReadValue<float>();
        stick.x = joyStickRz;
        stick.y = joyStickY * -1;
        float throttle = joyStickZ * -1;

        this.Stick = stick;
        this.Throttle = throttle;
    }
}
