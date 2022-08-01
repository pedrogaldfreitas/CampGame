using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newSlopeScript : MonoBehaviour
{

    public enum slopeFacing { HORIZONTAL, VERTICAL };
    [SerializeField]
    public slopeFacing thisSlopeState;

    //NEW VARS:
    public bool isHorizontalSlope;
    public bool isVerticalSlope;
    [SerializeField]
    float multiplier;

    public GameObject platform1;
    public GameObject platform2;

    public float y1;
    public float y2;
    public float x1;
    public float x2;

    //The floorheights of the two platforms attached to this slope.
    public float h1;
    public float h2;

    //Distance between platform 1 and platform 2. (Horizontal slope)
    public float horizontalDistance;
    //Distance between platform 1 and platform 2. (Vertical slope)
    public float verticalDistance;

    //Height of the slope. (measured from top of platform 1 to top of platform 2.)
    public float h3;

    //Value to be exported to ObjectProperties.cs. It's like the rise/run of the slope.
    public float horizontalFloorHeightThreshold;
    public float verticalFloorHeightThreshold;


    // Start is called before the first frame update
    void Awake()
    {
        /*y1 = platform1.transform.position.y;
        y2 = platform2.transform.position.y;
        x1 = platform1.transform.position.x;
        x2 = platform2.transform.position.x;

        h1 = platform1.GetComponent<platformScript>().floorHeight;
        //h2 = platform2.GetComponent<platformScript>().floorHeight;

        //h3 = Mathf.Abs(y2-y1);
        h3 = y2 - y1;
        h2 = h1 + h3;

        if (thisSlopeState == slopeFacing.HORIZONTAL)
        {
            horizontalDistance = this.GetComponent<SpriteRenderer>().bounds.size.x;
            horizontalFloorHeightThreshold = ((h2 - h1) / horizontalDistance)*multiplier;
        } else if (thisSlopeState == slopeFacing.VERTICAL)
        {
            verticalDistance = this.GetComponent<SpriteRenderer>().bounds.size.y;
            verticalFloorHeightThreshold = ((h2 - h1) / verticalDistance)*multiplier;
        }*/ //TEMPORARY COMMENT OUT


    }

}
