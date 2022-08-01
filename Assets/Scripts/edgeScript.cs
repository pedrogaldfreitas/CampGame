using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class edgeScript : MonoBehaviour
{
    public GameObject tallerPlatform;
    public GameObject lowerPlatform;
    public GameObject wall;
    public float floorHeightDiff;

    public bool colliderOn;

    void Start()
    {
        floorHeightDiff = tallerPlatform.GetComponent<platformScript>().floorHeight - lowerPlatform.GetComponent<platformScript>().floorHeight;
    }

    private void Update()
    {
        //if //player overlaps box aroudn the collider (so that the following code doesn't play when unnecessary)
        {
            if (GameObject.Find("Player").GetComponent<ObjectProperties>().floorHeight == tallerPlatform.GetComponent<platformScript>().floorHeight)
            {
                colliderOn = true;
            } else
            {
                colliderOn = false;
            }
        }

    }


}
