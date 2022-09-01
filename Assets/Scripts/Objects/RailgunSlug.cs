using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunSlug : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(transform.forward * 9980, ForceMode.VelocityChange);
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {

    }
}
