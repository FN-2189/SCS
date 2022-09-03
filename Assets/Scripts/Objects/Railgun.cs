using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : MonoBehaviour
{

    [SerializeField] private GameObject pfRailgunShot;
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
        GameObject g = Instantiate(pfRailgunShot, transform.position, transform.rotation);
        g.GetComponent<Rigidbody>().AddForce(transform.forward * 9980, ForceMode.VelocityChange);
        ship.AddForce(transform.forward * -9.98f, ForceMode.Impulse);
    }
}
