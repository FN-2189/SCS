using UnityEngine;

public class Thruster : MonoBehaviour
{
    public Vector3 position;
    public Vector3 rotation;
    public float thrust;

    private void Awake()
    {
        position = transform.localPosition;
        rotation = transform.localEulerAngles;
    }
}