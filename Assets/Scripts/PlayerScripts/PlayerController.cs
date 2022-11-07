using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement variables
    public float speed;
    private float runSpeedMultiplier;

    private Vector2 moveVelocity;
    Vector2 walkingDir;

    private bool isGrounded;
    public bool movementEnabled;

    //Animation variables
    [SerializeField]
    public Animator playerAnimator;

    //For slope speeds:
    public float hSlopeSlowdown;    //Should be proportional to slopeAngleInRad/(pi/2)
    public float hSlopeSlowdownMultiplier;
    public float vSlopeSlowdown;    //Should be proportional to slopeAngleInRad/(pi/2)
    public float vSlopeSlowdownMultiplier;

    void Start()
    {
        movementEnabled = true;
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
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            moveVelocity = moveInput.normalized * speed;

            runSpeedMultiplier = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;

            if (GetComponent<FakeHeightObject>().isGrounded)
            {
                transform.position += new Vector3(moveVelocity.x*(1-hSlopeSlowdown), moveVelocity.y*(1-vSlopeSlowdown)) * runSpeedMultiplier * Time.deltaTime;
            }

            //ANIMATION COMPONENTS
            playerAnimator.SetFloat("Vertical", Input.GetAxisRaw("Vertical") * 2);
            playerAnimator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal") * 2);
        }
    }

    //Leaps in the direction dir.
    void playerJump()
    {
        walkingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        GetComponent<FakeHeightObject>().Jump(walkingDir*25, 50);
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
