using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteFlip : StateMachineBehaviour
{

    Vector3 theScale;
    private bool flipped;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Flipping the character horizontally if he faces left. (Reduces # of sprites needed)
        if (!flipped)
        {
            theScale = animator.transform.localScale;
            theScale.x *= -1;
            animator.transform.localScale = theScale;
            flipped = true;
        }
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (flipped)
        {
            theScale = animator.transform.localScale;
            theScale.x *= -1;
            animator.transform.localScale = theScale;
            flipped = false;
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
