using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showOnlyTorso : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.Find("Arms").transform.Find("RightArmParent").Find("RightArm").GetComponent<SpriteRenderer>().enabled = false;
        animator.transform.Find("Arms").transform.Find("LeftArmParent").Find("LeftArm").GetComponent<SpriteRenderer>().enabled = false;
        animator.transform.Find("LegsParent").transform.Find("Legs").GetComponent<SpriteRenderer>().enabled = false;
        animator.transform.Find("HeadParent").transform.Find("Head").GetComponent<SpriteRenderer>().enabled = false;

        animator.transform.Find("EyesParent").Find("Eyes").GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        animator.transform.Find("NoseParent").Find("Nose").GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        animator.transform.Find("MouthParent").Find("Mouth").GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.Find("Arms").transform.Find("RightArmParent").Find("RightArm").GetComponent<SpriteRenderer>().enabled = true;
        animator.transform.Find("Arms").transform.Find("LeftArmParent").Find("LeftArm").GetComponent<SpriteRenderer>().enabled = true;
        animator.transform.Find("LegsParent").Find("Legs").GetComponent<SpriteRenderer>().enabled = true;
        animator.transform.Find("HeadParent").Find("Head").GetComponent<SpriteRenderer>().enabled = true;

        animator.transform.Find("EyesParent").Find("Eyes").GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        animator.transform.Find("NoseParent").Find("Nose").GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        animator.transform.Find("MouthParent").Find("Mouth").GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }

}
