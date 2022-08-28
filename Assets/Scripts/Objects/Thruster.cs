using UnityEngine;
using System;
using System.Collections.Generic;

public class Thruster : MonoBehaviour
{

    [SerializeField]
    private ThrusterType type;
    public Vector3 position => transform.localPosition;
    public Quaternion rotation => transform.localRotation;
    public float thrust {
        get {
            return type.power * thrustLevel;      
        }
    }

    public Dictionary<ThrusterGroup, bool> isInGroup;
    

    [SerializeField]
    [Range(0,1)]
    public float thrustLevel;

    [SerializeField]
    private ThrusterGroup[] inGroups = { };

    private void Awake()
    {
        isInGroup = new Dictionary<ThrusterGroup, bool>();
        Array vals = Enum.GetValues(typeof(ThrusterGroup));
        foreach(ThrusterGroup t in vals)
        {
            isInGroup.Add(t, false);
        }

        foreach(ThrusterGroup t in inGroups)
        {
            isInGroup[t] = true;
        }

    }
}
