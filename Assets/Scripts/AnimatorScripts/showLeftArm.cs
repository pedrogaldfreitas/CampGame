using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showLeftArm : StateMachineBehaviour
{

    private Vector3 armVector;
    private Transform rightArmTrans;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rightArmTrans = animator.transform.Find("PlayerArms").transform.Find("rightArmParent");

        armVector = new Vector3(rightArmTrans.localPosition.x, rightArmTrans.localPosition.y, -0.6f);
        animator.transform.Find("PlayerArms").transform.Find("leftArmParent").localPosition = armVector;    
    }
}
