using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public BulletType type;

    // Start is called before the first frame update
    void Start()
    {
        if(!rb) rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void SendIt(Vector3 v)
    {
        rb.velocity = v;
    }

    public void Hit(Collider hit)
    {
        Debug.Log(gameObject.name + " hit " + hit.gameObject.name);
        Destroy(gameObject);
    }


    /*
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(type.Name + " hit " + collision.gameObject.name);
        Destroy(gameObject);
    }
    */

    private Collider checkHit()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, rb.velocity * Time.fixedDeltaTime, out hit);
        Debug.DrawLine(transform.position, transform.position + rb.velocity * Time.fixedDeltaTime, Color.green, 2, false);
        if (hit.collider == null)
        {
            // Hit nothing will add stuff later
            return null;
        }
        return hit.collider;
    }
}
