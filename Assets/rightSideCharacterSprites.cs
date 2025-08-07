using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rightSideCharacterSprites : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform headParent = animator.transform.Find("HeadParent");
        headParent.localPosition = new Vector3(-0.0892f, 1.3675f, 0.2f);
        headParent.transform.Find("HairFront").localPosition = new Vector3(-0.0953f, 0.4152f, -0.2f);
        headParent.transform.Find("HairBack").localPosition = new Vector3(0.0032f, -0.5213f, 0.2f);
        animator.transform.Find("EyesParent").localPosition = new Vector3(0.00416f, 1.26767f, 0.1f);
        animator.transform.Find("NoseParent").localPosition = new Vector3(0.00644f, 1.04531f, 0.1f);
        animator.transform.Find("MouthParent").localPosition = new Vector3(0.01778f, 0.75943f, 0.1f);
        animator.transform.Find("TorsoParent").localPosition = new Vector3(-0.00751f, 0.2285f, 0.3f);

        Transform playerArms = animator.transform.Find("Arms");
        playerArms.localPosition = new Vector3(-2.472654f, 4.085711f, 0);

        Transform leftArmParent = playerArms.Find("LeftArmParent");
        Transform rightArmParent = playerArms.Find("RightArmParent");
        Transform leftSleeve = leftArmParent.Find("LeftSleeveParent");
        Transform rightSleeve = rightArmParent.Find("RightSleeveParent");

        leftArmParent.localPosition = new Vector3(14.41465f, -20.9424f, 0.6f);
        leftArmParent.localScale = new Vector3(2.26892f, 2.26892f, 1);
        rightArmParent.localPosition = new Vector3(11.6903f, -21.464f, -0.6f);
        rightArmParent.localScale = new Vector3(2.26892f, 2.26892f, 1);

        //NOTE: Moving LeftArm or RightArm should be reserved for the animator. Instead, change the pivot of the back arm sprite + sleeve sprites and set only the position of leftArmParent and rightArmParent accordingly.
        //leftArmParent.Find("LeftArm").localPosition = new Vector3(0, 0, 0); 
        //rightArmParent.Find("RightArm").localPosition = new Vector3(0.2558f, -0.083f, 0);

        leftSleeve.localPosition = new Vector3(-0.23995f, 0.95505f, -0.1f);
        leftSleeve.localScale = new Vector3(-1, 1, 1);
        rightSleeve.localPosition = new Vector3(0.24005f, 0.95505f, -0.1f);
        rightSleeve.localScale = new Vector3(1, 1, 1);

        animator.transform.Find("LegsParent").localPosition = new Vector3(-0.02524f, -0.5232f, 0.4f);
    }
}
