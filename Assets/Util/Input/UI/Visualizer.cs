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
        transform.position = new Vector3(input.Stick.x * 400f + Screen.width/2f, input.Stick.y * 400f + Screen.height/2f, 0f);
    }
}
