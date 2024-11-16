using System;
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
    private BoxCollider2D boxCollider;
    public List<Collider2D> baseCollisions;
    private ContactFilter2D baseCollisionCF;
    public LayerMask baseCollisionLayerMask;

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

        boxCollider = GetComponent<BoxCollider2D>();
        baseCollisions = new List<Collider2D>();
        baseCollisionCF = new ContactFilter2D();
        baseCollisionCF.SetLayerMask(baseCollisionLayerMask);
    }

    private void Update()
    {
        this.transform.position = new Vector2(transform.position.x, shadowScript.transform.position.y - shadowScript.floorHeight);
        baseCheckRay = Physics2D.RaycastAll(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down, 0f, (1 << 17));

        baseCollisions.Clear();
        boxCollider.OverlapCollider(baseCollisionCF, baseCollisions);

        //Debug.DrawRay(transform.position + Vector3.down * raycastDistanceMultiplier, Vector2.down * 0.3f, Color.blue);

        //Here: Determine the most accurate floorheight to set, and if it is of a horizontal or vertical slope.
        if (baseCollisions.Count > 0)
        {
            baseDetect(baseCollisions);
        }
        checkFloorHeight(baseCollisions);

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
    class baseAndFloorheightPair {
        public float floorHeight;
        public Collider2D colliderHit;
        public bool isSlope;
        public baseAndFloorheightPair(float fh, Collider2D colHit, bool slope)
        {
            floorHeight = fh;
            colliderHit = colHit;
            isSlope = slope;
        }
    }

    public void baseDetect(List<Collider2D> collidersHit)
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
        foreach (Collider2D colliderHit in collidersHit)
        {
            int baseObjLayer = colliderHit.transform.parent.gameObject.layer;
            if (baseObjLayer == LayerMask.NameToLayer("HorizontalSlope") || baseObjLayer == LayerMask.NameToLayer("VerticalSlope"))
            {
                newSlopeScript slopeScript = colliderHit.transform.parent.Find("top").GetComponent<newSlopeScript>();
                float lowestPlatformFH = Math.Min(slopeScript.h1, slopeScript.h2);
                baseAndFloorHeightArray.Add(new baseAndFloorheightPair(lowestPlatformFH, colliderHit, true));
            }
            else
            {
                baseAndFloorHeightArray.Add(new baseAndFloorheightPair(colliderHit.transform.parent.Find("top").GetComponent<platformScript>().floorHeight, colliderHit, false));
            }
        }

        //Order the list of ALL colliders detected by in ascending order of floorHeight.
        baseAndFloorHeightArray = baseAndFloorHeightArray.OrderBy(o => o.floorHeight).ToList();

        FakeHeightObject thisObjHeightInfo = parentTransform.GetComponent<FakeHeightObject>();
        float thisObjHeight = shadowScript.floorHeight + (thisObjHeightInfo.height + thisObjHeightInfo.shadowOffset);
        float accurateSlopeFH = -1;

        bool stopFlag = false;
        int index = 0;
        int highestReachablePlatformIndex = 0;

        //NEW ATTEMPT START
        while (!stopFlag && index < baseAndFloorHeightArray.Count)
        {
            baseAndFloorheightPair currentBaseAndFloorheightPair = baseAndFloorHeightArray[index];
            float platformFH;
            if (currentBaseAndFloorheightPair.isSlope)
            {
                accurateSlopeFH = FloorheightFunctions.FindSlopeFloorh(transform, currentBaseAndFloorheightPair.colliderHit.transform.parent.Find("top"));
                platformFH = accurateSlopeFH;
            } else
            {
                platformFH = currentBaseAndFloorheightPair.floorHeight;
            }

     
            bool objCanReachPlatform = thisObjHeight > platformFH;
            bool objFloorheightNotSameAsCurrentBaseFloorheight = (shadowScript.floorHeight != platformFH);
            //bool objFloorheightNotSameAsCurrentBaseFloorheight = (!currentBaseAndFloorheightPair.isSlope && shadowScript.floorHeight != currentBaseAndFloorheightPair.floorHeight) || (currentBaseAndFloorheightPair.isSlope && shadowScript.floorHeight != accurateSlopeFH);

            if (currentBaseAndFloorheightPair.isSlope)
            {
                Debug.Log("PEDROLOG/1: can reach slope: " + objCanReachPlatform + ", is this a new height?: " + objFloorheightNotSameAsCurrentBaseFloorheight);
            }

            if (objCanReachPlatform)
            {

                if (objFloorheightNotSameAsCurrentBaseFloorheight)
                {
                    //If it seems like the player has reached a higher platform than before, raise the shadow to the new platform.
                    if (currentBaseAndFloorheightPair.isSlope && currentBaseAndFloorheightPair.colliderHit.transform.parent != currentPlatformParent)
                    {
                        if (thisObjHeight > accurateSlopeFH && shadowScript.floorHeight < accurateSlopeFH)
                        {
                            parentTransform.GetComponent<FakeHeightObject>().Rise(accurateSlopeFH);
                        }
                    }
                    else
                    {
                        parentTransform.GetComponent<FakeHeightObject>().Rise(currentBaseAndFloorheightPair.floorHeight);
                    }
                }

                if (platformFH > baseAndFloorHeightArray[highestReachablePlatformIndex].floorHeight)
                {
                    highestReachablePlatformIndex = index;
                }

                index++;
            } else
            {
                stopFlag = true;
            }
        }

        baseAndFloorheightPair chosenPlatform = baseAndFloorHeightArray[highestReachablePlatformIndex];
        //baseAndFloorheightPair chosenPlatform = stopFlag ? baseAndFloorHeightArray[index] : baseAndFloorHeightArray[index-1];

        Debug.Log("PEDROLOG/FINAL: chosenPlatform = " + chosenPlatform.colliderHit.transform.parent.name);

        //I edited code from above this point. The rest can be cleaned up, because it looks like the Drop() calls never run here, they run in checkFloorHeight(...) instead.

        if (shadowScript.floorHeight > chosenPlatform.floorHeight && chosenPlatform.colliderHit.transform.parent != currentPlatformParent)
        {
            if (chosenPlatform.isSlope && thisObjHeight > accurateSlopeFH && Mathf.Abs(shadowScript.floorHeight - accurateSlopeFH) > 2)
            {
                Debug.Log("PEDROLOG: This runs.");  //NOTE: This debug log never actually runs. Look into why this code is here.
                parentTransform.GetComponent<FakeHeightObject>().Drop(((Vector2)transform.position - prevGroundVel) * 30, shadowScript.floorHeight - accurateSlopeFH);
            }
            else if (!chosenPlatform.isSlope && thisObjHeight > chosenPlatform.floorHeight)
            {
                parentTransform.GetComponent<FakeHeightObject>().Drop(((Vector2)transform.position - prevGroundVel) * 30, shadowScript.floorHeight - chosenPlatform.floorHeight);
            }

        }

        currentPlatformParent = chosenPlatform.colliderHit.transform.parent;

        //ISSUE: currentPlatformParent is not being set to the slope.
        Debug.Log("PEDROLOG:currentPlatformParent = " + currentPlatformParent.name);
    
        if (currentPlatformParent.gameObject.layer == LayerMask.NameToLayer("HorizontalSlope"))
        {
            onHorizontalSlope = true;
            onVerticalSlope = false;
            //SlopeCheck(currentPlatformParent.Find("top").gameObject, "horizontal");
        }
        else if (currentPlatformParent.gameObject.layer == LayerMask.NameToLayer("VerticalSlope"))
        {
            onHorizontalSlope = false;
            onVerticalSlope = true;
        }
        else
        {
            onHorizontalSlope = false;
            onVerticalSlope = false;
        }
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

    // Updates every frame, checks the appropriate floor height of the object.
    public void checkFloorHeight(List<Collider2D> colliderHits)
    {
        float slopeFH = -999;
        float nearestFloorHeight = -99999;

        bool highestFHisSlope = false;


        foreach (Collider2D colliderHit in colliderHits)
        {
            Transform platformParent = colliderHit.transform.parent;
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
            //Goal here: Find the greatest floor height that is still less than (or equal to) the current object's floor height.
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
