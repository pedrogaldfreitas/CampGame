using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class solidScript : MonoBehaviour
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
        solidHeight = transform.parent.Find("top").GetComponent<platformScript>().floorHeight;
        solidHeightFromBase = transform.position.y - transform.parent.Find("base").position.y;
    }

    private IEnumerator IgnoreCollisions(Collider2D otherCollider)
    {
        collidersBeingIgnored.Add(otherCollider);
        Collider2D solidCollider = this.GetComponent<Collider2D>();
        Collider2D baseCollider = transform.parent.Find("base").GetComponent<Collider2D>();

        newShadowScript shadow = otherCollider.transform.parent.Find("Shadow").GetComponent<newShadowScript>();
        FakeHeightObject fakeHeightObj = otherCollider.transform.parent.GetComponent<FakeHeightObject>();

        bool aboveWall = shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset > solidHeight;
        bool belowWall = solidHeightFromBase > fakeHeightObj.heightOfObject + shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset;

        Physics2D.IgnoreCollision(solidCollider, otherCollider, true);
        if (transform.parent.name == "stairBlock" && otherCollider.name == "LandTarget")
        {
        }
        while (aboveWall || belowWall || Physics2D.Distance(baseCollider, otherCollider).isOverlapped || collidersTouchingSlope.Contains(otherCollider)) {
            aboveWall = shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset > solidHeight;
            belowWall = solidHeightFromBase > fakeHeightObj.heightOfObject + shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset;
            yield return null; 
        }

        collidersBeingIgnored.Remove(otherCollider);
        Physics2D.IgnoreCollision(solidCollider, otherCollider, false);
    }

    //Use this function externally when an object is to be ignored by platforms' "solid" area as it is currently on their slope.
    public void ToggleIgnoreObjectOnSlope(Collider2D otherCollider, bool ignoreOrNot)
    {
        if (ignoreOrNot) {
            collidersTouchingSlope.Add(otherCollider);
            StartCoroutine(IgnoreCollisions(otherCollider));
        } else {
            collidersTouchingSlope.Remove(otherCollider);
        }
        return;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.name == "LandTarget"/* || collision.transform.name.StartsWith("RaccoonParent"))*/ && !collidersBeingIgnored.Contains(collision.GetComponent<Collider2D>()))
        {
            newShadowScript shadow = collision.transform.parent.Find("Shadow").GetComponent<newShadowScript>();
            FakeHeightObject fakeHeightObj = collision.transform.parent.GetComponent<FakeHeightObject>();

            //NOTE: Make sure we check if the collider already exists in the array before adding it in again. (Or not if the array isn't adding it again)
            bool aboveWall = shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset > solidHeight;
            bool belowWall = solidHeightFromBase > fakeHeightObj.heightOfObject + shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset;

            if (aboveWall || belowWall)
            {
                StartCoroutine(IgnoreCollisions(collision.GetComponent<Collider2D>()));
            }
        }
        
    }
}
