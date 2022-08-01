using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showRightArm : StateMachineBehaviour
{
    private Transform armTrans;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        armTrans = animator.transform.Find("PlayerArms").transform.Find("rightArmParent");
        
        armTrans.position = new Vector3(armTrans.localPosition.x, armTrans.localPosition.y, -0.6f);
        animator.transform.Find("PlayerArms").transform.Find("rightArmParent").localPosition = armTrans.position;
    }

}
