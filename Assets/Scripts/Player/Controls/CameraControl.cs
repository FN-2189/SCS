using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private float sensitivity;

    [SerializeField]
    private float maxUp;

    [SerializeField]
    private float maxDown;

    [SerializeField]
    private float maxSide;

    private Vector3 _currentRotation = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        //_currentRotation.x += -InputManager.MouseDelta.y * sensitivity * Time.deltaTime;
        //_currentRotation.x = Mathf.Clamp(_currentRotation.x, -maxUp, maxDown);
        //
        //_currentRotation.y += InputManager.MouseDelta.x * sensitivity * Time.deltaTime;
        //_currentRotation.y = Mathf.Clamp(_currentRotation.y, -maxSide, maxSide);
        //
        //transform.localRotation = Quaternion.Euler(_currentRotation);
    }
}
