using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformScript : MonoBehaviour
{
    //NOTE: If this platform contains H2, it means it is the secondary platform. The secondary
    //      platform does not have a floorheight value set manually in the Unity editor, but is
    //      instead calculated in the slope script, and is dependent on the height difference of
    //      the two platforms.

    public GameObject slope;
    public float floorHeight;
    public bool containsH2;

    void Start()
    {
        if (containsH2 == true) {
            floorHeight = slope.GetComponent<newSlopeScript>().h2;
        }
    }

}
