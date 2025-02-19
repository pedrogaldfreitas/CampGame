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
    private FakeHeightObject fakeHeightObject;

    //private float floorHeight;
    private Transform currentPlatformParent;

    RaycastHit2D[] verticalSlopeRay;
    [SerializeField]
    RaycastHit2D[] horizontalSlopeRay;
    Transform chosenHorizontalSlope;
    Transform chosenVerticalSlope;
    private BoxCollider2D boxCollider;
    public List<Collider2D> baseCollisions;
    private ContactFilter2D baseCollisionCF;
    public LayerMask baseCollisionLayerMask;
    private float boxColliderHeight;
    private float boxColliderWidth;

    [Range(0f, 3f)] public float raycastDistanceMultiplier;
    private float prevXVal;
    private float prevYVal;
    private Vector2 prevGroundVel;
    private float totalAmountRisenOrSunk;

    public bool onHorizontalSlope;
    public bool onVerticalSlope;

    private void Start()
    {
        totalAmountRisenOrSunk = 0;

        parentTransform = transform.parent;
        shadowTransform = parentTransform.Find("Shadow");
        shadowScript = shadowTransform.GetComponent<newShadowScript>();
        fakeHeightObject = parentTransform.GetComponent<FakeHeightObject>();

        boxCollider = GetComponent<BoxCollider2D>();
        baseCollisions = new List<Collider2D>();
        baseCollisionCF = new ContactFilter2D();
        baseCollisionCF.SetLayerMask(baseCollisionLayerMask);
        boxColliderHeight = boxCollider.bounds.extents.y * 2;
        boxColliderWidth = boxCollider.bounds.extents.x * 2;
    }

    private void Update()
    {
        this.transform.position = new Vector2(transform.position.x, shadowScript.transform.position.y - shadowScript.floorHeight);

        //Setup the ray that will scan for slopes.
        horizontalSlopeRay = Physics2D.RaycastAll(transform.position + Vector3.up * boxColliderHeight / 2, Vector2.down, boxColliderHeight, (1 << 17));
        verticalSlopeRay = Physics2D.RaycastAll(transform.position + Vector3.left * boxColliderWidth / 2, Vector2.right, boxColliderWidth, (1 << 17));
        //Filter only slopes in horizontalSlopeRay and verticalSlopeRay.
        horizontalSlopeRay = horizontalSlopeRay.Where(baseObj => baseObj.transform.parent.gameObject.layer == LayerMask.NameToLayer("HorizontalSlope")).ToArray();
        verticalSlopeRay = verticalSlopeRay.Where(baseObj => baseObj.transform.parent.gameObject.layer == LayerMask.NameToLayer("VerticalSlope")).ToArray();
        
        Debug.DrawRay(transform.position + Vector3.up * boxColliderHeight/2, Vector2.down * boxColliderHeight, Color.blue);
        Debug.DrawRay(transform.position + Vector3.left * boxColliderWidth / 2, Vector2.right * boxColliderWidth, Color.red);


        baseCollisions.Clear();
        boxCollider.OverlapCollider(baseCollisionCF, baseCollisions);

        //Here: Determine the most accurate floorheight to set, and if it is of a horizontal or vertical slope.
        if (baseCollisions.Count > 0)
        {
            baseDetect(baseCollisions);
        }
        checkFloorHeight(baseCollisions);


        //TEST: Delete this for sure.
        //shadowTransform.position += Vector3.up * 5 * Time.deltaTime;

        if (onHorizontalSlope)
        {
            SlopeCheck(chosenHorizontalSlope.Find("top").gameObject, "horizontal");
        } else if (onVerticalSlope)
        {
            SlopeCheck(chosenVerticalSlope.Find("top").gameObject, "vertical");
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

        float thisObjHeight = shadowScript.floorHeight + (fakeHeightObject.height + fakeHeightObject.shadowOffset);
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

            if (objCanReachPlatform)
            {
                bool prioritizeSlope = currentBaseAndFloorheightPair.floorHeight == baseAndFloorHeightArray[highestReachablePlatformIndex].floorHeight && currentBaseAndFloorheightPair.isSlope;

                if (platformFH > baseAndFloorHeightArray[highestReachablePlatformIndex].floorHeight || prioritizeSlope)
                {
                    highestReachablePlatformIndex = index;

                    bool platformIsNotAlreadyCurrentPlatform = baseAndFloorHeightArray[highestReachablePlatformIndex].colliderHit.transform != currentPlatformParent;
                    if (platformIsNotAlreadyCurrentPlatform)
                    {
                        if (currentBaseAndFloorheightPair.isSlope)
                        {
                            if (thisObjHeight > accurateSlopeFH && shadowScript.floorHeight < accurateSlopeFH)
                            {
                                fakeHeightObject.Rise(accurateSlopeFH);
                            }
                        }
                        else
                        {
                            fakeHeightObject.Rise(currentBaseAndFloorheightPair.floorHeight);
                        }
                    }
                }

                index++;
            } else
            {
                stopFlag = true;
            }
        }

        baseAndFloorheightPair chosenPlatform = baseAndFloorHeightArray[highestReachablePlatformIndex];


        RaycastHit2D detectedHorizontalSlope = default;
        RaycastHit2D detectedVerticalSlope = default;

        //Set onHorizontalSlope or onVerticalSlope based on the current platform detected + the slope raycasts.
        if (horizontalSlopeRay.Length > 0)
        {
            detectedHorizontalSlope = horizontalSlopeRay.Where(slopeBase => {
                newSlopeScript slopeScript = slopeBase.transform.parent.Find("top").GetComponent<newSlopeScript>();
                return chosenPlatform.colliderHit.transform == slopeScript.platform1.transform || slopeScript.platform2.transform || slopeScript.transform;
            }).ToArray()[0];
        }

        if (verticalSlopeRay.Length > 0)
        {
            detectedVerticalSlope = verticalSlopeRay.Where(slopeBase =>
            {
                newSlopeScript slopeScript = slopeBase.transform.parent.Find("top").GetComponent<newSlopeScript>();
                return chosenPlatform.colliderHit.transform == slopeScript.platform1.transform || slopeScript.platform2.transform || slopeScript.transform;
            }).ToArray()[0];
        }

        if (detectedHorizontalSlope)
        {
            chosenPlatform = baseAndFloorHeightArray.Where(item => item.colliderHit == detectedHorizontalSlope.transform.GetComponent<Collider2D>()).First();
            onHorizontalSlope = true;
            onVerticalSlope = false;
            chosenHorizontalSlope = detectedHorizontalSlope.transform.parent;
            chosenVerticalSlope = default;
        }
        else if (detectedVerticalSlope)
        {
            chosenPlatform = baseAndFloorHeightArray.Where(item => item.colliderHit == detectedVerticalSlope.transform.GetComponent<Collider2D>()).First();
            onHorizontalSlope = false;
            onVerticalSlope = true;
            chosenHorizontalSlope = default;
            chosenVerticalSlope = detectedVerticalSlope.transform.parent;
        }
        else
        {
            onHorizontalSlope = false;
            onVerticalSlope = false;
            chosenHorizontalSlope = default;
            chosenVerticalSlope = default;
        }

        //I edited code from above this point. The rest can be cleaned up, because it looks like the Drop() calls never run here, they run in checkFloorHeight(...) instead.

        if (shadowScript.floorHeight > chosenPlatform.floorHeight && chosenPlatform.colliderHit.transform.parent != currentPlatformParent)
        {
            if (chosenPlatform.isSlope && thisObjHeight > accurateSlopeFH && Mathf.Abs(shadowScript.floorHeight - accurateSlopeFH) > 2)
            {
                //NOTE: This debug log never actually runs. Look into why this code is here.
                fakeHeightObject.Drop(((Vector2)transform.position - prevGroundVel) * 30, shadowScript.floorHeight - accurateSlopeFH);
            }
            else if (!chosenPlatform.isSlope && thisObjHeight > chosenPlatform.floorHeight)
            {
                Debug.Log("PEDROLOG#1: chosenPlatform name = " + chosenPlatform.colliderHit.transform.parent.name);
                fakeHeightObject.Drop(((Vector2)transform.position - prevGroundVel) * 30, shadowScript.floorHeight - chosenPlatform.floorHeight);
            }

        }

        currentPlatformParent = chosenPlatform.colliderHit.transform.parent;
        return;
    }

    private void SlopeCheck(GameObject slope, string horizontalOrVertical)
    {
        float floorHeight = shadowScript.floorHeight;
        float slopeFloorH = FloorheightFunctions.FindSlopeFloorh(transform, slope.transform);

        if (horizontalOrVertical == "horizontal")
        {
            if (this.transform.position.x != prevXVal)
            {
                AdjustFloorHeightFromSlope(slopeFloorH, floorHeight);
            }
        }
        if (horizontalOrVertical == "vertical")
        {
            if (this.transform.position.y != prevYVal)  //CHECK IF PREVYVAL IS PROPERLY SET WHEREVER IT IS 
            {
                AdjustFloorHeightFromSlope(slopeFloorH, floorHeight);
            }
        }
    }

    //Only runs when the player's movement on a slope should change their floorHeight.
    private void AdjustFloorHeightFromSlope(float slopeFloorH, float floorHeight)
    {
        float changeAmount = slopeFloorH - floorHeight;
        shadowScript.floorHeight += changeAmount;

        if (fakeHeightObject.isGrounded)
        {
            parentTransform.position += Vector3.up * changeAmount;
        }
        else
        {
            shadowTransform.position += Vector3.up * changeAmount;
        }
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
                        fakeHeightObject.Drop(((Vector2)transform.position - prevGroundVel) * 30, shadowScript.floorHeight - nearestFloorHeight);
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
