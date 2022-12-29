using UnityEngine;
using System;
using System.Collections.Generic;

public class Thruster : MonoBehaviour
{
    public ThrusterType type;

    private ParticleSystem exhaustParticles;

    public Vector3 position => transform.localPosition;
    public Quaternion rotation => transform.localRotation;
    public float thrust {
        get {
            if (!isActive) return 0f;
            else if (isNegative && thrustLevel > 0f) return 0f;
            else if (!isNegative && thrustLevel < 0f) return 0f;
            else return type.power * Mathf.Clamp01(Mathf.Abs(thrustLevel));
        }
    }

    public Dictionary<Axis, bool> isInAxis;
    

    [SerializeField]
    [Range(0,1)]
    public float thrustLevel;

    [SerializeField]
    private bool isNegative;

    [SerializeField]
    private Axis[] inGroups = { };

    private ParticleSystem.MainModule PSMain;
    private ParticleSystem.EmissionModule PSEmission;

    public bool isActive;

    private void Awake()
    {
        isInAxis = new Dictionary<Axis, bool>();
        Array vals = Enum.GetValues(typeof(Axis));
        foreach(Axis t in vals)
        {
            isInAxis.Add(t, false);
        }

        foreach(Axis t in inGroups)
        {
            isInAxis[t] = true;
        }
        exhaustParticles = Instantiate(type.exhaust, gameObject.transform);
        exhaustParticles.transform.rotation = Quaternion.Euler(-exhaustParticles.transform.rotation.eulerAngles);
        PSMain = exhaustParticles.main;
        PSEmission = exhaustParticles.emission;

        PSEmission.rateOverTimeMultiplier = 0f;
        exhaustParticles.Play();
    }

    private void Update()
    {
        if (!isActive) 
        { 
            PSEmission.rateOverTimeMultiplier = 0f; 
            return; 
        }
        if((thrustLevel > 0f && !isNegative) || (thrustLevel < 0f && isNegative))
        {
            PSEmission.rateOverTimeMultiplier = type.exhaust.emission.rateOverTimeMultiplier;
            PSMain.startLifetimeMultiplier = type.exhaust.main.startLifetimeMultiplier * Mathf.Abs(thrustLevel);
        }
        else
        {
            PSEmission.rateOverTimeMultiplier = 0f;
        }
    }
}
