using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forwardCharacterSprites : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.Find("PlayerHeadParent").Find("PlayerHead").localPosition = new Vector3(0.02456036f, 1.3743f, 0.2f);
        animator.transform.Find("PlayerEyesParent").Find("PlayerEyes").localPosition = new Vector3(0.01548023f, 1.372019f, 0.1f);
        animator.transform.Find("PlayerNoseParent").Find("PlayerNose").localPosition = new Vector3(-0.01399994f, 1.115659f, 0.1f);
        animator.transform.Find("PlayerMouthParent").Find("PlayerMouth").localPosition = new Vector3(0.04272022f, 0.8801713f, 0.1f);
        animator.transform.Find("PlayerTorsoParent").Find("PlayerTorso").localPosition = new Vector3(0.00866f, -0.003f, 0.3f);

        Transform playerArms = animator.transform.Find("PlayerArms");
        playerArms.localPosition = new Vector3(-2.472654f, 4.085711f, 0);

        Transform leftArmParent = playerArms.Find("leftArmParent");
        Transform rightArmParent = playerArms.Find("rightArmParent");

        leftArmParent.localPosition = new Vector3(14.89107f, -17.56166f, 0.6f);
        rightArmParent.localPosition = new Vector3(9.51f, -17.39f, 0.6f);
        rightArmParent.localScale = new Vector3(2.26892f, 2.26892f, 1);

        //NOTE: Moving PlayerLeftArm or PlayerRightArm should be reserved for the animator. Instead, change the pivot of the back arm sprite + sleeve sprites and set only the position of leftArmParent and rightArmParent accordingly.
        //leftArmParent.Find("PlayerLeftArm").localPosition = new Vector3(0, 0, 0); 
        //rightArmParent.Find("PlayerRightArm").localPosition = new Vector3(0.2558f, -0.083f, 0);

        leftArmParent.Find("LeftSleeveParent").localPosition = new Vector3(-0.4102f, -0.628f, -0.1f);
        rightArmParent.Find("RightSleeveParent").localPosition = new Vector3(1.189f, -1.483f, -0.1f);

        animator.transform.Find("PlayerLegsParent").Find("PlayerLegs").localPosition = new Vector3(0.03590031f, -0.5746201f, 0.4f);
    }

}
