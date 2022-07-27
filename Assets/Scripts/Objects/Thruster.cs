using UnityEngine;

public class Thruster : MonoBehaviour
{
    public Vector3 position => transform.localPosition;
    public Quaternion rotation => transform.localRotation;
    public float thrust {
        get {
            return power * thrustLevel;      
        }
    }
    
    public virtual float power { get; }
    [SerializeField]
    private float thrustLevel;
}