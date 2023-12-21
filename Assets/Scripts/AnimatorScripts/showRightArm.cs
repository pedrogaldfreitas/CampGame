using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showRightArm : StateMachineBehaviour
{

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Transform armTrans = animator.transform.Find("PlayerArms").transform.Find("rightArmParent");

        armTrans.localPosition = new Vector3(12.2948f, -21.3718f, -0.6f);
        armTrans.localScale = new Vector3(-1, 1, 1);

        armTrans.Find("RightSleeve").localPosition = new Vector3(0.1423f, 2.186f, -0.1f);
    }

}
