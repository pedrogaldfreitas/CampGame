using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charAnimatorSpritePick : StateMachineBehaviour
{
    GameObject player;

    public string rightArmPos;
    public string leftArmPos;
    public string headPos;
    public string mouthPos;
    public string nosePos;
    public string eyesPos;
    public string legsPos;
    public string torsoPos;

    /*Animation milesDefaultRightArmForwardIdle;
    Animation milesDefaultLeftArmForwardIdle;
    Animation milesDefaultTorsoForwardIdle;
    Animation milesDefaultMouthForwardIdle;
    Animation milesDefaultEyesForwardIdle;
    Animation milesDefaultNoseForwardIdle;
    Animation milesDefaultLegsForwardIdle;
    Animation milesDefaultHeadForwardIdle;

    Animation milesDefaultRightArmRightIdle;
    Animation milesDefaultLeftArmForwardIdle;
    Animation milesDefaultTorsoForwardIdle;
    Animation milesDefaultMouthForwardIdle;
    Animation milesDefaultEyesForwardIdle;
    Animation milesDefaultNoseForwardIdle;
    Animation milesDefaultLegsForwardIdle;
    Animation milesDefaultHeadForwardIdle;*/

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.gameObject;

        rightArmPos = player.GetComponent<characterStats>().rightArmPos;
        leftArmPos = player.GetComponent<characterStats>().leftArmPos;
        headPos = player.GetComponent<characterStats>().headPos;
        mouthPos = player.GetComponent<characterStats>().mouthPos;
        nosePos = player.GetComponent<characterStats>().nosePos;
        eyesPos = player.GetComponent<characterStats>().eyesPos;
        legsPos = player.GetComponent<characterStats>().legsPos;
        torsoPos = player.GetComponent<characterStats>().torsoPos;

        if (rightArmPos == "DEFAULT")
        {
            
        }
    }

}
