using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions input;

    [SerializeField]
    private float mouseSensitivity;

    public Vector2 Stick { get; private set; }

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
        Vector2 pitchAndYaw = input.Main.PitchAndYaw.ReadValue<Vector2>();
        Vector2 stick = this.Stick;
        stick.x += pitchAndYaw.x * mouseSensitivity * Time.deltaTime;
        stick.y += pitchAndYaw.y * mouseSensitivity * Time.deltaTime;
        stick.x = Mathf.Clamp(stick.x, -1, 1);
        stick.y = Mathf.Clamp(stick.y, -1, 1);
        this.Stick = stick;
    }
}
