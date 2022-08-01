using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backwardCharacterSprites : StateMachineBehaviour
{
    Transform rightArmTrans;
    Transform leftArmTrans;
    GameObject eyes;
    GameObject nose;
    GameObject mouth;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rightArmTrans = animator.transform.Find("PlayerArms").transform.Find("rightArmParent");
        leftArmTrans = animator.transform.Find("PlayerArms").transform.Find("leftArmParent");
        eyes = animator.transform.Find("PlayerEyes").gameObject;
        nose = animator.transform.Find("PlayerNose").gameObject;
        mouth = animator.transform.Find("PlayerMouth").gameObject;

        rightArmTrans.localPosition = new Vector3(15.02f, -17.41f, 0);
        leftArmTrans.localPosition = new Vector3(9.73f, -17.49f, 0);

        //Make face invisible
        eyes.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        nose.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        mouth.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rightArmTrans = animator.transform.Find("PlayerArms").transform.Find("rightArmParent");
        leftArmTrans = animator.transform.Find("PlayerArms").transform.Find("leftArmParent");
 
        rightArmTrans.localPosition = new Vector3(9.693272f, -17.54705f, 0);
        leftArmTrans.localPosition = new Vector3(14.89107f, -17.56166f, 0);

        eyes.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        nose.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        mouth.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }

}
