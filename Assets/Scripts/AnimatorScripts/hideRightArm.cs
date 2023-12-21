using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideRightArm : StateMachineBehaviour
{
    private Transform armTrans;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Transform armTrans = animator.transform.Find("PlayerArms").transform.Find("rightArmParent");

        armTrans.localPosition = new Vector3(12.551f, -21.287f, 0.6f);
        armTrans.localScale = new Vector3(-1, 1, 1);

        armTrans.Find("RightSleeve").localPosition = new Vector3(0.1423f, 2.186f, -0.1f);

    }

}
