using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : MonoBehaviour
{

    [SerializeField] private Transform pfRailgunShot;
    [SerializeField] ParticleSystem railgunParticles;
    [SerializeField] Rigidbody ship;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartRailgunShot()
    {
        // + new Vector3(0f, -5.189f, 4.25f)
        railgunParticles.Play();
        Instantiate(pfRailgunShot, transform.position, transform.rotation);
        ship.AddForce(transform.forward * -9.98f, ForceMode.VelocityChange);
    }
}
