using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forwardCharacterSprites : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.Find("PlayerArms").transform.Find("leftArmParent").localPosition = new Vector3(14.89107f, -17.56166f, 0);
        animator.transform.Find("PlayerArms").transform.Find("rightArmParent").localPosition = new Vector3(9.693272f, -17.54705f, 0);
        animator.transform.Find("PlayerArms").localPosition = new Vector3(-2.472654f, 4.085711f, 1);
        animator.transform.Find("PlayerTorso").localPosition = new Vector3(0.00866f, -0.003f, 0.6f);
        animator.transform.Find("PlayerHead").localPosition = new Vector3(0.02456036f, 1.3743f, 0.4f);
        animator.transform.Find("PlayerLegs").localPosition = new Vector3(0.03590031f, -0.5746201f, 0.8f);
        animator.transform.Find("PlayerEyes").localPosition = new Vector3(0.01548023f, 1.372019f, 0.2f);
        animator.transform.Find("PlayerNose").localPosition = new Vector3(-0.01399994f, 1.115659f, 0.2f);
        animator.transform.Find("PlayerMouth").localPosition = new Vector3(0.04272022f, 0.8801713f, 0.2f);
    }

}
