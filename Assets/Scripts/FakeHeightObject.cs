using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeHeightObject : MonoBehaviour
{
    public Transform transObject;
    public Transform transBody;
    public Transform transShadow;
    public float height;
    public float heightOfObject;    //This is the height of the object from top to bottom, NOT height from the ground. This means the main object MUST have a trigger collider to measure from.

    public float gravity;
    public Vector2 prevGroundVelocity;
    public Vector2 groundVelocity;
    public float verticalVelocity;
    public Vector3 airTimeRotation;

    public bool isGrounded;
    public float shadowOffset;

    public Animator baseObjAnimator;
    private GameObject ignoredSolidPlatformObj;

    private Quaternion landingRotation;

    private void Start()
    {
        height = (float)Decimal.Round((Decimal)(transBody.position.y - transShadow.position.y), 3);
        prevGroundVelocity = new Vector2(0, 0);
        shadowOffset = transShadow.transform.localPosition.y;

        heightOfObject = transBody.GetComponent<Collider2D>().bounds.size.y; //Collider of object must be its first collider.
    }

    private void Update()
    {
        if (baseObjAnimator)
        {
            baseObjAnimator.SetBool("Grounded", isGrounded);
            baseObjAnimator.SetFloat("VerticalVelocity", verticalVelocity);
        }
        UpdatePosition();
        CheckGroundHit();

        height = (float)Decimal.Round((Decimal)(transBody.position.y - transShadow.position.y), 3);
        prevGroundVelocity = transform.position;
        
    }

    public void Jump(Vector2 groundVelocity, float verticalVelocity, bool grounded = true, Vector3 rotation = default(Vector3), Quaternion rotationAfterLanding = default(Quaternion))
    {
        if (grounded)
        {
            this.groundVelocity = groundVelocity;
            this.verticalVelocity = verticalVelocity;
            isGrounded = false;
            landingRotation = rotationAfterLanding;
            airTimeRotation = rotation;
        }
    }

    //Changes floor height of object and drops it from a proportional height. objToIgnore is to prevent collision with the "solid" object of a platform system when dropping.
    public void Drop(Vector2 groundVel, float height, GameObject objToIgnore = null)
    {
        if (isGrounded)
        {
            this.groundVelocity = groundVel;
        }
        if (objToIgnore)
        {
            ignoredSolidPlatformObj = objToIgnore;
            //if (this.name == "PlayerParent")
            //{
                Physics2D.IgnoreCollision(transBody.GetComponent<BoxCollider2D>(), objToIgnore.GetComponent<PolygonCollider2D>(), true);
            //}
        }

        //if (this.name == "PlayerParent")
        //{
            transBody.GetComponent<BoxCollider2D>().enabled = false;
            transShadow.GetComponent<BoxCollider2D>().enabled = true;
        //}
        transShadow.position = new Vector2(transShadow.position.x, transShadow.position.y - height);
        transShadow.GetComponent<newShadowScript>().floorHeight -= height;
        isGrounded = false;
    }

    //Increases floor height of the object.
    public void Rise(float height)
    {
        if (!isGrounded)
        {
            transShadow.position = new Vector2(transShadow.position.x, transShadow.position.y + height);
            transShadow.GetComponent<newShadowScript>().floorHeight += height;
        }
    }

    void UpdatePosition()
    {
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
            transBody.position += new Vector3(0, verticalVelocity, 0) * Time.deltaTime;

            transObject.position += (Vector3)groundVelocity * Time.deltaTime;

            transBody.Rotate(airTimeRotation);
        } 

    }

    void CheckGroundHit()
    {
        if ((transBody.position.y + shadowOffset) <= transShadow.position.y && !isGrounded)
        {
            verticalVelocity = 0;
            transBody.position = transShadow.position + new Vector3(0, -shadowOffset);
            isGrounded = true;
            transBody.rotation = landingRotation;
            landingRotation = default(Quaternion);
            if (ignoredSolidPlatformObj)
            {
                if (transBody.GetComponent<BoxCollider2D>() != null)
                {
                    Physics2D.IgnoreCollision(transBody.GetComponent<BoxCollider2D>(), ignoredSolidPlatformObj.GetComponent<PolygonCollider2D>(), false);
                }
                ignoredSolidPlatformObj = null;
            }

        }
    }
}
