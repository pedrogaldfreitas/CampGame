using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class verticalSpriteFlip : StateMachineBehaviour
{

    Vector3 theScale;
    private bool flipped;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!flipped)
        {
            theScale = animator.transform.localScale;
            theScale.y *= -1;
            animator.transform.localScale = theScale;
            flipped = true;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (flipped)
        {
            theScale = animator.transform.localScale;
            theScale.y *= -1;
            animator.transform.localScale = theScale;
            flipped = false;
        }
    }

    
}
