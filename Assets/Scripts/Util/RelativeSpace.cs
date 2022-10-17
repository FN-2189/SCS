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
    void LateUpdate()
    {
        // x-axis
        while(Player.position.x > maxDistance)
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
