using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideRightArm : StateMachineBehaviour
{
    private Transform armTrans;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        armTrans = animator.transform.Find("PlayerArms").transform.Find("rightArmParent");
        armTrans.position = new Vector3(armTrans.localPosition.x, armTrans.localPosition.y, 0f);
        armTrans.localPosition = armTrans.position;
        Debug.Log("Put right arm at " + armTrans.localPosition);

    }

}
