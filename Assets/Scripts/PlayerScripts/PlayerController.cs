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
    private direction movementDirection;

    private Rigidbody2D rb;

    //Test Variables for Braking
    [SerializeField]
    private float brakeTimeBeforeLerp;
    [SerializeField]
    private float brakeSlowdownRate;

    void Start()
    {
        movementEnabled = true;
        walkingInterpolation = 0.3f;
        rb = GetComponent<Rigidbody2D>();
        movementDirection = direction.STOP;
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
                StartCoroutine(RunBrake(movementDirection, moveVelocity));
            }

            if (isRunning)
            {
                runSpeedMultiplier = 2;
                //moveVelocity = new Vector2(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, runningInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, runningInterpolation));
                //moveVelocity = new Vector2(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, walkingInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, walkingInterpolation));
                switch (movementDirection) 
                {
                    case direction.UP:
                        if (moveInput.y == 0)   //If player lets go of the left direction while running to the left...
                        {
                            StartCoroutine(RunBrake(movementDirection, moveVelocity));  //Brake.
                        }
                        else //Otherwise, keep running this way.
                        {
                            moveVelocity = new Vector2(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, walkingInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, walkingInterpolation));
                        }
                        break;
                    case direction.DOWN:
                        if (moveInput.y == 0)   //If player lets go of the left direction while running to the left...
                        {
                            StartCoroutine(RunBrake(movementDirection, moveVelocity));  //Brake.
                        }
                        else //Otherwise, keep running this way.
                        {
                            moveVelocity = new Vector2(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, walkingInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, walkingInterpolation));
                        }
                        break;
                    case direction.LEFT:
                        if (moveInput.x == 0)   //If player lets go of the left direction while running to the left...
                        {
                            StartCoroutine(RunBrake(movementDirection, moveVelocity));  //Brake.
                        } else //Otherwise, keep running this way.
                        {
                            moveVelocity = new Vector2(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, walkingInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, walkingInterpolation));
                        }
                        break;
                    case direction.RIGHT:
                        if (moveInput.x == 0)
                        {
                            StartCoroutine(RunBrake(movementDirection, moveVelocity));  //Brake.
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

            if (Mathf.Abs(moveVelocity.x) < 0.005f)
            {
                moveVelocity = new Vector2(0, moveVelocity.y);
            }
            if (Mathf.Abs(moveVelocity.y) < 0.005f)
            {
                moveVelocity = new Vector2(moveVelocity.x, 0);
            }

            if (GetComponent<FakeHeightObject>().isGrounded)
            {
                rb.MovePosition(this.transform.position + (new Vector3(moveVelocity.x * (1 - hSlopeSlowdown), moveVelocity.y * (1 - vSlopeSlowdown)) * runSpeedMultiplier * Time.deltaTime));
            }

            movementDirection = GetPlayerMovementDirection();

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
        } else if (moveVelocity.x == 0 && moveVelocity.y == 0)
        {
            returnVal = direction.STOP;
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

    private IEnumerator RunBrake(direction momentumDirection, Vector2 breakMovementVelocity)
    {
        //disableMovement();
        ToggleRun(false);
        float breakTimePassed = 0;
        breakMovementVelocity = breakMovementVelocity * 0.9f;

        while (breakTimePassed < brakeTimeBeforeLerp) //Initial, linear slowdown
        {
            rb.MovePosition(this.transform.position + new Vector3(breakMovementVelocity.x * (1 - hSlopeSlowdown), breakMovementVelocity.y * (1 - vSlopeSlowdown)) * 2 * Time.deltaTime);
            breakTimePassed += 0.05f;
            yield return new WaitForEndOfFrame();
        }
        float lerpedX = breakMovementVelocity.x;
        float lerpedY = breakMovementVelocity.y;

        //NOTE: When the player speed when slowing down passes the regular movement speed, allow for interruption of this process by player input.
        while (Mathf.Abs(lerpedX) > 0 || Mathf.Abs(lerpedY) > 0) //Subsequent lerped slowdown
        {
            //Vector2 speedWhileBraking = new Vector2(lerpedX, lerpedY);

            if (Mathf.Abs(lerpedX) < 0.01f && Mathf.Abs(lerpedY) < 0.1f)
            {
                lerpedX = 0;
                lerpedY = 0;
            }
            lerpedX = Mathf.Lerp(lerpedX, 0, brakeSlowdownRate);
            lerpedY = Mathf.Lerp(lerpedY, 0, brakeSlowdownRate);

            rb.MovePosition(this.transform.position + new Vector3(lerpedX * (1 - hSlopeSlowdown), lerpedY * (1 - vSlopeSlowdown)) * 2 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        //enableMovement();
        yield return new WaitForEndOfFrame();
    }

}
