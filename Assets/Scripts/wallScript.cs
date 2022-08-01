using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallScript : MonoBehaviour
{

    public GameObject upperPlatform;
    private Transform objectToIgnore;

    // Update is called once per frame
    void Update()
    {
        if (objectToIgnore != null)
        {
            if (objectToIgnore.GetComponentInChildren<newShadowScript>().floorHeight < upperPlatform.GetComponent<platformScript>().floorHeight)
            {
                //ASSUMING objectToIgnore BELONGS TO PLAYER WITH PlayerLegs OBJECT
                Physics2D.IgnoreCollision(objectToIgnore.GetChild(0).GetChild(6).GetComponents<BoxCollider2D>()[1], this.gameObject.GetComponent<BoxCollider2D>(), false);
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("pappapia");
        if (collision.gameObject.GetComponentInChildren<newShadowScript>().floorHeight >= upperPlatform.GetComponent<platformScript>().floorHeight)
        {
            objectToIgnore = collision.transform;
            //FIRST PARAMETER EXPLANATION: Gets child with index 6 ("PlayerLegs") and takes the boxcollider2d component that is NOT a trigger (index 1). 
            Physics2D.IgnoreCollision(collision.transform.GetChild(0).GetChild(6).GetComponents<BoxCollider2D>()[1], this.gameObject.GetComponent<BoxCollider2D>(), true);
            
        }
    }

}
