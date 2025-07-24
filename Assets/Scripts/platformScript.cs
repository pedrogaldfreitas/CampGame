using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class platformScript : MonoBehaviour
{

    public GameObject slope;
    public float floorHeight;
    public bool containsH2;

    void Awake()
    {
        floorHeight = (float)Math.Round(transform.position.y - transform.parent.Find("base").position.y, 3);
        floorHeight = GetSolidHeight(transform.parent);
    }

    //A recursive function to get the absolute solid height. E.g, this platform of fh 5 is on a platform of fh 4, and that platform is on a platform with fh 3. GetSolidHeight returns 5 + 4 + 3.
    private float GetSolidHeight(Transform platform)
    {
        float solidHeight = platform.Find("top").GetComponent<platformScript>().floorHeight;
        Transform hasStartingPlatform = platform.Find("base").GetComponent<BaseScript>().startingPlatform;
        if (hasStartingPlatform)
        {
            return solidHeight + GetSolidHeight(hasStartingPlatform);
        }
        else
        {
            return solidHeight;
        }
    }
}
