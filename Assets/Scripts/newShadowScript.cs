using System.Collections;
using System.Collections.Generic;
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
    public bool onVerticalSlope;
    float slopeFH;

    private Renderer thisRenderer;

    public GameObject parentObj;
    public GameObject mainObj;

    [SerializeField]
    int offset;

    public float mult;

    private Vector2 prevGroundVel;

    private void Start()
    {
        thisRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        RaycastHit2D horizontalSlopeCheckRay = Physics2D.Raycast(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down, 0.3f, (1 << 12));
        RaycastHit2D verticalSlopeCheckRay = Physics2D.Raycast(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down, 0.3f, (1 << 11));
        //RaycastHit2D cliffsideCheckRay1 = Physics2D.Raycast(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down, 0.3f, (1 << 15));//Middle
        //RaycastHit2D cliffsideCheckRay2 = Physics2D.Raycast(transform.position + Vector3.down * raycastDistanceMultiplier + Vector3.left*mult, Vector2.down, 0.3f, (1 << 15));//left
        //RaycastHit2D cliffsideCheckRay3 = Physics2D.Raycast(transform.position + Vector3.down * raycastDistanceMultiplier + Vector3.right*mult, Vector2.down, 0.3f, (1 << 15));//right

        RaycastHit2D[] platformBaseCheckRay = Physics2D.RaycastAll(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down, 0.3f, (1 << 17));

        checkFloorHeight();
        sortingOrderAdjust();

        //The cliffside check rays, visualized.
        Debug.DrawRay(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down*0.3f, Color.blue);
        //Debug.DrawRay(transform.position + Vector3.down * raycastDistanceMultiplier + Vector3.left * mult, Vector2.down * 0.3f, Color.green);
        //Debug.DrawRay(transform.position + Vector3.down * raycastDistanceMultiplier + Vector3.right * mult, Vector2.down * 0.3f, Color.green);

        if (horizontalSlopeCheckRay)
        {
            SlopeCheck(horizontalSlopeCheckRay.collider.gameObject);
        } else
        {
            onHorizontalSlope = false;
        }

        if (verticalSlopeCheckRay)
        {
            SlopeCheck(verticalSlopeCheckRay.collider.gameObject);
        } else
        {
            onVerticalSlope = false;
        }

        /*if (cliffsideCheckRay1)
        {
            //NOTE: Make sure the parameter is the height of the cliff.
            cliffDetect(cliffsideCheckRay1, cliffsideCheckRay1.transform.localScale.y*((BoxCollider2D)cliffsideCheckRay1.collider).size.y); 
        }*/

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
        //Goal: Raise shadow position if the object is above the platform.
        float platformHeight = platformBaseCheckRay[0].transform.parent.Find("top").GetComponent<platformScript>().floorHeight;
        if ((floorHeight + parentObj.GetComponent<FakeHeightObject>().height > platformHeight)&&(floorHeight != platformBaseCheckRay[0].transform.parent.Find("top").GetComponent<platformScript>().floorHeight))
        {
            parentObj.GetComponent<FakeHeightObject>().Rise(platformHeight);
        }
    }

    //This function is called when the object falls off a cliff.
    /*private void cliffDetect(RaycastHit2D cliff, float fallHeight)
    {
        Debug.Log("cliffDetect ran.");
        if (floorHeight >= cliff.transform.GetComponent<wallScript>().upperPlatform.GetComponent<platformScript>().floorHeight)
        {
            Physics2D.IgnoreCollision(mainObj.GetComponents<BoxCollider2D>()[0], cliff.collider, true);
        } else
        {
            if (parentObj.GetComponent<FakeHeightObject>().isGrounded)
            {
                Physics2D.IgnoreCollision(mainObj.GetComponents<BoxCollider2D>()[0], cliff.collider, false);
            }
        }
    }*/


    //SOURCE OF PROBLEM: Janky jumping likely happens because of this function.
    private void SlopeCheck(GameObject slope)
    {
        onHorizontalSlope = slope.GetComponent<newSlopeScript>().isHorizontalSlope;
        onVerticalSlope = slope.GetComponent<newSlopeScript>().isVerticalSlope;

        if (onHorizontalSlope)
        {
            float slopeValue = slope.GetComponent<newSlopeScript>().horizontalFloorHeightThreshold;

            if ((this.transform.position.x != prevXVal)&&(Mathf.Abs(FindSlopeFloorh(slope.transform) - floorHeight) < 2))
            {
                if (parentObj.GetComponent<FakeHeightObject>().isGrounded)
                {
                    parentObj.transform.position += Vector3.up * 35 * (transform.position.x - prevXVal) * slopeValue * Time.deltaTime;
                } else
                {
                    this.transform.position += Vector3.up * 35 * (transform.position.x - prevXVal) * slopeValue * Time.deltaTime;
                }
            }
        }
        if (onVerticalSlope)
        {
            float slopeValue = slope.GetComponent<newSlopeScript>().verticalFloorHeightThreshold;
            if ((this.transform.position.y != prevYVal) && (Mathf.Abs(FindSlopeFloorh(slope.transform) - floorHeight) < 2))
            {
                //floorHeight += slopeValue;
                if (parentObj.GetComponent<FakeHeightObject>().isGrounded)
                {
                    parentObj.transform.position -= Vector3.down * 6 * (transform.position.y - prevYVal) * slopeValue * Time.deltaTime;
                } else
                {
                    this.transform.position -= Vector3.down * 6 * (transform.position.y - prevYVal) * slopeValue * Time.deltaTime;
                }
            }
        }


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
            if (platform.transform.GetComponent<platformScript>().floorHeight <= floorHeight+2)
            {
                if (platform.transform.GetComponent<platformScript>().floorHeight > nearestFloorHeight)
                {
                    nearestFloorHeight = platform.transform.GetComponent<platformScript>().floorHeight;
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
                        //parentObj.GetComponent<FakeHeightObject>().Drop(new Vector2(0, 0), floorHeight - nearestFloorHeight); TEMPORARILY DISABLED FOR TESTING
                        //THERE IS LIKELY A PROBLEM HERE
                        //parentObj.GetComponent<FakeHeightObject>().Drop(((Vector2)transform.position-prevGroundVel)*30, floorHeight - nearestFloorHeight, GameObject.Find("bridgeLeftSide").transform.Find("solid").gameObject);
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

            float plat1FH = slope.GetComponent<newSlopeScript>().platform1.GetComponent<platformScript>().floorHeight;
            float plat2FH = slope.GetComponent<newSlopeScript>().platform2.GetComponent<platformScript>().floorHeight;

            //float objRelativeXPos = slopeLeftSide + this.transform.position.x;
            horizontalFH = ((transform.position.x - slopeLeftSide) / (slopeRightSide - slopeLeftSide)); //This should be the % of the way up the slope.

            if (plat1FH > plat2FH)
            {
                horizontalFH = (1 - horizontalFH);
            }

            horizontalFH = horizontalFH * Mathf.Abs((slope.GetComponent<newSlopeScript>().platform2.GetComponent<platformScript>().floorHeight) - (slope.GetComponent<newSlopeScript>().platform1.GetComponent<platformScript>().floorHeight));

            if (plat1FH < plat2FH)
            {
                horizontalFH = horizontalFH + slope.GetComponent<newSlopeScript>().platform1.GetComponent<platformScript>().floorHeight;
            } else
            {
                horizontalFH = horizontalFH + slope.GetComponent<newSlopeScript>().platform2.GetComponent<platformScript>().floorHeight;
            }
        }
        if (slope.GetComponent<newSlopeScript>().isVerticalSlope)
        {
            float slopeTopSide = slope.transform.position.y + slope.transform.GetComponent<PolygonCollider2D>().bounds.extents.y;
            float slopeBottomSide = slope.transform.position.y - slope.transform.GetComponent<PolygonCollider2D>().bounds.extents.y;

            float plat1FH = slope.GetComponent<newSlopeScript>().platform1.GetComponent<platformScript>().floorHeight;
            float plat2FH = slope.GetComponent<newSlopeScript>().platform2.GetComponent<platformScript>().floorHeight;
            
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
                verticalFH = verticalFH + slope.GetComponent<newSlopeScript>().platform1.GetComponent<platformScript>().floorHeight;
            }
            else
            {
                verticalFH = verticalFH + slope.GetComponent<newSlopeScript>().platform2.GetComponent<platformScript>().floorHeight;
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
