using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stickRotator : MonoBehaviour
{
    [SerializeField]
    private Vector3 maximumStickRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 stickRotation;

        stickRotation.y = -InputManager.Stick.y * maximumStickRotation.y;
        stickRotation.x = -InputManager.Stick.z * maximumStickRotation.x;
        stickRotation.z = InputManager.Stick.x * maximumStickRotation.z;

        transform.localRotation = Quaternion.Euler(stickRotation);
    }
}
