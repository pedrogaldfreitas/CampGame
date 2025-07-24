using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelAI : MonoBehaviour
{

    public float speed;
    public Vector2 moveSpot;

    public enum State {WANDER, CHASE, ATTACK, CIRCLING, ALERTED, BACKUP};
    public State squirrelState;
    private bool squirrelAngry;

    private float waitTime;
    private float startWaitTime;
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    private float clockWise; // 1-10

    //ATTACK variables
    private bool jumpedOnce;

    Vector3 playerLegsPos;



    // Start is called before the first frame update
    void Start()
    {
        jumpedOnce = false;
        squirrelAngry = false;
        startWaitTime = Random.Range(0f, 5f);
        waitTime = startWaitTime;
        minX = transform.position.x - 9;
        maxX = transform.position.x + 9;
        minY = transform.position.y - 9;
        maxY = transform.position.y + 9;
        moveSpot = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        clockWise = Mathf.Round(Random.Range(1,10));

    }


    void Update()
    {
        //Determine which state should be used if the squirrel is angry.

        if (squirrelAngry == true)
        {
            //If squirrel is far from the player, use CHASE to get closer only if the previous state wasn't already in CIRCLING.
            if ((Vector2.Distance(GameObject.Find("Player").transform.position, this.transform.position) >= 21)&&(squirrelState != State.CIRCLING))
            {
                squirrelState = State.CIRCLING;
            } //If you back away from the squirrel while it's circling you, it'll take a little more distance for it to begin chasing you again.
            else if ((Vector2.Distance(GameObject.Find("Player").transform.position, this.transform.position) >= 32) && (squirrelState == State.CIRCLING))
            {
                squirrelState = State.CHASE;
            } /*else if (Vector2.Distance(GameObject.Find("Player").transform.position, this.transform.position) < 21)
            {
                squirrelState = State.BACKUP;
            }*/ else
            {
                squirrelState = State.CIRCLING;
            }
        }

        if (squirrelState == State.BACKUP)
        {
            transform.position = Vector2.MoveTowards(transform.position, GameObject.Find("Player").transform.position, -0.3f);
        }

        //CIRCLING STATE
        if (squirrelState == State.CIRCLING) 
        {
            //Hexagon Orbiting
            playerLegsPos = GameObject.Find("Legs").transform.position;

            if (transform.position.y-playerLegsPos.y > 2.972) {
                if (transform.position.x-playerLegsPos.x >= 0) //Topright
                {
                    if (clockWise > 5)
                    {
                        transform.position = new Vector2(transform.position.x + 0.15f, transform.position.y - 0.15f);
                    } else
                    {
                        transform.position = new Vector2(transform.position.x - 0.15f, transform.position.y + 0.15f);
                    }
                } else if (transform.position.x - playerLegsPos.x < 0)//Topleft
                {
                    if (clockWise > 5)
                    {
                        transform.position = new Vector2(transform.position.x + 0.15f, transform.position.y + 0.15f);
                    } else
                    {
                        transform.position = new Vector2(transform.position.x - 0.15f, transform.position.y - 0.15f);
                    }
                }
            } else if (transform.position.y-playerLegsPos.y < -2.972)
            {
                if (transform.position.x-playerLegsPos.x > 0) // Bottomright
                {
                    if (clockWise > 5)
                    {
                        transform.position = new Vector2(transform.position.x - 0.15f, transform.position.y - 0.15f);
                    } else
                    {
                        transform.position = new Vector2(transform.position.x + 0.15f, transform.position.y + 0.15f);
                    }
                } else if (transform.position.x - playerLegsPos.x <= 0)//Bottomleft
                {
                    if (clockWise > 5)
                    {
                        transform.position = new Vector2(transform.position.x - 0.15f, transform.position.y + 0.15f);
                    } else
                    {
                        transform.position = new Vector2(transform.position.x + 0.15f, transform.position.y - 0.15f);
                    }
                }
            } else
            {
                if (transform.position.x - playerLegsPos.x >= 0) //Right
                {
                    if (clockWise > 5)
                    {
                        transform.position = new Vector2(transform.position.x, transform.position.y - 0.21f);
                    } else
                    {
                        transform.position = new Vector2(transform.position.x, transform.position.y + 0.21f);
                    }
                } else if (transform.position.x - playerLegsPos.x < 0) //Left
                {
                    if (clockWise > 5)
                    {
                        transform.position = new Vector2(transform.position.x, transform.position.y + 0.21f);
                    } else
                    {
                        transform.position = new Vector2(transform.position.x, transform.position.y - 0.21f);
                    }
                }
            }



            //Move away or towards player depending on how close the squirrel is.

            if (Vector2.Distance(GameObject.Find("Player").transform.position, this.transform.position) > 21)
            {
                transform.position = Vector2.MoveTowards(transform.position, GameObject.Find("Player").transform.position, 0.3f);
            } else if (Vector2.Distance(GameObject.Find("Player").transform.position, this.transform.position) < 20)
            {
                transform.position = Vector2.MoveTowards(transform.position, GameObject.Find("Player").transform.position, -0.3f);
            }
        }

        //WANDER STATE
        if (squirrelState == State.WANDER)
        {
            //MOVE
            if (GetComponent<ObjectProperties>().height > 0)
            {
                //To ensure the squirrel jumps properly while moving.
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(moveSpot.x, moveSpot.y + GetComponent<ObjectProperties>().height), speed * Time.deltaTime);
            } else
            {
                transform.position = Vector2.MoveTowards(transform.position, moveSpot, speed * Time.deltaTime);
            }

            //WAIT
            if (Vector2.Distance(transform.position, moveSpot) < 0.2f)
            {

                if (waitTime <= 0)
                {
                    moveSpot = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                    startWaitTime = Random.Range(0f, 5f);
                    waitTime = startWaitTime;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }

        //CHASE STATE
        if (squirrelState == State.CHASE)
        {
            if (GetComponent<ObjectProperties>().height > 0)
            {
                //ERROR: Squirrel's y position doesn't move while height > 0.
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(GameObject.Find("Legs").transform.position.x, GameObject.Find("Legs").transform.position.y + GetComponent<ObjectProperties>().height), speed * Time.deltaTime*3);
            } else
            {
                transform.position = Vector2.MoveTowards(transform.position, GameObject.Find("Legs").transform.position, speed * Time.deltaTime * 3);
            }
        }

        //ATTACK STATE
        if (squirrelState == State.ATTACK)
        {


            if (jumpedOnce == false)
            {
                jumpedOnce = true;
                //'Gravity' deprecated: Use another Jump() function.
                //this.GetComponent<Gravity>().Jump(30);
            }

        }
    }

    public void changeState(string newState)
    {
        if (newState == "ANGRY")
        {
            squirrelAngry = true;
            clockWise = Mathf.Round(Random.Range(1, 10));
            //squirrelState = State.ALERTED;
        }
        return;
    }
}
