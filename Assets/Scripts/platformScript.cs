﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformScript : MonoBehaviour
{

    public GameObject slope;
    public float floorHeight;
    public bool containsH2;

    void Start()
    {
        floorHeight = transform.position.y - transform.parent.Find("base").position.y;
    }

}
