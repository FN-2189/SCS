using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public Magazine mag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Launch()
    {
        if(mag.currentAmount > 0)
        {
            GameObject g = Instantiate(mag.missileType, transform.position, transform.rotation);
            Missile m = g.GetComponent<Missile>();
        }
    }
}

public class Magazine
{
    public int size { get; private set; }
    public int currentAmount;
    public GameObject missileType { get; private set; }
}
