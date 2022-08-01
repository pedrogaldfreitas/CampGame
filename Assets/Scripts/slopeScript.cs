/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slopeScript : MonoBehaviour
{
    private slopeTile[] slope;

    /* THE MULTIPLIER for the horizontalDiff variable. This is to make it so there are more slopeTiles in the array
       and to make the movement uphill less choppy. 
    private int m;

    [SerializeField]
    private GameObject platform1;
    [SerializeField]
    private GameObject platform2;
    private float h1;
    private float h2;
    private float heightDiff;
    private int horizontalDiff;
    //"playerStepPos" finds the centered position of the bottom of the player's feet. 
    private Vector2 playerStepPos;
    private float playerStepPosRoundedX;
    private GameObject playerLegs;
    private PolygonCollider2D polyCollider;

    float currentRoundedSlopeTileXVal;

    int rightSlopeTileIndex;
    int leftSlopeTileIndex;
    int currSlopeTileIndex;

    void Start()
    {

        m = 1;
        playerLegs = GameObject.Find("PlayerLegs");
        polyCollider = GetComponent<PolygonCollider2D>();
        h1 = platform1.GetComponent<platformScript>().floorHeight;
        h2 = platform2.GetComponent<platformScript>().floorHeight;
        horizontalDiff = (int)polyCollider.bounds.extents.x * 2;
        heightDiff = Mathf.Abs(h2 - h1);

        Debug.Log("h1: " + h1);
        Debug.Log("h2: " + h2);
        Debug.Log("horizontalDiff: " + horizontalDiff);
        Debug.Log("heightDiff: " + heightDiff);


        /*Initialize the array of values between the two platforms containing the different floorheights
         based on the horizontal difference between the two platforms.
        slope = new slopeTile[horizontalDiff * m];

        //Populate the slopeTile array.
        for (int i = 0; i < slope.Length; i++)
        {
            slope[i] = new slopeTile();
        }

        //Insert each slopetile in the correct location of the slope, and asign each slopetile a proper floorheight value.
        for (int i = 0; i < slope.Length; i++)
        {
            //First take the center of the slope, then move to the very left of it. Then, add the x positions of the following things.
            slope[i].pos = polyCollider.bounds.center.x - polyCollider.bounds.extents.x + 2 * (((i + 1) * polyCollider.bounds.extents.x / (horizontalDiff * m)));

            if (h2 > h1)
            {
                slope[i].floorh = h1 + (heightDiff / (horizontalDiff * m)) * i;
            } else
            {
                slope[i].floorh = h1 - (heightDiff / (horizontalDiff * m)) * i;
            }
            Debug.Log("x position: " + slope[i].pos + ", floorHeight: " + slope[i].floorh);

        }
    }


    void Update()
    {
        //WARNING: When changing player sprite, change the value from 1.04f to the correct value corresponding to the bottom of their feet.
        playerStepPos = new Vector2(playerLegs.transform.position.x, playerLegs.transform.position.y - 1.04f);

        //Round the x value of the player's footstep to two decimal places.
        playerStepPosRoundedX = (Mathf.Round(playerStepPos.x * 10)) / 10;


    }

    //While on the slope, the player will go up or down as he goes left or right.
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (playerStepPosRoundedX < slope[0].pos) {
            currSlopeTileIndex = 0;
            leftSlopeTileIndex = 0;
            rightSlopeTileIndex = 1;
        }
        else if (((playerStepPosRoundedX >= slope[currSlopeTileIndex].pos) && (playerStepPosRoundedX < slope[rightSlopeTileIndex].pos)) && (rightSlopeTileIndex < slope.Length))
        {
            leftSlopeTileIndex = currSlopeTileIndex;
            currSlopeTileIndex = currSlopeTileIndex + 1;
            rightSlopeTileIndex = currSlopeTileIndex + 1;
            Debug.Log("CURRSLOPETILE: " + currSlopeTileIndex);
            collision.GetComponent<ObjectProperties>().getFloorHeight(slope[currSlopeTileIndex].floorh);
        }
        else if (((playerStepPosRoundedX < slope[currSlopeTileIndex].pos) && (playerStepPosRoundedX >= slope[leftSlopeTileIndex].pos)) && (leftSlopeTileIndex > 0))
        {
            currSlopeTileIndex = currSlopeTileIndex - 1;
            leftSlopeTileIndex = currSlopeTileIndex - 1;
            rightSlopeTileIndex = currSlopeTileIndex + 1;
            collision.GetComponent<ObjectProperties>().getFloorHeight(slope[currSlopeTileIndex].floorh);
        }

        /*
        for (int i = 0; i < slope.Length; i++)
        {
            currentRoundedSlopeTileXVal = (Mathf.Round(slope[i].pos* 10)) / 10;
            if ( playerStepPosRoundedX == currentRoundedSlopeTileXVal )
            {
                Debug.Log(slope[i].floorh);
                collision.GetComponent<ObjectProperties>().getFloorHeight(slope[i].floorh);
            }
        }
    }

    public class slopeTile
    {
        public float floorh;
        public float pos;

        public slopeTile()
        {
            floorh = 0f;
            pos = 0f;
        }

    }
}*/
