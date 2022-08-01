using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onSlopeOrNot : MonoBehaviour
{

    //SLOPE VARIABLES
    public bool onSlope;
    public float verticalSlope;
    public float horizontalSlope;

    [Range(1f, 5f)] public float raycastDistanceMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        onSlope = false;
        verticalSlope = 0;
        horizontalSlope = 0;
    }

    private void FixedUpdate()
    {
        SlopeCheck();
    }

    private void SlopeCheck()
    {
        RaycastHit2D slopeCheckRay = Physics2D.Raycast(this.transform.position + Vector3.down*raycastDistanceMultiplier, Vector2.down, 8f);
        if (slopeCheckRay)
        {
            Debug.DrawRay(slopeCheckRay.point, slopeCheckRay.normal, Color.blue);
            if (slopeCheckRay.collider.tag == "Slope")
            {
                horizontalSlope = slopeCheckRay.collider.GetComponent<slopeScriptRayCast>().horizontalSlope;
                verticalSlope = slopeCheckRay.collider.GetComponent<slopeScriptRayCast>().verticalSlope;
            }
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slope")
        {
            onSlope = true;
            if (collision.gameObject.GetComponent<newSlopeScript>().thisSlopeState == newSlopeScript.slopeFacing.VERTICAL)
            {
                onVerticalSlope = true;
            }
            if (collision.gameObject.GetComponent<newSlopeScript>().thisSlopeState == newSlopeScript.slopeFacing.HORIZONTAL)
            {
                onHorizontalSlope = true;
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slope")
        {
            onSlope = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slope")
        {
            onSlope = false;
            if (collision.gameObject.GetComponent<newSlopeScript>().thisSlopeState == newSlopeScript.slopeFacing.VERTICAL)
            {
                onVerticalSlope = false;
            }
            if (collision.gameObject.GetComponent<newSlopeScript>().thisSlopeState == newSlopeScript.slopeFacing.HORIZONTAL)
            {
                onHorizontalSlope = false;
            }
        }
    }*/
}
