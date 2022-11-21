using System.Collections;
using UnityEngine;

namespace Assets
{
    public class ShotCounter : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            print(new Vector3(21341.6035f, 15167.1494f, 8750.06445f).magnitude);
        }

        // Update is called once per frame
        void Update()
        {
            print(GameObject.FindGameObjectsWithTag("Bullet").Length);
        }
    }
}