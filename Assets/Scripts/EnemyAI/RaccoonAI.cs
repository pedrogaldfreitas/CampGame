using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaccoonAI : MonoBehaviour
{

    /*RACCOON BEHAVIORS:
     * 
     * -Wander (just like squirrel)
     * 
     * -Run towards player
     * -Jump at player (faster attack)
     * -Stand still, staring at player
     * 
     */

    public float speed;
    public Vector2 currSpot;
    public Vector2 moveSpot;

    private Transform landTarget;

    public enum State { WANDER, CHASE, JUMPATTACK, ALERT, RELOCATE };
    public State raccoonState;

    public bool raccoonAngry;
    private Transform playerLandTarget;
    private Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        moveSpot = new Vector2(0, 0);
        playerLandTarget = GameObject.Find("PlayerParent").transform.Find("LandTarget");
        parent = transform.parent;
        raccoonAngry = false;
        landTarget = transform.parent.Find("LandTarget");
    }

    // Update is called once per frame
    void Update()
    {

        if (raccoonState == State.CHASE)
        {
            if (parent.GetComponent<FakeHeightObject>().isGrounded)
            {
                currSpot = landTarget.position;
                moveSpot = Vector2.MoveTowards(currSpot, playerLandTarget.transform.position, speed / 15f);
                parent.position = moveSpot;

                if (Vector2.Distance(transform.position, playerLandTarget.transform.position) < 15)
                {
                    //raccoonState = State.JUMPATTACK;
                }
            }
        }
        if (raccoonState == State.JUMPATTACK)
        {
            Debug.Log("JUMPATTACK!");

            //moveSpot = Vector2.MoveTowards(transform.position, player.transform.position, speed / 8f);
            //parent.gameObject.GetComponent<FakeHeightObject>().Jump((moveSpot - currSpot)*30f, 40);
            // raccoonState = State.RELOCATE;
            raccoonState = State.CHASE;
        }
        if (raccoonState == State.RELOCATE)
        {
            if (parent.GetComponent<FakeHeightObject>().isGrounded)
            {

            }
        }

    }
}
