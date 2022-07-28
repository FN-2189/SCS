using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    [SerializeField]
    private InputManager input;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(input.Stick.x * 400 + Screen.width/2, input.Stick.y * 400 + Screen.height/2, 0f);
    }
}
