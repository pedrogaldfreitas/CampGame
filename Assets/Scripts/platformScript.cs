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
    }

}
