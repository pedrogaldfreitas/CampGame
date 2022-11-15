using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour
{
    //This is the "clearance" height of the object (if other object is above this height, it ignores collisions with it. SET AUTOMATICALLY VIA Start()
    public float solidHeight;

    //This is the solid's height from its base. An object can ignore collisions by passing "under" this solid.
    public float solidHeightFromBase;

    public List<Collider2D> collidersTouchingSlope;
    public List<Collider2D> collidersBeingIgnored;

    private void Start()
    {
        collidersTouchingSlope = new List<Collider2D>();
        collidersBeingIgnored = new List<Collider2D>();
        solidHeight = transform.parent.Find("top").GetComponent<platformScript>().floorHeight;
        solidHeightFromBase = transform.parent.Find("solid").position.y - transform.position.y;
    }

    private IEnumerator IgnoreCollisions(Collider2D otherCollider)
    {
        collidersBeingIgnored.Add(otherCollider);
       // Collider2D solidCollider = transform.parent.Find("solid").GetComponent<Collider2D>();
        Collider2D baseCollider = GetComponent<Collider2D>();

        newShadowScript shadow = otherCollider.transform.parent.Find("Shadow").GetComponent<newShadowScript>();
        FakeHeightObject fakeHeightObj = otherCollider.transform.parent.GetComponent<FakeHeightObject>();

        bool aboveWall = shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset > solidHeight;
        bool belowWall = solidHeightFromBase > fakeHeightObj.heightOfObject + shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset;

        Physics2D.IgnoreCollision(baseCollider, otherCollider, true);

        while (aboveWall || belowWall || Physics2D.Distance(baseCollider, otherCollider).isOverlapped || collidersTouchingSlope.Contains(otherCollider))
        {
            aboveWall = shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset > solidHeight;
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

            bool aboveWall = shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset > solidHeight;
            bool belowWall = solidHeightFromBase > fakeHeightObj.heightOfObject + shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset;

            if (this.transform.parent.name == "stairBlock (1)")
            {
                Debug.Log("PEDROLOG: aboveWall = " + aboveWall + ", belowWall = " + belowWall + ", while colliding with LandTarget.");
            }
            if (aboveWall || belowWall)
            {
                StartCoroutine(IgnoreCollisions(collision.GetComponent<Collider2D>()));
            }
        }

    }
}
