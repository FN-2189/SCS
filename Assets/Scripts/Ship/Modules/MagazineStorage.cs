using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineStorage : ShipModule
{
    [SerializeField]
    private GameObject explosionPrefab;

    public int ammoCount;

    void Start()
    {
        
    }

    new void Update()
    {
        base.Update();
    }

    public override void ModuleDestroyed()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform);

        Destroy(explosion, 30f);
    }
}
