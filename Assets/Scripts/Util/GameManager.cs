using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string s = "";
        foreach (double d in MathHelper.SolveQuarticReal(1, 2, -10, 4, 2)) s += $"{d} : ";
        Debug.Log(s);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
