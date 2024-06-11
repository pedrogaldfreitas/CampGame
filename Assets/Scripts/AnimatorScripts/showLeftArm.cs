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
        Transform leftArm = animator.transform.Find("PlayerArms").transform.Find("leftArmParent");
        armVector = new Vector3(12.295f, -21.371f, -0.6f);

        leftArm.localPosition = armVector;
        leftArm.Find("LeftSleeveParent").localPosition = new Vector3(0.05f, 1f, -0.1f);
    }
}
