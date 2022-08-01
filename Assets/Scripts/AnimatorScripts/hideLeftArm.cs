using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideLeftArm : StateMachineBehaviour
{
    private Vector3 armVector;
    private Transform rightArmTrans;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            rightArmTrans = animator.transform.Find("PlayerArms").transform.Find("rightArmParent");

            armVector = new Vector3(rightArmTrans.localPosition.x, rightArmTrans.localPosition.y, 0);
            animator.transform.Find("PlayerArms").transform.Find("leftArmParent").localPosition = armVector;
    }

}
