using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class solidScript : MonoBehaviour
{
    //This is the "clearance" height of the object (if other object is above this height, it ignores collisions with it. SET AUTOMATICALLY VIA Start()
    public float solidHeight;

    public List<Collision2D> ignoredCollisions;
    private int ignoredCollisionsCount;
    private bool coroutineRunning;

    public Collision2D[] onCollisionEnterBuffer;
    private int collisionBuffer;

    // Start is called before the first frame update
    void Start()
    {
        ignoredCollisions = new List<Collision2D>();
        onCollisionEnterBuffer = new Collision2D[20];
        //Debug.Log("Length of ignoredCollisions: " + ignoredCollisions.Length);
        coroutineRunning = false;
        collisionBuffer = 0;
    }

    private IEnumerator ignoredCollidersCheck()
    {
        while (ignoredCollisions.Count > 0)
        {
            for (int i = 0; i < ignoredCollisions.Count; i++)
            {
                newShadowScript shadow = ignoredCollisions[i].transform.Find("Shadow").GetComponent<newShadowScript>();
                FakeHeightObject fakeHeightObj = ignoredCollisions[i].transform.GetComponent<FakeHeightObject>();
                //Debug.Log((shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset < solidHeight));

                if (shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset < solidHeight)
                {
                    Physics2D.IgnoreCollision(this.GetComponent<PolygonCollider2D>(), ignoredCollisions[i].transform.Find("Shadow").GetComponent<Collider2D>(), false);
                    ignoredCollisions.RemoveAt(i);
                    i--;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
        coroutineRunning = false;
    }

    //Take any shadow collided with this object and put them in the IgnoredCollisions array if the floorHeight is above the solid's.
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.name == "PlayerParent")
        {
            newShadowScript shadow = collision.transform.Find("Shadow").GetComponent<newShadowScript>();
            FakeHeightObject fakeHeightObj = collision.transform.GetComponent<FakeHeightObject>();
            Debug.Log("Shadow FH = " + shadow.floorHeight + ", Player Height w/ shadow offset = " + (fakeHeightObj.height + fakeHeightObj.shadowOffset) + ", Solid Base Height = " + solidHeight);

            if (shadow.floorHeight + fakeHeightObj.height + fakeHeightObj.shadowOffset > solidHeight)
            {
                Physics2D.IgnoreCollision(this.GetComponent<PolygonCollider2D>(), collision.transform.Find("Shadow").GetComponent<Collider2D>(), true);
                ignoredCollisions.Add(collision);
                Debug.Log("Now ignoring collisions with " + collision.transform.name);
            }

            if (!coroutineRunning)
            {
                coroutineRunning = true;
                StartCoroutine(ignoredCollidersCheck());
            }
        }
    }

}
