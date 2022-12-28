using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipModule : MonoBehaviour
{
    public bool destroyed;
    public float maxHealth;
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        destroyed = false;
        health = maxHealth;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (health <= 0 && !destroyed)
        {
            destroyed = true;
            Debug.Log($"{gameObject.name} was destroyed");
            ModuleDestroyed();
        }

    }

    public void Repair(float addedhealth)
    {
        float tempHealth = health;

        if (tempHealth > maxHealth * 0.1)
        {
            tempHealth += addedhealth;

            health = Mathf.Min(tempHealth, maxHealth);
        }

    }

    public virtual void ModuleDestroyed() {}
}
