using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class solidScript : MonoBehaviour
{
    //This is the "clearance" height of the object (if other object is above this height, it ignores collisions with it. SET AUTOMATICALLY VIA Start()
    public float solidHeight;
    
    private void Start()
    {
        solidHeight = transform.parent.Find("top").GetComponent<platformScript>().floorHeight;
    }

    private IEnumerator IgnoreCollisions(Collider2D collider1, Collider2D collider2)
    {
        Physics2D.IgnoreCollision(collider1, collider2, true);
        while (Physics2D.Distance(collider1, collider2).isOverlapped) { yield return null; }
        Physics2D.IgnoreCollision(collider1, collider2, false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.name == "PlayerParent" || collision.transform.name.StartsWith("RaccoonParent"))
        {
            newShadowScript shadow = collision.transform.Find("Shadow").GetComponent<newShadowScript>();
            FakeHeightObject fakeHeightObj = collision.transform.GetComponent<FakeHeightObject>();
            if (shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset > solidHeight)
            {
                StartCoroutine(IgnoreCollisions(this.GetComponent<Collider2D>(), collision.collider));
            }
        }
    }

}
