using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStaticTarget : MonoBehaviour
{
    public float t = 3f;
    public Vector3 v;
    private bool restart = true;

    private void Start()
    {
        GetComponent<Rigidbody>().velocity = v;
    }
    private void Update()
    {
        if(restart) StartCoroutine(Move());
    }


    private IEnumerator Move()
    {
        restart = false;
        GetComponent<Rigidbody>().velocity = -GetComponent<Rigidbody>().velocity;
        yield return new WaitForSeconds(t);
        restart = true;
    }
}
