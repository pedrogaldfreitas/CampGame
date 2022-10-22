using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class solidScript : MonoBehaviour
{
    //This is the "clearance" height of the object (if other object is above this height, it ignores collisions with it. SET AUTOMATICALLY VIA Start()
    public float solidHeight;

    public List<Collider2D> collidersTouchingSlope;
    
    private void Start()
    {
        collidersTouchingSlope = new List<Collider2D>();
        solidHeight = transform.parent.Find("top").GetComponent<platformScript>().floorHeight;
    }

    private IEnumerator IgnoreCollisions(Collider2D otherCollider)
    {
        Collider2D solidCollider = this.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(solidCollider, otherCollider, true);
        if (otherCollider.name == "Shadow" && this.transform.parent.name == "platformFloor2")
        {
            string hi = "";
            for (int i = 0; i < collidersTouchingSlope.Count; i++)
            {
                hi = hi + collidersTouchingSlope[i].name + " ";
            }
            Debug.Log("PEDROLOG: collidersTouchingSlope = " + hi + ". Does this contain the Shadow collider? -> " + collidersTouchingSlope.Contains(otherCollider));
        }
        while (Physics2D.Distance(solidCollider, otherCollider).isOverlapped || collidersTouchingSlope.Contains(otherCollider)) {
            if (otherCollider.name == "Shadow" && this.transform.parent.name == "platformFloor2")
            {
                Debug.Log("PEDROLOG: This should be repeating for " + otherCollider.name);
            }
            yield return null; 
        }
        if (otherCollider.name == "Shadow" && this.transform.parent.name == "platformFloor2")
        {
            Debug.Log("PEDROLOG: no longer ignoring collisions between Shadow and " + this.transform.name);
        }
        Physics2D.IgnoreCollision(solidCollider, otherCollider, false);
    }

    //Use this function externally when an object is to be ignored by platforms' "solid" area as it is currently on their slope.
    public void ToggleIgnoreObjectOnSlope(Collider2D otherCollider, bool ignoreOrNot)
    {
        if (ignoreOrNot) {
            Debug.Log("PEDROLOG#1 - " + collidersTouchingSlope.Count);
            collidersTouchingSlope.Add(otherCollider);
            Debug.Log("PEDROLOG: ToggleIgnoreObjectOnSlope ADDS the slope from the array.");
            StartCoroutine(IgnoreCollisions(otherCollider));
        } else {
            Debug.Log("PEDROLOG: ToggleIgnoreObjectOnSlope REMOVES the slope from the array.");
            collidersTouchingSlope.Remove(otherCollider);
        }
        return;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.name == "Shadow" || collision.transform.name.StartsWith("RaccoonParent"))
        {
            newShadowScript shadow = collision.GetComponent<newShadowScript>();
            FakeHeightObject fakeHeightObj = collision.transform.parent.GetComponent<FakeHeightObject>();
            if (shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset > solidHeight)
            {
                StartCoroutine(IgnoreCollisions(collision.GetComponent<Collider2D>()));
            }
        }
        
    }
}
