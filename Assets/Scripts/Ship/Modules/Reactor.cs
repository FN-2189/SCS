using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : ShipModule
{
    [SerializeField]
    private GameObject explosionPrefab;

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
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(explosion, 30f);
    }

}
