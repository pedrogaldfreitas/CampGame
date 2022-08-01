using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFacing : MonoBehaviour
{
    public enum facingDir { UP, DOWN, LEFT, RIGHT };
    public facingDir playerFacingDir;
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        playerFacingDir = facingDir.DOWN;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo animStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (animStateInfo.IsName("playerRight")||animStateInfo.IsName("playerIdleRight"))
        {
            playerFacingDir = facingDir.RIGHT;
        }
        if (animStateInfo.IsName("playerLeft") || animStateInfo.IsName("playerIdleLeft"))
        {
            playerFacingDir = facingDir.LEFT;
        }
        if (animStateInfo.IsName("playerForward") || animStateInfo.IsName("playerIdle"))
        {
            playerFacingDir = facingDir.DOWN;
        }
        if (animStateInfo.IsName("playerBackward") || animStateInfo.IsName("idleBack"))
        {
            playerFacingDir = facingDir.UP;
        }

        animator.SetInteger("FacingDir(UDLR)", (int)playerFacingDir);

    }
}
