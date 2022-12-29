using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : PowerModule
{
    [Header("Tracker")]
    public Transform Target;

    private List<Trackfile> trackedTargets = new();

    // Start is called before the first frame update
    void Start()
    {
        trackedTargets.Clear();
        Trackfile target = new("Target 1")
        {
            ID = 1
        };
        trackedTargets.Add(target);
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        // this thing is lagging at least one frame behind, other than that it's on point


        Trackfile track = GetFile(1);

        if(moduleActive) track.UpdateFile(Target.position - transform.position, Target.rotation * Quaternion.Inverse(transform.rotation));
        if(DebugManager.instance.GetSettingState("tracker_log")) Debug.Log($"Target: {track.Name}: Position: {track.Position}, Velocity: {track.Velocity}m/s, Acceleration: {track.Acceleration}m/s²");

        Debug.DrawRay(transform.position, trackedTargets[0].Position, Color.red);
    }

    public Trackfile GetFile(int ID)
    {
        return trackedTargets.Find(t => t.ID == ID);
    }

    public Trackfile GetFile(string name)
    {
        return trackedTargets.Find(t => t.Name == name);
    }
}

[System.Serializable]
public class Trackfile
{
    public string Name;
    public int ID;
    public bool Valid { get { return _collectedSamples >= 3; } }
    private int _collectedSamples = 0;
    private Vector3[] _lastPositions;
    public Vector3 Position { get { return _lastPositions[0]; } }
    public Quaternion Direction { 
        get
        {
            return Quaternion.LookRotation(_lastPositions[0]);
        }
    }

    private Vector3[] _lastVelocities;
    public Vector3 Velocity {
        get
        {
            // this negativ shouldn't be needed here need to fix
            return _lastVelocities[0] - Acceleration * Time.fixedDeltaTime;
        }
    }
    public Vector3 Acceleration {
        get
        {
            return MathHelper.Derive(_lastVelocities[1], _lastVelocities[0], Time.fixedDeltaTime);
        } 
    }
    private Quaternion[] _lastRotations;
    public Quaternion Rotation { get { return _lastRotations[0]; } }
    public float Size;
    public float Aspect {
        get
        {
            return Quaternion.Angle(Direction, _lastRotations[0]);
        }
    }

    public Trackfile(string name, int bufferSize = 10)
    {
        _lastPositions = new Vector3[bufferSize];
        _lastVelocities = new Vector3[bufferSize];
        _lastRotations = new Quaternion[bufferSize];
    }

    public void UpdateFile(Vector3 newPosition, Quaternion newRotation)
    {
        _lastPositions = ArrayUtils.AdvanceQueue(_lastPositions, newPosition);
        _lastVelocities = ArrayUtils.AdvanceQueue(_lastVelocities, MathHelper.Derive(_lastPositions[1], _lastPositions[0], Time.fixedDeltaTime));
        _lastRotations = ArrayUtils.AdvanceQueue(_lastRotations, newRotation);
        _collectedSamples++;
    }
}
