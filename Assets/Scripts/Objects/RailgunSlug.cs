using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunSlug : MonoBehaviour
{
    private Vector3 _v;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.magnitude > 20000f) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Hit " + collision.gameObject.name);
        Destroy(gameObject);
    }

    public void SendIt(Vector3 v, Collider shooterCollider)
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), shooterCollider, true);
        GetComponent<Rigidbody>().velocity = v;
    }

    private void FixedUpdate()
    {
        //transform.position += _v * Time.fixedDeltaTime;
    }
}
