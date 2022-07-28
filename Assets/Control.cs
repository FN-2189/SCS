using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Control : MonoBehaviour
{
    [SerializeField]
    private ThrustManager thrusters;

    private InputManager input;

    private void Awake()
    {
        input = GameObject.Find("Manager").GetComponent<InputManager>();
    }

    private void Update()
    {
        Debug.Log(input.Stick);
    }
}
