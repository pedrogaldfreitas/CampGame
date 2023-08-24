using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LandTargetScript : MonoBehaviour
{
    private newShadowScript shadowScript;
    private Transform parentTransform;
    private Transform shadowTransform;

    //private float floorHeight;
    private Transform currentPlatformParent;

    RaycastHit2D[] baseCheckRay;
    [Range(0f, 3f)] public float raycastDistanceMultiplier;
    private float prevXVal;
    private float prevYVal;
    private Vector2 prevGroundVel;
    private float totalAmountRisenOrSunk;

    public bool onHorizontalSlope;
    public bool onVerticalSlope;
    private bool wasPrevOnHorizontalSlope;

    private void Start()
    {
        totalAmountRisenOrSunk = 0;

        parentTransform = transform.parent;
        shadowTransform = parentTransform.Find("Shadow");
        shadowScript = shadowTransform.GetComponent<newShadowScript>();
    }

    private void Update()
    {
        this.transform.position = new Vector2(transform.position.x, shadowScript.transform.position.y - shadowScript.floorHeight);
        baseCheckRay = Physics2D.RaycastAll(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down, 0f, (1 << 17));

        Debug.DrawRay(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down * 0.3f, Color.blue);

        //Here: Determine the most accurate floorheight to set, and if it is of a horizontal or vertical slope.
        if (baseCheckRay.Length > 0)
        {
            baseDetect(baseCheckRay);
        }
        checkFloorHeight(baseCheckRay);

        if (parentTransform.name == "PlayerParent")
        {
            Debug.Log("PEDROLOG/0: currentPlatformParent name = " + currentPlatformParent?.name + ", layer = " + currentPlatformParent?.transform.gameObject.layer + "onHorizontalSlope = " + onHorizontalSlope);
        }

        if (onHorizontalSlope)
        {
            SlopeCheck(currentPlatformParent.Find("top").gameObject, "horizontal");
        } else if (onVerticalSlope)
        {
            SlopeCheck(currentPlatformParent.Find("top").gameObject, "vertical");
        }

        //These must be set AFTER the SlopeCheck call here on Update().
        prevXVal = transform.position.x;
        prevYVal = transform.position.y;
        prevGroundVel = transform.position;
    }


    //To be used exclusively by baseDetect().
    class baseAndFloorheightPair
    {
        public float floorHeight;
        public RaycastHit2D rayHit;
        public bool isSlope;
        public baseAndFloorheightPair(float fh, RaycastHit2D raycastHit, bool slope)
        {
            floorHeight = fh;
            rayHit = raycastHit;
            isSlope = slope;
        }
    }

    public void baseDetect(RaycastHit2D[] baseCheckRay)
    {
        if (wasPrevOnHorizontalSlope)
        {
            if (totalAmountRisenOrSunk != 0)
            {
            }
            totalAmountRisenOrSunk = 0;
        }      

        //Goal: Raise shadow position if the object is above the platform.
        List<baseAndFloorheightPair> baseAndFloorHeightArray = new List<baseAndFloorheightPair>();
        foreach (RaycastHit2D baseHit in baseCheckRay)
        {
            int baseObjLayer = baseHit.transform.parent.gameObject.layer;
            if (baseObjLayer == LayerMask.NameToLayer("HorizontalSlope") || baseObjLayer == LayerMask.NameToLayer("VerticalSlope"))
            {
                newSlopeScript slopeScript = baseHit.transform.parent.Find("top").GetComponent<newSlopeScript>();
                float lowestPlatformFH = System.Math.Min(slopeScript.h1, slopeScript.h2);
                baseAndFloorHeightArray.Add(new baseAndFloorheightPair(lowestPlatformFH, baseHit, true));
            } else
            {
                baseAndFloorHeightArray.Add(new baseAndFloorheightPair(baseHit.transform.parent.Find("top").GetComponent<platformScript>().floorHeight, baseHit, false));
            }
        }

        baseAndFloorHeightArray = baseAndFloorHeightArray.OrderBy(o => o.floorHeight).ToList();

        FakeHeightObject thisObjHeightInfo = parentTransform.GetComponent<FakeHeightObject>();
        float thisObjHeight = shadowScript.floorHeight + (thisObjHeightInfo.height + thisObjHeightInfo.shadowOffset);
        float accurateSlopeFH = -1;

        bool stopFlag = false;
        int index = 0;
        while (!stopFlag && index < baseAndFloorHeightArray.Count)
        {
            baseAndFloorheightPair currentBaseAndFloorheightPair = baseAndFloorHeightArray[index];
            if (currentBaseAndFloorheightPair.isSlope)
            {
                accurateSlopeFH = FloorheightFunctions.FindSlopeFloorh(transform, currentBaseAndFloorheightPair.rayHit.transform.parent.Find("top"));
            }

            if (thisObjHeight > currentBaseAndFloorheightPair.floorHeight && ((!currentBaseAndFloorheightPair.isSlope && shadowScript.floorHeight != currentBaseAndFloorheightPair.floorHeight) || (currentBaseAndFloorheightPair.isSlope && shadowScript.floorHeight != accurateSlopeFH)))
            {
                if (currentBaseAndFloorheightPair.isSlope && currentBaseAndFloorheightPair.rayHit.transform.parent != currentPlatformParent)
                {
                    if (thisObjHeight > accurateSlopeFH && shadowScript.floorHeight < accurateSlopeFH) {

                        parentTransform.GetComponent<FakeHeightObject>().Rise(accurateSlopeFH);
                    }
                } else
                {
                    parentTransform.GetComponent<FakeHeightObject>().Rise(currentBaseAndFloorheightPair.floorHeight);
                }
                stopFlag = true;
            }
            else if (index != baseAndFloorHeightArray.Count-1)
            {
                index++;
            } else
            {
                stopFlag = true;
            }
        }

        baseAndFloorheightPair chosenPlatform = baseAndFloorHeightArray[index];

        if (shadowScript.floorHeight > chosenPlatform.floorHeight && chosenPlatform.rayHit.transform.parent != currentPlatformParent)
        {
            if (chosenPlatform.isSlope && thisObjHeight > accurateSlopeFH && Mathf.Abs(shadowScript.floorHeight-accurateSlopeFH) > 2)
            {
                parentTransform.GetComponent<FakeHeightObject>().Drop(((Vector2)transform.position - prevGroundVel) * 30, shadowScript.floorHeight - accurateSlopeFH);
            } else if (!chosenPlatform.isSlope && thisObjHeight > chosenPlatform.floorHeight)
            {
               //parentTransform.GetComponent<FakeHeightObject>().Drop(((Vector2)transform.position - prevGroundVel) * 30, shadowScript.floorHeight - chosenPlatform.floorHeight);
            }

        }

        currentPlatformParent = chosenPlatform.rayHit.transform.parent;


        if (currentPlatformParent.gameObject.layer == LayerMask.NameToLayer("HorizontalSlope"))
        {
            onHorizontalSlope = true;
            onVerticalSlope = false;
            //SlopeCheck(currentPlatformParent.Find("top").gameObject, "horizontal");
        } else if (currentPlatformParent.gameObject.layer == LayerMask.NameToLayer("VerticalSlope"))
        {
            onHorizontalSlope = false;
            onVerticalSlope = true;
        } else
        {
            onHorizontalSlope = false;
            onVerticalSlope = false;
        }
        Debug.Log("PEDROLOG/4: currentPlayformParent = " + currentPlatformParent + ", onHorizontalSlope = " + onHorizontalSlope);
        return;
    }

    private void SlopeCheck(GameObject slope, string horizontalOrVertical)
    {
        newSlopeScript slopeScript = slope.GetComponent<newSlopeScript>();

        float floorMovementMultiplier = slopeScript.movementMultiplier;
        float slopeAngle = slopeScript.slopeAngle;
        float floorHeight = shadowScript.floorHeight;

        if (horizontalOrVertical == "horizontal" || wasPrevOnHorizontalSlope)
        {
            float slopeValue = slopeScript.horizontalFloorHeightThreshold;
            float slopeFloorH = FloorheightFunctions.FindSlopeFloorh(transform, slope.transform);
            if (onHorizontalSlope)
            {
                if ((this.transform.position.x != prevXVal) && (Mathf.Abs(slopeFloorH - floorHeight) < 2))
                {
                    totalAmountRisenOrSunk += 35 * (transform.position.x - prevXVal) * slopeValue * Time.deltaTime;
                    if (!onHorizontalSlope && wasPrevOnHorizontalSlope)
                    {
                        float[] twoPlatformHeights = new float[2] { slopeScript.h1, slopeScript.h2 };
                        float platformBeingSteppedOn = twoPlatformHeights.Aggregate((x, y) => Mathf.Abs(x - totalAmountRisenOrSunk) < Mathf.Abs(y - totalAmountRisenOrSunk) ? x : y);
                        if (parentTransform.GetComponent<FakeHeightObject>().isGrounded)
                        {
                            float changeAmount = (platformBeingSteppedOn - totalAmountRisenOrSunk) * floorMovementMultiplier;
                            parentTransform.position += Vector3.up * changeAmount;
                            shadowScript.floorHeight += changeAmount;
                        }
                        else
                        {
                            shadowTransform.position += Vector3.up * (platformBeingSteppedOn - totalAmountRisenOrSunk);
                        }
                    }
                    else
                    {
                        if (parentTransform.GetComponent<FakeHeightObject>().isGrounded)
                        {
                            float changeAmount = 35 * (transform.position.x - prevXVal) * slopeValue * Time.deltaTime;
                            parentTransform.position += Vector3.up * changeAmount;
                            shadowScript.floorHeight += changeAmount;
                        }
                        else
                        {
                            float changeAmount = 35 * (transform.position.x - prevXVal) * slopeValue * Time.deltaTime;
                            shadowTransform.position += Vector3.up * changeAmount;
                            shadowScript.floorHeight += changeAmount;
                        }
                    }
                }
            }
        }

        wasPrevOnHorizontalSlope = onHorizontalSlope;

    }

    //Updates every frame, checks the appropriate floor height of the object.

    public void checkFloorHeight(RaycastHit2D[] baseRayHits)
    {
        float slopeFH = -999;
        float nearestFloorHeight = -99999;

        bool highestFHisSlope = false;

        foreach (RaycastHit2D baseHit in baseRayHits)
        {
            Transform platformParent = baseHit.transform.parent;
            Transform platformTop = platformParent.Find("top");
            if (platformParent.gameObject.layer == LayerMask.NameToLayer("HorizontalSlope") || platformParent.gameObject.layer == LayerMask.NameToLayer("VerticalSlope"))
            {
                //Goal here: Find the greatest point in the slope that's less than the current object's floor height.
                slopeFH = FloorheightFunctions.FindSlopeFloorh(transform, platformTop);
                if (slopeFH <= shadowScript.floorHeight + 2)
                {
                    if (slopeFH > nearestFloorHeight)
                    {
                        nearestFloorHeight = slopeFH;
                        highestFHisSlope = true;
                    }
                }
            }
            //Goal here: Find the greatest floor height that is still less than the current object's floor height.
            else if (platformTop.GetComponent<platformScript>().floorHeight <= shadowScript.floorHeight + 2)
            {
                if (platformTop.GetComponent<platformScript>().floorHeight > nearestFloorHeight)
                {
                    nearestFloorHeight = platformTop.GetComponent<platformScript>().floorHeight;
                }
            }
        }

        //Goal here: Calculate the appropriate floor height.
        if ((!onVerticalSlope) && (!onHorizontalSlope) && (Mathf.Abs(shadowScript.floorHeight - nearestFloorHeight) < 1))
        {
            shadowScript.floorHeight = nearestFloorHeight;
        }
        else
        {
            if (highestFHisSlope)
            {
               // shadowScript.floorHeight = slopeFH;
            }
            else
            {
                if (shadowScript.floorHeight > nearestFloorHeight)
                {
                    if (Mathf.Abs(shadowScript.floorHeight - nearestFloorHeight) < 1)
                    {
                        shadowScript.floorHeight = nearestFloorHeight;
                    }
                    else
                    {
                        parentTransform.GetComponent<FakeHeightObject>().Drop(((Vector2)transform.position - prevGroundVel) * 30, shadowScript.floorHeight - nearestFloorHeight);
                    }
                }
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "LandTarget")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
        }
    }

}
