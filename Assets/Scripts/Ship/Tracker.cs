using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tracker : MonoBehaviour
{
    public Transform Target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetAimAngles()
    {
        Vector3 relPos = Target.position - transform.position;

        relPos = transform.InverseTransformDirection(relPos);

        Vector2 aim = Vector2.zero;

        // trig and pythagoras
        aim.x = -Mathf.Atan2(relPos.z, relPos.x) * Mathf.Rad2Deg + 90;

        float c = Mathf.Sqrt(relPos.x * relPos.x + relPos.z * relPos.z);

        aim.y = -Mathf.Atan2(relPos.y, c) * Mathf.Rad2Deg;

        return aim;
    }
}
