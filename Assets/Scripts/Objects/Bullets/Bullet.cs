using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public BulletType type;
    public LineRenderer lineRenderer;

    [SerializeField]
    private float despawnTime = 30f;

    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        if(!rb) rb = GetComponent<Rigidbody>();
    }

    private void OnDestroy()
    {
        BulletManager.RemoveFromList(this);
    }

    public void SendIt(Vector3 v)
    {
        startTime = Time.time;
        rb.velocity = v;
        Destroy(gameObject, despawnTime);
    }

    public void Hit(Collider hit)
    {
        Debug.Log(gameObject.name + " hit " + hit.gameObject.name);
        Destroy(gameObject, despawnTime);
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
