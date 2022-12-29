using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipModule : MonoBehaviour
{
    [Header("Ship Module")]
    public bool destroyed;
    public float maxHealth;
    public float health;
    public Collider collider;

    void Start()
    {
        destroyed = false;
        health = maxHealth;
        collider.isTrigger = true;
    }

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

    public virtual void Damage(float damage)
    {
        float tempHealth = health;
        tempHealth -= damage;

        health = Mathf.Max(tempHealth, 0f);
    }
}
