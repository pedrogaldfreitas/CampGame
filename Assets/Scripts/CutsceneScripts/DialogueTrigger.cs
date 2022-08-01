using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public string dialogue;
    public bool dialogueActive;
    private GameObject continueButton;


    private void Start()
    {
        dialogueActive = false;
        continueButton = GameObject.Find("ContinueButton");
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if ((col.gameObject.tag == "NPC")&&(col.GetType() == typeof(CircleCollider2D)&&(this.GetComponent<BoxCollider2D>().IsTouching(col) == true)))
        {
            if ((Input.GetKeyDown(KeyCode.K) == true)&&(continueButton.GetComponent<Image>().enabled == false)&&(dialogueActive == false))
            {
                Debug.Log("K key pressed");
                TriggerDialogue(col.gameObject);
            }
        }
    }

    /*NOTE:
      In order to easily find the proper dialogue, create a function in another script that searches for the
      proper dialogue depending on the room, previously mentioned dialogue, previous narrative choices, etc.
         */
    public void TriggerDialogue(GameObject interactObj)
    {
        //IMPORTANT: Get diag0 as a dialogue option out of all the dialogue options in AllDiagOptions.
        if (interactObj.name == "Victor")
        {
            dialogue = GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().getProperDialogueOptionVictor();
        } else if (interactObj.tag == "InteractObject")
        {
            dialogue = GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().getProperDialogueOptionObject(interactObj);
        }

        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
