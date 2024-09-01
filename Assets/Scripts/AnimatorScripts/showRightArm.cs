using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showRightArm : StateMachineBehaviour
{

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Transform armTrans = animator.transform.Find("PlayerArms").transform.Find("rightArmParent");

        armTrans.localPosition = new Vector3(12.5f, -21.238f, -0.6f);
        armTrans.localScale = new Vector3(-2.26892f, 2.26892f, 1);

        armTrans.Find("RightSleeveParent").localPosition = new Vector3(0.155f, 0.901f, -0.1f);
    }

}
