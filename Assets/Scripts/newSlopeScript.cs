using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newSlopeScript : MonoBehaviour
{
    public enum slopeFacing { HORIZONTAL, VERTICAL };
    [Header("Setup Variables")]
    [SerializeField]
    public slopeFacing thisSlopeState;
    //NEW VARS:
    public bool isHorizontalSlope;
    public bool isVerticalSlope;
    public GameObject platform1;
    public GameObject platform2;
    [Space(20)]

    [SerializeField]
    float multiplier;

    public float y1;
    public float y2;
    public float x1;
    public float x2;

    public float movementMultiplier;

    //The floorheights of the two platforms attached to this slope.
    public float h1;
    public float h2;

    public float slopeAngle;

    //Distance between platform 1 and platform 2. (Horizontal slope)
    public float horizontalDistance;
    //Distance between platform 1 and platform 2. (Vertical slope)
    public float verticalDistance;

    //Height of the slope. (measured from top of platform 1 to top of platform 2.)
    public float h3;

    //Value to be exported to ObjectProperties.cs. It's like the rise/run of the slope.
    public float horizontalFloorHeightThreshold;
    public float verticalFloorHeightThreshold;

    private void Start()
    {
        //Gather important values
        h1 = platform1.transform.Find("top").GetComponent<platformScript>().floorHeight;
        h2 = platform2.transform.Find("top").GetComponent<platformScript>().floorHeight;
        x1 = platform1.transform.Find("base").position.x;
        x2 = platform2.transform.Find("base").position.x;

        if (isHorizontalSlope)
        {
            horizontalFloorHeightThreshold = 1;
        }
        if (isVerticalSlope)
        {
            verticalFloorHeightThreshold = 1;
        }

        //Calculate movement multiplier
        float H = Mathf.Abs(h2 - h1);
        float L = Mathf.Abs(x2 - x1);
        float S = Mathf.Sqrt(Mathf.Pow(H,2) + Mathf.Pow(L, 2));
        movementMultiplier = 1 / S;

        //Calculate angle of triangle
        slopeAngle = Mathf.Atan2(H,L);
    }

}
