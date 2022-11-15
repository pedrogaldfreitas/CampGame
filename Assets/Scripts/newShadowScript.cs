using System.Collections;
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

    private void Start()
    {
        thisRenderer = GetComponent<Renderer>();
        landTarget = transform.parent.Find("LandTarget");
        wasPrevOnHorizontalSlope = false;
        wasPrevOnVerticalSlope = false;
        totalAmountRisenOrSunk = 0;
    }

    void Update()
    {
        RaycastHit2D horizontalSlopeCheckRay = Physics2D.Raycast(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down, 0f, (1 << 12));
        RaycastHit2D verticalSlopeCheckRay = Physics2D.Raycast(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down, 0f, (1 << 11));
        //RaycastHit2D horizontalSlopeCheckRay = Physics2D.Raycast(transform.position + new Vector3(horizSlopeCheckRayXStart, horizSlopeCheckRayYStart), Vector2.down, horizSlopeCheckRayLength, (1 << 12));
        //RaycastHit2D verticalSlopeCheckRay = Physics2D.Raycast(transform.position + new Vector3(vertiSlopeCheckRayXStart, vertiSlopeCheckRayYStart), Vector2.right, vertiSlopeCheckRayLength, (1 << 11));

        RaycastHit2D[] platformBaseCheckRay = Physics2D.RaycastAll(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down, 0f, (1 << 17));
        //RaycastHit2D[] platformBaseCheckRay = Physics2D.RaycastAll(transform.position + new Vector3(platformCheckRayXStart, platformCheckRayYStart), Vector2.right, platformCheckRayLength, (1 << 17));

        checkFloorHeight();
        sortingOrderAdjust();
        Debug.DrawRay(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down*0.3f, Color.blue);
        //Debug.DrawRay(transform.position + new Vector3(horizSlopeCheckRayXStart, horizSlopeCheckRayYStart), Vector2.down * horizSlopeCheckRayLength, Color.blue);
        //Debug.DrawRay(transform.position + new Vector3(vertiSlopeCheckRayXStart, vertiSlopeCheckRayYStart), Vector2.right * vertiSlopeCheckRayLength, Color.red);
        //Debug.DrawRay(transform.position + new Vector3(platformCheckRayXStart, platformCheckRayYStart), Vector2.right * platformCheckRayLength, Color.green);

        if (horizontalSlopeCheckRay)
        {
            SlopeCheck(horizontalSlopeCheckRay.collider.gameObject, "horizontal");
            onHorizontalSlope = true;
        } else
        {
            onHorizontalSlope = false;
        }

        if (verticalSlopeCheckRay)
        {
            SlopeCheck(verticalSlopeCheckRay.collider.gameObject, "vertical");
        } else
        {
            onVerticalSlope = false;
        }

        if (platformBaseCheckRay.Length > 0)
        {
            platformBaseDetect(platformBaseCheckRay);
        }

        prevXVal = transform.position.x;
        prevYVal = transform.position.y;
        prevGroundVel = transform.position;
    }
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

    //SOURCE OF PROBLEM: Janky jumping likely happens because of this function.
    private void SlopeCheck(GameObject slope, string horizontalOrVertical)
    {
        newSlopeScript slopeScript = slope.GetComponent<newSlopeScript>();
        //onHorizontalSlope = slopeScript.isHorizontalSlope;
        //onVerticalSlope = slopeScript.isVerticalSlope;

        float floorMovementMultiplier = slopeScript.movementMultiplier;
        float slopeAngle = slopeScript.slopeAngle;

        if (horizontalOrVertical == "horizontal" || wasPrevOnHorizontalSlope)
        {
            float slopeValue = slopeScript.horizontalFloorHeightThreshold;
            float slopeFloorH = FindSlopeFloorh(slope.transform);
            if (onHorizontalSlope)
            {
                if ((this.transform.position.x != prevXVal)&&(Mathf.Abs(slopeFloorH - floorHeight) < 2))
                {
                    totalAmountRisenOrSunk += 35 * (transform.position.x - prevXVal) * slopeValue * Time.deltaTime;
                    //Debug.Log("PEDROLOG: totalAmountRisenOrSunk =" + totalAmountRisenOrSunk);
                    if (!onHorizontalSlope && wasPrevOnHorizontalSlope)
                    {
                        float[] twoPlatformHeights = new float[2] { slopeScript.h1, slopeScript.h2 };
                        float platformBeingSteppedOn = twoPlatformHeights.Aggregate((x, y) => Mathf.Abs(x - totalAmountRisenOrSunk) < Mathf.Abs(y - totalAmountRisenOrSunk) ? x : y);
                        Debug.Log("PEDROLOG: The last bit of height needed to be added = " + (platformBeingSteppedOn - totalAmountRisenOrSunk));
                        if (parentObj.GetComponent<FakeHeightObject>().isGrounded)
                        {
                            parentObj.transform.position += (Vector3.up * (platformBeingSteppedOn - totalAmountRisenOrSunk)) * floorMovementMultiplier ;
                        }
                        else
                        {
                            this.transform.position += Vector3.up * (platformBeingSteppedOn - totalAmountRisenOrSunk);
                        }
                    } else
                    {
                        if (parentObj.GetComponent<FakeHeightObject>().isGrounded)
                        {
                            //parentObj.transform.position = new Vector2(35 * (transform.position.x - prevXVal) * slopeValue, transform.position.x - prevXVal).normalized * Time.deltaTime;
                            parentObj.transform.position += Vector3.up * 35 * (transform.position.x - prevXVal) * slopeValue * Time.deltaTime;
                        } else
                        {
                            this.transform.position += Vector3.up * 35 * (transform.position.x - prevXVal) * slopeValue * Time.deltaTime;
                        }
                    }
                }
            }
        }
        
        if (onVerticalSlope)
        {
            float slopeValue = slopeScript.verticalFloorHeightThreshold;
            if ((this.transform.position.y != prevYVal) && (Mathf.Abs(FindSlopeFloorh(slope.transform) - floorHeight) < 2))
            {
                //floorHeight += slopeValue;
                if (parentObj.GetComponent<FakeHeightObject>().isGrounded)
                {
                    parentObj.transform.position += Vector3.up * 35 * (transform.position.y - prevYVal) * slopeValue * Time.deltaTime;
                    //parentObj.transform.position -= Vector3.down * 6 * (transform.position.y - prevYVal) * slopeValue * Time.deltaTime;
                } else
                {
                    this.transform.position += Vector3.up * 35 * (transform.position.y - prevYVal) * slopeValue * Time.deltaTime;
                    //this.transform.position -= Vector3.down * 6 * (transform.position.y - prevYVal) * slopeValue * Time.deltaTime;
                }
            }
        }
        wasPrevOnHorizontalSlope = onHorizontalSlope;
        wasPrevOnVerticalSlope = onVerticalSlope;
       
    }

    //Updates every frame, checks the appropriate floor height of the object.
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
            if (parentObj.tag == "Player")
            {
                slopeFH = FindSlopeFloorh(horizSlope.transform);
                if (slopeFH <= floorHeight+2)
                {
                    if (slopeFH > nearestFloorHeight)
                    {
                        nearestFloorHeight = slopeFH;
                        highestFHisSlope = true;
                    }
                }
            }
        }
        foreach (RaycastHit2D vertiSlope in verticalSlopeCheckRay)
        {
            //Goal here: Find the greatest point in the horizontal slope that's less than the current object's floor height.
            if (parentObj.tag == "Player")
            {
                slopeFH = FindSlopeFloorh(vertiSlope.transform);
                if (slopeFH <= floorHeight + 2)
                {
                    if (slopeFH > nearestFloorHeight)
                    {
                        nearestFloorHeight = slopeFH;
                        highestFHisSlope = true;
                    }
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
    }

    //Takes floorheight of the player if they are standing on a slope.
    private float FindSlopeFloorh(Transform slope)
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
            
            if (parentObj.name == "PlayerParent")
            {
                Debug.Log("slopeTopSide = " + slopeTopSide);
                Debug.Log("slopeBottomSide = " + slopeBottomSide);
                Debug.Log("plat1FH = " + plat1FH);
                Debug.Log("plat2FH = " + plat2FH);
            }

            verticalFH = ((transform.position.y - slopeBottomSide) / (slopeTopSide - slopeBottomSide)); //This should be the % of the way up the slope.

            if (parentObj.name == "PlayerParent")
            {
                Debug.Log("verticalFH_1 = " + verticalFH);
            }
            /*if (plat1FH > plat2FH)
            {
                verticalFH = (1 - verticalFH);
            }*/

            verticalFH = verticalFH * Mathf.Abs(plat2FH - plat1FH);

            if (parentObj.name == "PlayerParent")
            {
                Debug.Log("verticalFH_2 = " + verticalFH);
            }

            if (plat1FH < plat2FH)
            {
                verticalFH = verticalFH + slope.GetComponent<newSlopeScript>().platform1.transform.Find("top").GetComponent<platformScript>().floorHeight;
            }
            else
            {
                verticalFH = verticalFH + slope.GetComponent<newSlopeScript>().platform2.transform.Find("top").GetComponent<platformScript>().floorHeight;
            }

            if (parentObj.name == "PlayerParent")
            {
                Debug.Log("verticalFH_3 = " + verticalFH);
            }
        }

        fh = (horizontalFH > verticalFH) ? horizontalFH : verticalFH;
        return fh;
    }

    public void sortingOrderAdjust()
    {
        //FIGURE OUT: Do I put the offset here or in the positionRendererSort function?
        thisRenderer.sortingOrder = -(int)transform.position.y - offset;
        return;
    }
}
