using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeSpace : MonoBehaviour
{
    private static List<Transform> _allTransforms;

    public Transform Player;

    [SerializeField]
    private float maxDistance;


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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // x-axis
        if(Player.position.x > maxDistance)
        {
            moveObjects(new Vector3(-maxDistance, 0, 0));
        }
        else if (Player.position.x < -maxDistance)
        {
            moveObjects(new Vector3(maxDistance, 0, 0));
        }

        //y-axis
        if (Player.position.y > maxDistance)
        {
            moveObjects(new Vector3(0, -maxDistance, 0));
        }
        else if (Player.position.y < -maxDistance)
        {
            moveObjects(new Vector3(0, maxDistance, 0));
        }


        //z-axis
        if (Player.position.z > maxDistance)
        {
            moveObjects(new Vector3(0, 0, -maxDistance));
        }
        else if (Player.position.z < -maxDistance)
        {
            moveObjects(new Vector3(0, 0, maxDistance));
        }
    }

    private static void moveObjects(Vector3 movement)
    {
        foreach(Transform t in _allTransforms)
        {
            t.position += movement;
        }
    }

    public static void addObject(Transform t)
    {
        _allTransforms.Add(t);
    }

    public static void removeObject(Transform t)
    {
        _allTransforms.Remove(t);
    }
}
