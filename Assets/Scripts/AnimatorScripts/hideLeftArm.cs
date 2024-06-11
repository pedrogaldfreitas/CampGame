using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideLeftArm : StateMachineBehaviour
{
    private Vector3 armVector;
    private Transform rightArmTrans;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform leftArm = animator.transform.Find("PlayerArms").transform.Find("leftArmParent");
        armVector = new Vector3(12.039f, -21.4548f, 0.6f);

        leftArm.localPosition = armVector;
        leftArm.Find("LeftSleeveParent").localPosition = new Vector3(0.05f, 1f, -0.1f);
    }

}
