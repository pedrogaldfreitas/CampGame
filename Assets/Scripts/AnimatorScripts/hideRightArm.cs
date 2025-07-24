using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideRightArm : StateMachineBehaviour
{
    private Transform armTrans;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Transform armTrans = animator.transform.Find("Arms").transform.Find("RightArmParent");

        armTrans.localPosition = new Vector3(12.551f, -21.287f, 0.6f);
        armTrans.localScale = new Vector3(-2.26892f, 2.26892f, 1);

        armTrans.Find("RightSleeveParent").localPosition = new Vector3(0.155f, 0.901f, -0.1f);

    }

}
