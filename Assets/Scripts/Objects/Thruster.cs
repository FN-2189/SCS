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
            if (isNegative && thrustLevel > 0f) return 0f;
            else if (!isNegative && thrustLevel < 0f) return 0f;
            else return type.power * Mathf.Abs(thrustLevel);
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

    }
}
