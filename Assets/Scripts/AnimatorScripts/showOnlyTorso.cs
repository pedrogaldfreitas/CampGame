﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showOnlyTorso : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.Find("PlayerArms").transform.Find("rightArmParent").Find("PlayerRightArm").GetComponent<SpriteRenderer>().enabled = false;
        animator.transform.Find("PlayerArms").transform.Find("leftArmParent").Find("PlayerLeftArm").GetComponent<SpriteRenderer>().enabled = false;
        animator.transform.Find("PlayerLegsParent").transform.Find("PlayerLegs").GetComponent<SpriteRenderer>().enabled = false;
        animator.transform.Find("PlayerHeadParent").transform.Find("PlayerHead").GetComponent<SpriteRenderer>().enabled = false;

        animator.transform.Find("PlayerEyesParent").Find("PlayerEyes").GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        animator.transform.Find("PlayerNoseParent").Find("PlayerNose").GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        animator.transform.Find("PlayerMouthParent").Find("PlayerMouth").GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.Find("PlayerArms").transform.Find("rightArmParent").Find("PlayerRightArm").GetComponent<SpriteRenderer>().enabled = true;
        animator.transform.Find("PlayerArms").transform.Find("leftArmParent").Find("PlayerLeftArm").GetComponent<SpriteRenderer>().enabled = true;
        animator.transform.Find("PlayerLegsParent").Find("PlayerLegs").GetComponent<SpriteRenderer>().enabled = true;
        animator.transform.Find("PlayerHeadParent").Find("PlayerHead").GetComponent<SpriteRenderer>().enabled = true;

        animator.transform.Find("PlayerEyesParent").Find("PlayerEyes").GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        animator.transform.Find("PlayerNoseParent").Find("PlayerNose").GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        animator.transform.Find("PlayerMouthParent").Find("PlayerMouth").GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }

}
