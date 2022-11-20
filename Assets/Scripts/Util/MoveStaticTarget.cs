using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveStaticTarget : MonoBehaviour
{
    public float t = 10f;
    public Transform target;
    public Vector3 v;
    public float closingSpeed = 3000f;
    private bool restart = true;


    private Rigidbody rb;
    private Rigidbody targetRb;

    private void Start()
    {
        StartCoroutine(Run());
        rb = GetComponent<Rigidbody>();
        targetRb = target.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if ((target.position - transform.position).magnitude < 50f && restart)
        {
            Debug.Log("You've failed!");
            StartCoroutine(Run());
        }
    }


    private IEnumerator Run()
    {
        restart = false;
        GetComponent<Rigidbody>().velocity = v;
        yield return new WaitForSeconds(t);
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        Vector3? tryLeadPoint = LeadCalculator.CaculateLead(targetRb.velocity - rb.velocity, transform.position, target.position, closingSpeed) - transform.position;
        Vector3 leadPoint = Vector3.zero;
        if (tryLeadPoint != null)
        {
            leadPoint = tryLeadPoint.Value;
        }
        else
        {
            Debug.Log("How have you escaped!?");
        }

        GetComponent<Rigidbody>().velocity = transform.InverseTransformDirection(leadPoint.normalized) * closingSpeed;
        restart = true;
        Debug.Log("I will return");
    }
}
