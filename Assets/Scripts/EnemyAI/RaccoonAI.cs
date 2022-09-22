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

    public enum State { WANDER, CHASE, JUMPATTACK, ALERT, RELOCATE };
    public State raccoonState;

    public bool raccoonAngry;
    private GameObject player;
    private Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        moveSpot = new Vector2(0, 0);
        player = GameObject.Find("Player");
        parent = transform.parent;
        raccoonAngry = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (raccoonState == State.CHASE)
        {
            if (parent.GetComponent<FakeHeightObject>().isGrounded)
            {
                currSpot = transform.position;
                moveSpot = Vector2.MoveTowards(transform.position, player.transform.position, speed/15f);
                parent.position = moveSpot;
                if (Vector2.Distance(transform.position, player.transform.position) < 15)
                {
                    //raccoonState = State.JUMPATTACK;
                }
            }
        }
        if (raccoonState == State.JUMPATTACK)
        {
            Debug.Log("JUMPATTACK!");

            moveSpot = Vector2.MoveTowards(transform.position, player.transform.position, speed / 8f);
            parent.gameObject.GetComponent<FakeHeightObject>().Jump((moveSpot - currSpot)*30f, 40);
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
