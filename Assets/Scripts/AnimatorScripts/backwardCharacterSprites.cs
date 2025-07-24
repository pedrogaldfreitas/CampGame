using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backwardCharacterSprites : StateMachineBehaviour
{
    Transform rightArmTrans;
    Transform leftArmTrans;
    Transform rightSleeve;
    Transform leftSleeve;
    GameObject eyes;
    GameObject nose;
    GameObject mouth;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rightArmTrans = animator.transform.Find("Arms").transform.Find("RightArmParent");
        leftArmTrans = animator.transform.Find("Arms").transform.Find("LeftArmParent");
        rightSleeve = rightArmTrans.Find("RightSleeveParent");
        leftSleeve = leftArmTrans.Find("LeftSleeveParent");
        eyes = animator.transform.Find("EyesParent").Find("Eyes").gameObject;
        nose = animator.transform.Find("NoseParent").Find("Nose").gameObject;
        mouth = animator.transform.Find("MouthParent").Find("Mouth").gameObject;

        //rightArmTrans.localPosition = new Vector3(15.02f, -17.41f, 0);
        rightArmTrans.localPosition = new Vector3(14.004f, -20.94915f, 0.6f);
        rightArmTrans.localScale = new Vector3(2.26892f, 2.26892f, 1);
        rightSleeve.localPosition = new Vector3(-0.01895f, 0.86295f, -0.1f);
        rightSleeve.localScale = new Vector3(1, 1, 1);

        leftArmTrans.localPosition = new Vector3(10.399f, -21.226f, 0.6f);
        leftArmTrans.localScale = new Vector3(2.26892f, 2.26892f, 1);
        leftSleeve.localPosition = new Vector3(0.21993f, 0.96498f, -0.1f); ;
        leftSleeve.localScale = new Vector3(-1, 1, 1);

        //Make face invisible
        eyes.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        nose.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        mouth.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rightArmTrans = animator.transform.Find("Arms").transform.Find("RightArmParent");
        leftArmTrans = animator.transform.Find("Arms").transform.Find("LeftArmParent");
 
        rightArmTrans.localPosition = new Vector3(9.51f, -17.38f, 0.6f);
        leftArmTrans.localPosition = new Vector3(14.89107f, -17.56166f, 0.6f);
        leftSleeve.localPosition = new Vector3(-0.4102f, -0.628f, -0.1f);
        leftSleeve.localScale = new Vector3(1, 1, 1);
        rightSleeve.localPosition = new Vector3(0.61170f, -0.7300006f, -0.1f);
        rightSleeve.localScale = new Vector3(-1, 1, 1);

        eyes.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        nose.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        mouth.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }

}
