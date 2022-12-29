using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : ShipModule
{
    [SerializeField]
    private ParticleSystem reactorParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

    }

    public override void ModuleDestroyed()
    {
        Debug.Log("Reactor goes boom");
        reactorParticleSystem.Play();
    }

}
