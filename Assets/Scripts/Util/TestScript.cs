using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        string compound = "";
        foreach (double i in MathHelper.SolveQuarticReal(1, 2, 3, 4, 5))
        {
            compound += (i + ", ");            
        }
        Debug.Log(compound);
    }
}
