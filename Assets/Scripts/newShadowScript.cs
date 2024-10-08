﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class newShadowScript : MonoBehaviour
{
    [Range(0f, 3f)] public float raycastDistanceMultiplier;
    public bool checkingForEdge = true;
    public float floorHeight;
    public GameObject edge;

    //Slope Variables
    private float prevXVal;
    private float prevYVal;
    public bool onHorizontalSlope;
    private bool wasPrevOnHorizontalSlope;
    public bool onVerticalSlope;
    private bool wasPrevOnVerticalSlope;
    float slopeFH;

    private float totalAmountRisenOrSunk;

    private Renderer thisRenderer;

    public GameObject parentObj;
    public GameObject mainObj;
    public Transform landTarget;

    [SerializeField]
    int offset;

    public float mult;

    //TEMPORARY FOR TESTING:
    public float horizSlopeCheckRayXStart;
    public float horizSlopeCheckRayYStart;
    public float horizSlopeCheckRayLength;
    public float vertiSlopeCheckRayXStart;
    public float vertiSlopeCheckRayYStart;
    public float vertiSlopeCheckRayLength;
    public float platformCheckRayXStart;
    public float platformCheckRayYStart;
    public float platformCheckRayLength;

    private Vector2 prevGroundVel;
    RaycastHit2D horizontalSlopeCheckRay;
    RaycastHit2D verticalSlopeCheckRay;

    bool flag;

    private void Start()
    {
        thisRenderer = GetComponent<Renderer>();
        landTarget = transform.parent.Find("LandTarget");
        wasPrevOnHorizontalSlope = false;
        wasPrevOnVerticalSlope = false;
        totalAmountRisenOrSunk = 0;
        flag = true;
    }

    void Update()
    {
        RaycastHit2D[] platformBaseCheckRay = Physics2D.RaycastAll(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down, 0f, (1 << 17));
        //RaycastHit2D[] platformBaseCheckRay = Physics2D.RaycastAll(transform.position + new Vector3(platformCheckRayXStart, platformCheckRayYStart), Vector2.right, platformCheckRayLength, (1 << 17));

        //checkFloorHeight();
        sortingOrderAdjust();
        //Debug.DrawRay(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down*0.3f, Color.blue);
        //Debug.DrawRay(transform.position + new Vector3(horizSlopeCheckRayXStart, horizSlopeCheckRayYStart), Vector2.down * horizSlopeCheckRayLength, Color.blue);
        //Debug.DrawRay(transform.position + new Vector3(vertiSlopeCheckRayXStart, vertiSlopeCheckRayYStart), Vector2.right * vertiSlopeCheckRayLength, Color.red);
        //Debug.DrawRay(transform.position + new Vector3(platformCheckRayXStart, platformCheckRayYStart), Vector2.right * platformCheckRayLength, Color.green);


        if (platformBaseCheckRay.Length > 0)
        {
            //platformBaseDetect(platformBaseCheckRay);
        }

        prevXVal = transform.position.x;
        prevYVal = transform.position.y;
        prevGroundVel = transform.position;
    }

    /*
    public void platformBaseDetect(RaycastHit2D[] platformBaseCheckRay)
    {
        if (wasPrevOnHorizontalSlope)
        {
            if (totalAmountRisenOrSunk != 0)
            {
                //Debug.Log("PEDROLOG: went up a total of " + totalAmountRisenOrSunk);
            }
            totalAmountRisenOrSunk = 0;
        }

        //Goal: Raise shadow position if the object is above the platform.
        List<float> platformFloorHeightArray = new List<float>();
        foreach(RaycastHit2D platformHit in platformBaseCheckRay)
        {
            platformFloorHeightArray.Add(platformHit.transform.parent.Find("top").GetComponent<platformScript>().floorHeight);
        }

        platformFloorHeightArray.Sort();

        FakeHeightObject thisObjHeightInfo = parentObj.GetComponent<FakeHeightObject>();

        bool stopFlag = false;
        int index = 0;
        while (!stopFlag && index < platformFloorHeightArray.Count)
        {
            if ((floorHeight + (thisObjHeightInfo.height + thisObjHeightInfo.shadowOffset) > platformFloorHeightArray[index]) && (floorHeight != platformFloorHeightArray[index]))
            {
                parentObj.GetComponent<FakeHeightObject>().Rise(platformFloorHeightArray[index]);
                stopFlag = true;
            } else
            {
                index++;
            }
        }
    }
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Slope")
        {
            newSlopeScript slopeScript = collision.transform.GetComponent<newSlopeScript>();
            BaseScript platform1BaseScript = slopeScript.platform1.transform.Find("base").GetComponent<BaseScript>();
            BaseScript platform2BaseScript = slopeScript.platform2.transform.Find("base").GetComponent<BaseScript>();
            platform1BaseScript.ToggleIgnoreObjectOnSlope(landTarget.GetComponent<Collider2D>(), true);
            platform2BaseScript.ToggleIgnoreObjectOnSlope(landTarget.GetComponent<Collider2D>(), true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Slope")
        {
            newSlopeScript slopeScript = collision.transform.GetComponent<newSlopeScript>();
            BaseScript platform1BaseScript = slopeScript.platform1.transform.Find("base").GetComponent<BaseScript>();
            BaseScript platform2BaseScript = slopeScript.platform2.transform.Find("base").GetComponent<BaseScript>();
            platform1BaseScript.ToggleIgnoreObjectOnSlope(landTarget.GetComponent<Collider2D>(), false);
            platform2BaseScript.ToggleIgnoreObjectOnSlope(landTarget.GetComponent<Collider2D>(), false);
        }
    }

    //Updates every frame, checks the appropriate floor height of the object.
    /*
    public void checkFloorHeight()
    {
        float nearestFloorHeight = -99999;
        RaycastHit2D[] platformCheckRay = Physics2D.RaycastAll(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down, 0.3f, (1 << 16));
        RaycastHit2D[] horizontalSlopeCheckRay = Physics2D.RaycastAll(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down, 0.3f, (1 << 12));
        RaycastHit2D[] verticalSlopeCheckRay = Physics2D.RaycastAll(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down, 0.3f, (1 << 11));
        float highestPlatformFH = 0;
        bool highestFHisSlope = false;

        foreach (RaycastHit2D platform in platformCheckRay)
        {
            //Goal here: Find the greatest floor height that is still less than the current object's floor height.
            if (platform.transform.parent.Find("top").GetComponent<platformScript>().floorHeight <= floorHeight+2)
            {
                if (platform.transform.parent.Find("top").GetComponent<platformScript>().floorHeight > nearestFloorHeight)
                {
                    nearestFloorHeight = platform.transform.parent.Find("top").GetComponent<platformScript>().floorHeight;
                    highestPlatformFH = nearestFloorHeight;
                }
            }
        }
   
        slopeFH = -999;
        foreach (RaycastHit2D horizSlope in horizontalSlopeCheckRay)
        {
            //Goal here: Find the greatest point in the horizontal slope that's less than the current object's floor height.
            slopeFH = FloorheightFunctions.FindSlopeFloorh(transform, horizSlope.transform.parent.Find("top").transform);
            if (slopeFH <= floorHeight+2)
            {
                if (slopeFH > nearestFloorHeight)
                {
                    nearestFloorHeight = slopeFH;
                    highestFHisSlope = true;
                }
            }
        }
        
        foreach (RaycastHit2D vertiSlope in verticalSlopeCheckRay)
        {
            //Goal here: Find the greatest point in the horizontal slope that's less than the current object's floor height.
            slopeFH = FloorheightFunctions.FindSlopeFloorh(transform, vertiSlope.transform);
            if (slopeFH <= floorHeight + 2)
            {
                if (slopeFH > nearestFloorHeight)
                {
                    nearestFloorHeight = slopeFH;
                    highestFHisSlope = true;
                }
            }
        }

        if ( (!onVerticalSlope)&&(!onHorizontalSlope)&&(Mathf.Abs(floorHeight-nearestFloorHeight) < 1) ) {
            floorHeight = nearestFloorHeight;
        } 
        else
        {
            if (highestFHisSlope)
            {
                floorHeight = slopeFH;
            } else
            {
                if (floorHeight > nearestFloorHeight)
                {
                    if (Mathf.Abs(floorHeight - nearestFloorHeight) < 1)
                    {
                        floorHeight = nearestFloorHeight;
                    } else
                    {
                        parentObj.GetComponent<FakeHeightObject>().Drop(((Vector2)transform.position - prevGroundVel) * 30, floorHeight - nearestFloorHeight);
                    }
                }
            }
        }
    }*/

    //Takes floorheight of the player if they are standing on a slope.

    //NOTE: This has been taken and put into FloorheightFunctions.cs. This is to make this function more accessible by other scripts.

   /* private float FindSlopeFloorh(Transform slope)
    {
        float fh = 0;
        float horizontalFH = -9999;
        float verticalFH = -9999;
        if (slope.GetComponent<newSlopeScript>().isHorizontalSlope)
        {
            float slopeLeftSide = slope.transform.position.x - slope.transform.GetComponent<PolygonCollider2D>().bounds.extents.x;
            float slopeRightSide = slope.transform.position.x + slope.transform.GetComponent<PolygonCollider2D>().bounds.extents.x;

            float plat1FH = slope.GetComponent<newSlopeScript>().platform1.transform.Find("top").GetComponent<platformScript>().floorHeight;
            float plat2FH = slope.GetComponent<newSlopeScript>().platform2.transform.Find("top").GetComponent<platformScript>().floorHeight;
            //float objRelativeXPos = slopeLeftSide + this.transform.position.x;
            horizontalFH = ((transform.position.x - slopeLeftSide) / (slopeRightSide - slopeLeftSide)); //This should be the % of the way up the slope.

            if (plat1FH > plat2FH)
            {
                horizontalFH = (1 - horizontalFH);
            }

            horizontalFH = horizontalFH * Mathf.Abs((slope.GetComponent<newSlopeScript>().platform2.transform.Find("top").GetComponent<platformScript>().floorHeight) - (slope.GetComponent<newSlopeScript>().platform1.transform.Find("top").GetComponent<platformScript>().floorHeight));

            if (plat1FH < plat2FH)
            {
                horizontalFH = horizontalFH + slope.GetComponent<newSlopeScript>().platform1.transform.Find("top").GetComponent<platformScript>().floorHeight;
            } else
            {
                horizontalFH = horizontalFH + slope.GetComponent<newSlopeScript>().platform2.transform.Find("top").GetComponent<platformScript>().floorHeight;
            }
        }
        if (slope.GetComponent<newSlopeScript>().isVerticalSlope)
        {
            float slopeTopSide = slope.transform.position.y + slope.transform.GetComponent<PolygonCollider2D>().bounds.extents.y;
            float slopeBottomSide = slope.transform.position.y - slope.transform.GetComponent<PolygonCollider2D>().bounds.extents.y;

            float plat1FH = slope.GetComponent<newSlopeScript>().platform1.transform.Find("top").GetComponent<platformScript>().floorHeight;
            float plat2FH = slope.GetComponent<newSlopeScript>().platform2.transform.Find("top").GetComponent<platformScript>().floorHeight;

            verticalFH = ((transform.position.y - slopeBottomSide) / (slopeTopSide - slopeBottomSide)); //This should be the % of the way up the slope.

            verticalFH = verticalFH * Mathf.Abs(plat2FH - plat1FH);

            if (plat1FH < plat2FH)
            {
                verticalFH = verticalFH + slope.GetComponent<newSlopeScript>().platform1.transform.Find("top").GetComponent<platformScript>().floorHeight;
            }
            else
            {
                verticalFH = verticalFH + slope.GetComponent<newSlopeScript>().platform2.transform.Find("top").GetComponent<platformScript>().floorHeight;
            }

        }

        fh = (horizontalFH > verticalFH) ? horizontalFH : verticalFH;
        if (flag)
        {
            flag = false;
        }
        return fh;
    }*/

    public void sortingOrderAdjust()
    {
        //FIGURE OUT: Do I put the offset here or in the positionRendererSort function?
        thisRenderer.sortingOrder = -(int)transform.position.y - offset;
        return;
    }
}
