using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(InputManager.Stick.x * 200f + Screen.width/2f, InputManager.Stick.y * 200f + Screen.height/2f, 0f);
    }
}
