using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaccoonAI : MonoBehaviour
{
    //INTERFACE ON UNITY
    [Range(0, 50)]
    public int growlRadius = 50;
    [Range(0, 50)]
    public int attackRadius = 50;
    public float speed;
    public Vector2 moveDirection;
    private float distanceFromPlayer;

    private Transform parent;
    private Transform landTarget;
    private Transform playerLandTarget;
    private Enemy enemyScript;
    private Rigidbody2D parentRB;

    public enum State { EATINGTRASH, CHASE, JUMPATTACK, ALERT, RELOCATE, BACKANDFORTHJUMPTEST };
    public State raccoonState;

    private bool CR_running;

    private Vector2 testHopDirection = Vector2.right;

    // Start is called before the first frame update
    void Start()
    {
        enemyScript = GetComponent<Enemy>();
        CR_running = false;
        moveDirection = new Vector2(0, 0);
        playerLandTarget = GameObject.Find("Player").transform.Find("LandTarget");
        parent = transform.parent;
        parentRB = parent.GetComponent<Rigidbody2D>();
        landTarget = transform.parent.Find("LandTarget");
    }

    // Update is called once per frame
    void Update()
    {
        switch(raccoonState)
        {
            case State.EATINGTRASH:
                distanceFromPlayer = Vector2.Distance(playerLandTarget.position, landTarget.position);
                //IDEA: Raccoon is chill when player is far away.
                if (distanceFromPlayer >= growlRadius)
                {
                    //Raccoon is chill when player is far away, eating trash.
                } else if (distanceFromPlayer < growlRadius && distanceFromPlayer >= attackRadius)
                {
                    //Raccoon growls.
                    if (!CR_running)
                    {
                        StartCoroutine(RaccoonBlink());
                    }
                } else
                {
                    raccoonState = State.CHASE;
                    //Raccoon attacks.
                }
                //If player gets closer, Raccoon growls but stays still.

                break;
            case State.CHASE:
                if (parent.GetComponent<FakeHeightObject>().isGrounded && !enemyScript.movementBlocked)
                {
                    moveDirection = (playerLandTarget.position - landTarget.position).normalized;
                    parentRB.MovePosition(parent.position + (Vector3)(moveDirection * speed / 15f));

                    if (Vector2.Distance(transform.position, playerLandTarget.transform.position) < 15)
                    {
                        //raccoonState = State.JUMPATTACK;
                    }
                }
                break;
            case State.JUMPATTACK:
                //moveSpot = Vector2.MoveTowards(transform.position, player.transform.position, speed / 8f);
                //parent.gameObject.GetComponent<FakeHeightObject>().Jump((moveSpot - currSpot)*30f, 40);
                raccoonState = State.CHASE;
                break;
            case State.BACKANDFORTHJUMPTEST:
                if (parent.GetComponent<FakeHeightObject>().isGrounded)
                {
                    parent.gameObject.GetComponent<FakeHeightObject>().Jump(testHopDirection * 30f, 40);
                    testHopDirection = -testHopDirection;
                }
                break;
            default:
                break;
        }

    }

    IEnumerator RaccoonBlink()
    {
        CR_running = true;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color1 = new Color(0.6981132f, 0.06915272f, 0.1973518f);
        Color color2 = new Color(1, 0, 0.113f);
        while (distanceFromPlayer < 30 && distanceFromPlayer >= 10)
        {
            if (spriteRenderer.color == color1)
            {
                spriteRenderer.color = color2;
            } else
            {
                spriteRenderer.color = color1;
            }
            yield return new WaitForSeconds(0.25f);
        }
        spriteRenderer.color = color1;
        CR_running = false;
    }
}
