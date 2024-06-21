using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement variables
    public float speed;
    private float runSpeedMultiplier;
    private float walkingInterpolation;

    private Vector2 moveVelocity;
    Vector2 walkingDir;

    private bool isGrounded;
    public bool movementEnabled;

    //Running variables
    public bool isRunning;
    [SerializeField]
    private float runAcceleration;
    [SerializeField]
    private float runDeceleration;
    public float runningInterpolation;
    public float runningTurningInterpolation;

    //Animation variables
    [SerializeField]
    public Animator playerAnimator;

    //For slope speeds:
    public float hSlopeSlowdown;    //Should be proportional to slopeAngleInRad/(pi/2)
    public float hSlopeSlowdownMultiplier;
    public float vSlopeSlowdown;    //Should be proportional to slopeAngleInRad/(pi/2)
    public float vSlopeSlowdownMultiplier;

    private enum direction { STOP, UP, DOWN, LEFT, RIGHT };
    private direction playerMovementDirection;

    private Rigidbody2D rb;

    void Start()
    {
        movementEnabled = true;
        walkingInterpolation = 0.3f;
        rb = GetComponent<Rigidbody2D>();
        playerMovementDirection = direction.STOP;
        moveVelocity = Vector2.zero;
    }

    private void Update()
    {
        isGrounded = GetComponent<FakeHeightObject>().isGrounded;

        if (Input.GetKeyDown(KeyCode.Space) && this.tag == "Player" && isGrounded)
        {
            playerJump();
        }

        if (movementEnabled)
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("HorizontalArrows"), Input.GetAxisRaw("VerticalArrows"));
            Vector2 targetDirectionAndSpeed = moveInput.normalized * speed;

            //THIS SECTION: Make alterations if player running
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ToggleRun(true);
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                ToggleRun(false);
            }

            playerMovementDirection = GetPlayerMovementDirection();

            //Debug.Log("PEDROLOG: direction = " + playerMovementDirection);
            if (isRunning)
            {
                runSpeedMultiplier = 2;
                //Debug.Log("PEDROLOG: targetDirectionAndSpeed = " + targetDirectionAndSpeed);
                //Debug.Log("PEDROLOG: moveVelocity = " + moveVelocity);
                //moveVelocity = new Vector2(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, runningInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, runningInterpolation));
                switch (playerMovementDirection) 
                {
                    case direction.UP:                   
                        if (moveInput.y > 0)    //Continue moving in this direction, with some interpolation
                        {
                            moveVelocity = new Vector2(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, runningInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, runningInterpolation));
                        } else
                        {
                            //Player has released the key for running in this direction. Should "drift" if turning to another direction, or "brake" gradually if stopping/running in opposite direction.
                            moveVelocity = new Vector2(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, runningTurningInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, runningTurningInterpolation));
                        }                
                        break;
                    case direction.DOWN:
                        if (moveInput.y < 0)
                        {
                            moveVelocity = new Vector2(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, runningInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, runningInterpolation));
                        }
                        else
                        {
                            moveVelocity = new Vector2(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, walkingInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, walkingInterpolation));
                        }
                        break;
                    case direction.LEFT:
                        if (moveInput.x < 0)
                        {
                            moveVelocity = new Vector2(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, runningInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, runningInterpolation));
                        }
                        else
                        {
                            moveVelocity = new Vector2(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, walkingInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, walkingInterpolation));
                        }
                        break;
                    case direction.RIGHT:
                        if (moveInput.x > 0)
                        {
                            moveVelocity = new Vector2(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, runningInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, runningInterpolation));
                        }
                        else
                        {
                            moveVelocity = new Vector2(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, walkingInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, walkingInterpolation));
                        }
                        break;
                    default:
                        moveVelocity = new Vector2(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, walkingInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, walkingInterpolation));
                        break;
                }

            }
            else
            {
                runSpeedMultiplier = 1;
                moveVelocity = new Vector2(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, walkingInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, walkingInterpolation));
            }

            if (GetComponent<FakeHeightObject>().isGrounded)
            {
                rb.MovePosition(this.transform.position + (new Vector3(moveVelocity.x * (1 - hSlopeSlowdown), moveVelocity.y * (1 - vSlopeSlowdown)) * runSpeedMultiplier * Time.deltaTime));
            }

            //ANIMATION COMPONENTS
            playerAnimator.SetFloat("Vertical", Input.GetAxisRaw("VerticalArrows") * 2);
            playerAnimator.SetFloat("Horizontal", Input.GetAxisRaw("HorizontalArrows") * 2);
        }
    }

    private direction GetPlayerMovementDirection()
    {
        direction returnVal = direction.STOP;

        if (Mathf.Abs(moveVelocity.x) > Mathf.Abs(moveVelocity.y))
        {
            returnVal = moveVelocity.x < 0 ?  direction.LEFT :  direction.RIGHT;
        } else if (Mathf.Abs(moveVelocity.x) < Mathf.Abs(moveVelocity.y))
        {
            returnVal = moveVelocity.y < 0 ? direction.DOWN : direction.UP;
        }

        return returnVal;
    }

    //Leaps in the direction dir.
    void playerJump()
    {
        walkingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        GetComponent<FakeHeightObject>().Jump(walkingDir*25, 50);
    }

    private void ToggleRun(bool toggle)
    {
        isRunning = toggle;
        return;
    }

    public void disableMovement()
    {
        movementEnabled = false;
    }

    public void enableMovement()
    {
        movementEnabled = true;
    }

}
