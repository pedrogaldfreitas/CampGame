using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missionManager : MonoBehaviour
{
    [SerializeField]
    private string missionActive;

    private float[] xPositionBounds;
    private float[] yPositionBounds;

    private GameObject player;


    /*
     * OBJECTIVES (DAY 1):
     * Go find a seat at the campfire (D1M1)
     * Go find the missing girl (D1M2)
     * Fight raccoon (D1M3)
     * Help girl (D1M4)
     * Pick up strange green stone (D1M5)
     * find a way out of the gazebo using the shore + return to campfire (D1M6)
     * Go line up for Lunch (D1M7)
     * 
     */

    // Start is called before the first frame update
    void Start()
    {
        xPositionBounds = new float[2];
        yPositionBounds = new float[2];

        missionActive = "none";
        xPositionBounds[0] = -999;
        xPositionBounds[1] = 999;
        yPositionBounds[0] = -999;
        yPositionBounds[1] = 999;
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        switch (missionActive)
        {
            case "D1M1":
                //Debug.Log("D1M1 active.");
                break;
            case "D1M2":
                //Debug.Log("D1M2 active.");
                break;
            case "D1M3":
                //Debug.Log("D1M3 active.");
                break;
            case "D1M4":
                //Debug.Log("D1M4 active.");
                break;
        }
        /*if (player.transform.position.x >= xPositionBounds[1])
        {

        }
        if (player.transform.position.x <= xPositionBounds[0])
        {

        }
        if (player.transform.position.y >= yPositionBounds[1])
        {

        }
        if (player.transform.position.y <= yPositionBounds[0])
        {

        }*/

    }

    public void setMission(string mission)
    {
        missionActive = mission;

        //Set the bounds for the mission.
        switch (mission)
        {
            case "D1M1":
                xPositionBounds[0] = -999;
                xPositionBounds[1] = 999;
                yPositionBounds[0] = -999;
                yPositionBounds[1] = 999;
                break;
            case "D1M2":
                xPositionBounds[0] = -999;
                xPositionBounds[1] = 999;
                yPositionBounds[0] = -999;
                yPositionBounds[1] = 999;
                break;
            case "D1M3":
                xPositionBounds[0] = -999;
                xPositionBounds[1] = 999;
                yPositionBounds[0] = -999;
                yPositionBounds[1] = 999;
                break;
            case "D1M4":
                xPositionBounds[0] = -999;
                xPositionBounds[1] = 999;
                yPositionBounds[0] = -999;
                yPositionBounds[1] = 999;
                break;
            case "D1M5":
                xPositionBounds[0] = -999;
                xPositionBounds[1] = 999;
                yPositionBounds[0] = -999;
                yPositionBounds[1] = 999;
                break;
        }
    }

}
