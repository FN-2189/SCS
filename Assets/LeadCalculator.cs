using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadCalculator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Vector3 CaculateLead(Vector3 relV, Vector3 pos, Vector3 targetPos, float bulletV) // it no work AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
    {
        Vector3 q = targetPos - pos;

        //solving quadratic equation from t*t(Vx*Vx + Vy*Vy - S*S) + 2*t*(Vx*Qx)(Vy*Qy) + Qx*Qx + Qy*Qy = 0

        float a = Vector3.Dot(relV, relV) - (bulletV * bulletV); //Dot is basicly (targetSpeed.x * targetSpeed.x) + (targetSpeed.y * targetSpeed.y)
        float b = 2 * Vector3.Dot(relV, q); //Dot is basicly (targetSpeed.x * q.x) + (targetSpeed.y * q.y)
        float c = Vector3.Dot(q, q); //Dot is basicly (q.x * q.x) + (q.y * q.y)

        //Discriminant
        float D = Mathf.Sqrt((b * b) - 4 * a * c);

        float t1 = (-b + D) / (2 * a);
        float t2 = (-b - D) / (2 * a);

        //Debug.Log("t1: " + t1 + " t2: " + t2);

        float time = Mathf.Max(t1, t2);

        Vector3 ret = targetPos + relV * time;
        return ret;
    }
}
