using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement variables
    private Vector2 moveInput;
    private Vector2 targetDirectionAndSpeed;
    public float speed;
    private float runSpeedMultiplier;
    private float walkingInterpolation;
    private Vector2 newPosition;

    private Vector2 moveVelocity;
    private Vector2 walkingDir;

    private bool isGrounded;
    public bool movementEnabled;

    //Braking variables
    private bool isBraking;
    private float brakeTimePassed;
    private Vector2 brakeMovementVelocity;
    private float brakeLerpedX;
    private float brakeLerpedY;
    private float initialBrakeSpeed;

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
    private direction[] VERTICALadjacent;
    private direction[] HORIZONTALadjacent;

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

        isBraking = false;
        brakeTimePassed = 0;
        newPosition = this.transform.position;
        initialBrakeSpeed = 0;

        VERTICALadjacent = new direction[2] { direction.LEFT, direction.RIGHT };
        HORIZONTALadjacent = new direction[2] { direction.UP, direction.DOWN };
    }

    //Update() should only be used to collect input. Actual movement must occur in FixedUpdate().
    private void Update()   
    {
        isGrounded = GetComponent<FakeHeightObject>().isGrounded;

        if (Input.GetKeyDown(KeyCode.Space) && this.tag == "Player" && isGrounded)
        {
            playerJump();
        }

        if (movementEnabled)
        {
            moveInput = new Vector2(Input.GetAxisRaw("HorizontalArrows"), Input.GetAxisRaw("VerticalArrows"));
            targetDirectionAndSpeed = moveInput.normalized * speed;

            //THIS SECTION: Make alterations if player running
            if (!isRunning && Input.GetKeyDown(KeyCode.LeftShift) && moveInput != Vector2.zero)
            {
                ToggleRun(true);
            }

            if (isRunning && ShouldStopRunning())
            {
                Debug.Log("PEDROLOG/3: ShouldStopRunning.");
                ToggleRun(false);
                BrakeStart();
            }

            //ANIMATION COMPONENTS
            playerAnimator.SetFloat("Vertical", Input.GetAxisRaw("VerticalArrows") * 2);
            playerAnimator.SetFloat("Horizontal", Input.GetAxisRaw("HorizontalArrows") * 2);
        }
    }

    

    private void FixedUpdate()
    {
        if (isRunning)
        {
            runSpeedMultiplier = 2;
            switch (movementDirection)
            {
                case direction.UP:
                    if (moveInput.y == 0 && !isBraking)
                    {
                        BrakeStart();
                    }
                    else
                    {
                        moveVelocity.Set(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, walkingInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, walkingInterpolation));
                    }
                    break;
                case direction.DOWN:
                    if (moveInput.y == 0 && !isBraking)
                    {
                        BrakeStart();
                    }
                    else
                    {
                        moveVelocity.Set(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, walkingInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, walkingInterpolation));
                    }
                    break;
                case direction.LEFT:
                    if (moveInput.x == 0 && !isBraking)   //If player lets go of the left direction while running to the left...
                    {
                        BrakeStart();
                    }
                    else //Otherwise, keep running this way.
                    {
                        moveVelocity.Set(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, walkingInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, walkingInterpolation));
                    }
                    break;
                case direction.RIGHT:
                    if (moveInput.x == 0 && !isBraking)
                    {
                        BrakeStart();
                    }
                    else
                    {
                        moveVelocity.Set(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, walkingInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, walkingInterpolation));
                    }
                    break;
                default:
                    moveVelocity.Set(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, walkingInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, walkingInterpolation));
                    break;
            }

        }
        else
        {
            runSpeedMultiplier = 1;
            moveVelocity.Set(Mathf.Lerp(moveVelocity.x, targetDirectionAndSpeed.x, walkingInterpolation), Mathf.Lerp(moveVelocity.y, targetDirectionAndSpeed.y, walkingInterpolation));
        }

        if (Mathf.Abs(moveVelocity.x) < 0.005f)
        {
            moveVelocity.Set(0, moveVelocity.y);
        }
        if (Mathf.Abs(moveVelocity.y) < 0.005f)
        {
            moveVelocity.Set(moveVelocity.x, 0);
        }

        newPosition = this.transform.position;
        Vector2 walkMovement = Vector2.zero;
        Vector2 brakeMovement = Vector2.zero;
        Vector2 totalMovement = Vector2.zero;

        if (GetComponent<FakeHeightObject>().isGrounded)
        {
            walkMovement = new Vector2(moveVelocity.x * (1 - hSlopeSlowdown), moveVelocity.y * (1 - vSlopeSlowdown)) * runSpeedMultiplier * Time.deltaTime;
            if (isBraking)
            {
                brakeMovement = CalculateBrakeMovement();
                walkMovement = WalkMovementBrakeAdjustment(walkMovement, brakeMovement);
            }
            totalMovement = walkMovement + brakeMovement;

            Debug.Log("PEDROLOG: movementDirection = " + movementDirection + ", walkingDir = " + walkingDir);
            Debug.Log("PEDROLOG2: breakMovementVelocity = " + movementDirection);
            newPosition += totalMovement;
            rb.MovePosition(newPosition);
        }

        movementDirection = GetPlayerMovementDirection();
    }

    private Vector2 WalkMovementBrakeAdjustment(Vector2 walkMovement, Vector2 brakeMovement)
    {
        /*switch (movementDirection)
        {
            case movementDirection == direction.UP:
                break;
        }*/

        return walkMovement;
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

    private Vector2 CalculateBrakeMovement()
    {
        Vector2 outputVector = Vector2.zero;

        if (brakeTimePassed < brakeTimeBeforeLerp) //Initial, linear slowdown
        {
            outputVector = new Vector2(brakeMovementVelocity.x * (1 - hSlopeSlowdown), brakeMovementVelocity.y * (1 - vSlopeSlowdown)) * 2 * Time.deltaTime;
            brakeTimePassed += 0.05f;
        } else
        {
            playerAnimator.SetBool("RunBrake", false);
            //NOTE: When the player speed when slowing down passes the regular movement speed, allow for interruption of this process by player input.
            if (Mathf.Abs(brakeLerpedX) > 0 || Mathf.Abs(brakeLerpedY) > 0) //Subsequent lerped slowdown
            {
                if (Mathf.Abs(brakeLerpedX) < 2f && Mathf.Abs(brakeLerpedY) < 2f)
                {
                    brakeLerpedX = 0;
                    brakeLerpedY = 0;
                }
                brakeLerpedX = Mathf.Lerp(brakeLerpedX, 0, brakeSlowdownRate);
                brakeLerpedY = Mathf.Lerp(brakeLerpedY, 0, brakeSlowdownRate);

                outputVector = new Vector2(brakeLerpedX * (1 - hSlopeSlowdown), brakeLerpedY * (1 - vSlopeSlowdown)) * 2 * Time.deltaTime;

            } else
            {
                //Finished braking in the original direction.
                isBraking = false;
                brakeLerpedX = 0;
                brakeLerpedY = 0;
                brakeMovementVelocity = Vector2.zero;
                brakeTimePassed = 0;
            }
        }

        return outputVector;
    }

    private void BrakeStart()
    {
        //Do NOT disable isRunning, because Brake() can be called while drifting.
        if (!isBraking)
        {
            isBraking = true;
            playerAnimator.SetBool("RunBrake", true);
            brakeMovementVelocity = moveVelocity * 0.9f;
            initialBrakeSpeed = brakeMovementVelocity.magnitude;
            brakeLerpedX = brakeMovementVelocity.x;
            brakeLerpedY = brakeMovementVelocity.y;
        }
    }

    private bool ShouldStopRunning()
    {
        switch (movementDirection)
        {
            case direction.UP:
                if (moveInput.y < 0)
                {
                    return true;
                }
                break;
            case direction.DOWN:
                if (moveInput.y > 0)
                {
                    return true;
                }
                break;
            case direction.LEFT:
                if (moveInput.x > 0)
                {
                    return true;
                }
                break;
            case direction.RIGHT:
                if (moveInput.x < 0)
                {
                    return true;
                }
                break;
            default:
                break;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            return true;
        }

        return false;
    }
}
