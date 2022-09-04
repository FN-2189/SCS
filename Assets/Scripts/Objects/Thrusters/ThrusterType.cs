using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Thruster")]
public class ThrusterType : ScriptableObject
{
    public float power;
    public ParticleSystem exhaust;
}