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
        if(transform.position.magnitude > 100000f) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {

    }

    public void SendIt(Vector3 v)
    {
        _v = v;
        transform.position += _v * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        transform.position += _v * Time.fixedDeltaTime;
    }
}
