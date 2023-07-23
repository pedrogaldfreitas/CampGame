using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour
{
    //This is the "clearance" height of the object (if other object is above this height, it ignores collisions with it. SET AUTOMATICALLY VIA Start()
    private float solidHeight;

    //Is this platform a slope?
    [SerializeField]
    private bool isSlope = false;

    //This is the solid's height from its base. An object can ignore collisions by passing "under" this solid.
    private float solidHeightFromBase;

    [Header("Setup Variables")]
    [SerializeField]
    [Tooltip("If the object is sitting on a platform (not hovering), drag the platform here to set the solidHeightFromBase variable.")]
    private Transform startingPlatform;

    [Header("Test Variables")]
    public List<Collider2D> collidersTouchingSlope;
    public List<Collider2D> collidersBeingIgnored;

    private void Start()
    {
        collidersTouchingSlope = new List<Collider2D>();
        collidersBeingIgnored = new List<Collider2D>();
        solidHeight = transform.parent.Find("top").GetComponent<platformScript>().floorHeight;
        solidHeightFromBase = transform.parent.Find("solid").position.y - transform.position.y;

        if (startingPlatform)
        {
            solidHeightFromBase = startingPlatform.Find("top").GetComponent<platformScript>().floorHeight;
            this.transform.position = new Vector2(transform.position.x, transform.position.y - solidHeightFromBase);
        }
    }

    private IEnumerator IgnoreCollisions(Collider2D otherCollider)
    {
        collidersBeingIgnored.Add(otherCollider);
        Collider2D baseCollider = GetComponent<Collider2D>();

        newShadowScript shadow = otherCollider.transform.parent.Find("Shadow").GetComponent<newShadowScript>();
        FakeHeightObject fakeHeightObj = otherCollider.transform.parent.GetComponent<FakeHeightObject>();

        bool aboveWall = shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset >= solidHeight;
        bool belowWall = solidHeightFromBase > fakeHeightObj.heightOfObject + shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset;

        Physics2D.IgnoreCollision(baseCollider, otherCollider, true);
        while (aboveWall || belowWall || Physics2D.Distance(baseCollider, otherCollider).isOverlapped || collidersTouchingSlope.Contains(otherCollider))
        {
            aboveWall = shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset >= solidHeight;
            belowWall = solidHeightFromBase > fakeHeightObj.heightOfObject + shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset;

            yield return null;
        }
        collidersBeingIgnored.Remove(otherCollider);
        Physics2D.IgnoreCollision(baseCollider, otherCollider, false);
    }

    //Use this function externally when an object is to be ignored by platforms' "solid" area as it is currently on their slope.
    public void ToggleIgnoreObjectOnSlope(Collider2D otherCollider, bool ignoreOrNot)
    {
        if (ignoreOrNot)
        {
            collidersTouchingSlope.Add(otherCollider);
            StartCoroutine(IgnoreCollisions(otherCollider));
        }
        else
        {
            collidersTouchingSlope.Remove(otherCollider);
        }
        return;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.name == "LandTarget" && !collidersBeingIgnored.Contains(collision.GetComponent<Collider2D>()))
        {      
            newShadowScript shadow = collision.transform.parent.Find("Shadow").GetComponent<newShadowScript>();
            FakeHeightObject fakeHeightObj = collision.transform.parent.GetComponent<FakeHeightObject>();

            bool aboveWall = false;

            if (isSlope)
            {
                //For HORIZONTAL SLOPES ONLY now (vertical will not work unless updated) (Maybe?)
                newSlopeScript slopeScript = transform.parent.Find("top").GetComponent<newSlopeScript>();
                float slopeHeightAtCollisionPoint = FloorheightFunctions.FindSlopeFloorh(shadow.transform, slopeScript.transform);
                float lowestPlatformFH = System.Math.Min(slopeScript.h1, slopeScript.h2);
                aboveWall = shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset >= lowestPlatformFH + slopeHeightAtCollisionPoint;
            } else
            {
                aboveWall = shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset >= solidHeight;
            }

            bool belowWall = solidHeightFromBase > fakeHeightObj.heightOfObject + shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset;

            if (aboveWall || belowWall)
            {
                StartCoroutine(IgnoreCollisions(collision.GetComponent<Collider2D>()));
            }
        }

    }
}
