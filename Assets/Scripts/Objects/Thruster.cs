using UnityEngine;

public class Thruster : MonoBehaviour
{
    public Vector3 position;
    public Vector3 rotation;
    public float thrust;

    private void Start()
    {
        this.position = transform.localPosition;
        this.rotation = transform.localEulerAngles;
    }
}