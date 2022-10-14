using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        foreach (var g in GameObject.FindGameObjectsWithTag("Testing")) g.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
