using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * 100, ForceMode.VelocityChange);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }
}
