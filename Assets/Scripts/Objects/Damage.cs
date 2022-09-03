using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField]
    public float health;

    [SerializeField] ParticleSystem explosionParticleSystem;

    public bool destroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && !destroyed)
        {
            destroyed = true;
            explosionParticleSystem.Play();
            Destroy(gameObject, explosionParticleSystem.main.startLifetime.constant);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "RailgunSlug(Clone)")
        {
            Debug.Log("RailgunHit");
            health -= 100f;
        }
        
        if (collision.gameObject.name == "PDCRound(Clone)")
        {
            Debug.Log("PDCHit");
            health -= 10f;
        }
    }
}
