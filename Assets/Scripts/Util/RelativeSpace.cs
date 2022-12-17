using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeSpace : MonoBehaviour
{
    private static List<Transform> _allTransforms;
    private static List<Rigidbody> _allRigidbodies;
    private static List<Transform> _allTransformsNoRb;

    public Transform Player;
    private Rigidbody _playerRb;

    [SerializeField]
    private float maxDistance;

    [SerializeField]
    private float maxVelocity;

    public static Vector3 CurrentVelocity { get; private set;}


    // Start is called before the first frame update
    void Awake()
    {
        // find all objects in scene
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Relative");

        _allTransforms = new List<Transform>();
        for(int i = 0; i < objects.Length; i++)
        {
            _allTransforms.Add(objects[i].transform);
        }


        _playerRb = Player.GetComponent<Rigidbody>();

        // get Rigidbodies if present
        _allRigidbodies = new List<Rigidbody>();
        // get transforms of objects without rb
        _allTransformsNoRb = new List<Transform>();

        for(int i = 0; i < objects.Length; i++)
        {
            if(objects[i].TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                _allRigidbodies.Add(rb);
            }
            else
            {
                _allTransformsNoRb.Add(objects[i].transform);
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        #region Position
        // x-axis
        while (Player.position.x > maxDistance)
        {
            moveObjects(new Vector3(-maxDistance, 0, 0));
        }
        while (Player.position.x < -maxDistance)
        {
            moveObjects(new Vector3(maxDistance, 0, 0));
        }

        //y-axis
        while (Player.position.y > maxDistance)
        {
            moveObjects(new Vector3(0, -maxDistance, 0));
        }
        while (Player.position.y < -maxDistance)
        {
            moveObjects(new Vector3(0, maxDistance, 0));
        }


        //z-axis
        while (Player.position.z > maxDistance)
        {
            moveObjects(new Vector3(0, 0, -maxDistance));
        }
        while (Player.position.z < -maxDistance)
        {
            moveObjects(new Vector3(0, 0, maxDistance));
        }
        #endregion

        #region Velocity
        // x-axis
        while (_playerRb.velocity.x > maxVelocity)
        {
            ChangeVelocities(new Vector3(-maxVelocity, 0, 0));
        }
        while (_playerRb.velocity.x < -maxVelocity)
        {
            ChangeVelocities(new Vector3(maxVelocity, 0, 0));
        }

        //y-axis
        while (_playerRb.velocity.y > maxVelocity)
        {
            ChangeVelocities(new Vector3(0, -maxVelocity, 0));
        }
        while (_playerRb.velocity.y < -maxVelocity)
        {
            ChangeVelocities(new Vector3(0, maxVelocity, 0));
        }


        //z-axis
        while (_playerRb.velocity.z > maxVelocity)
        {
            ChangeVelocities(new Vector3(0, 0, -maxVelocity));
        }
        while (_playerRb.velocity.z < -maxVelocity)
        {
            ChangeVelocities(new Vector3(0, 0, maxVelocity));
        }
        #endregion


        // Manually move objects without rb
        foreach(Transform t in _allTransformsNoRb)
        {
            t.position += CurrentVelocity * Time.deltaTime;
        }
    }

    private static void moveObjects(Vector3 movement)
    {
        foreach(Transform t in _allTransforms)
        {
            t.position += movement;
        }

    }

    private static void ChangeVelocities(Vector3 amount)
    {
        foreach (Rigidbody t in _allRigidbodies)
        {
            t.velocity += amount;
        }

        CurrentVelocity += amount;
    }

    public static void addObject(Transform t)
    {
        _allTransforms.Add(t);
        if (t.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            _allRigidbodies.Add(rb);
        }
        else
        {
            _allTransformsNoRb.Add(t);
        }
    }

    public static void removeObject(Transform t)
    {
        _allTransforms.Remove(t);
        if (t.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            _allRigidbodies.Remove(rb);
        }
        else
        {
            _allTransformsNoRb.Remove(t);
        }
    }
}
