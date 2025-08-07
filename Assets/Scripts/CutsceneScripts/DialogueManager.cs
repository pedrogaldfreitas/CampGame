using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class DialogueManager : MonoBehaviour
{

    private Queue<string> sentences;
    public TextMeshProUGUI dialogueText;

    [SerializeField]
    private Animator speechBubbleAnim;

    private int sentenceCount = 0;
    private string diag;

    [SerializeField]
    private float typeSentenceSpeed;

    //Dialogue choices
    public bool diagChoicePickerActive;
    public int diagChoice;

    //Objects
    public GameObject diagArrow;
    public GameObject diagOptionDivider;
    public GameObject diagTimer;
    public GameObject continueButton;

    private const string REMAINDER_REGEX = "(.*?((?=>)|(/|$)))";
    private const string ANIM_START_REGEX_STRING = "<anim:(?<anim>" + REMAINDER_REGEX + ")>";
    private static readonly Regex animStartRegex = new Regex(ANIM_START_REGEX_STRING);
    private const string ANIM_END_REGEX_STRING = "</anim>";
    private static readonly Regex animEndRegex = new Regex(ANIM_END_REGEX_STRING);

    void Start()
    {

        //Debug.Log(animStartRegex);
        //Debug.Log(animEndRegex);

        diagOptionDivider.SetActive(false);
        diagArrow.SetActive(false);
        diagTimer.SetActive(false);
        continueButton.SetActive(false);

        sentences = new Queue<string>();

        //StartCoroutine(DialogueOptions(4, 0.5f, "Poop", "Diag option 2", "Diag Option 3", "Diag Option 4"));
    }

    private void Update()
    {
        //dialogueText.ForceMeshUpdate();

        if(Input.GetKeyDown(KeyCode.A) == true && continueButton.activeSelf)
        {
            EndDialogue();
        }

    }

    public void StartDialogue(string dialogue)
    {
        diag = dialogue;
        GameObject.Find("Player").GetComponent<DialogueTrigger>().dialogueActive = true;

        StartCoroutine(TypeDialogue());

    }

    IEnumerator TypeDialogue()
    {
        TMP_TextInfo textInfo = dialogueText.textInfo; 
        dialogueText.text = diag;

        //PURELY FOR TESTING:
        //StartCoroutine(WobbleText(5, 12, 15f, 0.05f));

        int charIterator = 1;
        foreach (char letter in diag)
        {

            //TODO: Must find a way to iteratively place character-by-character on screen

            string visibleText = diag.Substring(0, charIterator);

            visibleText += "<color=#00000000>" + diag.Substring(charIterator) + "</color>";
            dialogueText.text = visibleText;



            charIterator++;

            if (Input.GetKey(KeyCode.A) == true)
            {
                yield return new WaitForSeconds(0.02f * typeSentenceSpeed);
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
        continueButton.SetActive(true);
    }

    public void EndDialogue()
    {
        dialogueText.text = "";
        continueButton.SetActive(false);
        GameObject.Find("Player").GetComponent<DialogueTrigger>().dialogueActive = false;
    }

    public IEnumerator WobbleText(int startIndex = 0, int endIndex = 0, float speed = 11.8f, float intensity = 0.01f)
    {
        Mesh mesh;
        Vector3[] vertices;

        while (true)
        {
            dialogueText.ForceMeshUpdate();
            mesh = dialogueText.mesh;
            vertices = mesh.vertices;

            /*for (int i = 0; i < dialogueText.textInfo.characterCount; i++)
            {
                TMP_CharacterInfo c = dialogueText.textInfo.characterInfo[i];
                int index = c.vertexIndex;
                Vector3 offset = Wobble(Time.time + i);
                vertices[index] += offset;
                vertices[index + 1] += offset;
                vertices[index + 2] += offset;
                vertices[index + 3] += offset;
            }*/

            for (int i = startIndex*4; i < endIndex*4; i++)//vertices.Length; i++)
            {
                Vector3 offset = LightWobble(Time.time + i, speed, intensity);
                vertices[i] = vertices[i] + offset;
            }
            mesh.SetVertices(vertices);

            dialogueText.canvasRenderer.SetMesh(mesh);

            yield return new WaitForEndOfFrame();
        }
    }

    //Default: Speed = 11.8f, intensity = 0.01f
    private Vector2 LightWobble(float time, float speed, float intensity)
    {
        return new Vector2(intensity * Mathf.Sin(time*(speed-0.5f)), intensity * Mathf.Cos(time* speed));
    }


    //****      'amount' is the amount of dialogue options there are. 'timerSpeed' is the speed the timer should go down per frame. Timer counts down from 100 to 0.

    public IEnumerator DialogueOptions(int amount, float timerSpeed, string option1, string option2, string option3 = "", string option4 = "" )
    {
        int option1resistance = 0;
        int option2resistance = 3;
        int option3resistance = 0;
        int option4resistance = 0;

        //Pressing K to start dialogue would also end the dialogue at the same time. loopCount ensures that a certain amount of loops have passed before K can be pressed again.
        int loopCount = 0;

        // ****** SET UP THE DIALOGUE CHOICES

        if (!GameObject.Find("Player").GetComponent<DialogueTrigger>().dialogueActive)
        {
            speechBubbleAnim.SetTrigger("StartDiag");
        }

        diagOptionDivider.SetActive(true);
        diagTimer.SetActive(true);
        diagTimer.GetComponent<Image>().fillAmount = 1;

        GameObject.Find("Player").GetComponent<PlayerController>().enabled = false;
        
        GameObject diagoption1 = GameObject.Find("Option1Text");
        GameObject diagoption2 = GameObject.Find("Option2Text");
        GameObject diagoption3 = GameObject.Find("Option3Text");
        GameObject diagoption4 = GameObject.Find("Option4Text");
        
        if (amount == 2)
        {
            diagoption1.GetComponent<TextMeshPro>().text = option1;
            diagoption2.GetComponent<TextMeshPro>().text = option2;
        }
        if (amount == 3)
        {
            diagoption1.GetComponent<TextMeshPro>().text = option1;
            diagoption2.GetComponent<TextMeshPro>().text = option2;
            diagoption3.GetComponent<TextMeshPro>().text = option3;
            diagoption3.transform.localPosition = new Vector3(-1.3f, 11.89999f, -500f);
        }
        if (amount == 4)
        {
            diagoption1.GetComponent<TextMeshPro>().text = option1;
            //Debug.Log("OPTION 1: " + diagoption1.GetComponent<TextMeshPro>().text);
            diagoption2.GetComponent<TextMeshPro>().text = option2;
            //Debug.Log("OPTION 2: " + diagoption2.GetComponent<TextMeshPro>().text);
            diagoption3.GetComponent<TextMeshPro>().text = option3;
            //Debug.Log("OPTION 3: " + diagoption3.GetComponent<TextMeshPro>().text);
            //diagoption3.transform.localPosition = new Vector3(-8f, 11.89999f, -500f);
            diagoption4.GetComponent<TextMeshPro>().text = option4;
            //Debug.Log("OPTION 4: " + diagoption4.GetComponent<TextMeshPro>().text);
        }

        //GameObject.Find("diagSelectorPoint").GetComponent<Image>().color = new Color((104f / 255f), (68f / 255f), (68f / 255f), 1f);
        diagArrow.SetActive(true);
        diagArrow.GetComponent<Image>().color = new Color((104f / 255f), (68f / 255f), (68f / 255f), 1f);
        diagChoicePickerActive = true;


        // ****** SETUP DONE


        diagChoice = 1;
        diagArrow.transform.position = GameObject.Find("Option1Text").transform.position;
        StartCoroutine(GameObject.Find("PieTimer").GetComponent<diagTimer>().countdown(timerSpeed));


        while (diagChoicePickerActive == true)
        {
            if (amount >= 2)
            {
                if (diagChoice == 1)
                {
                    if (Input.GetAxisRaw("Horizontal") == 1)
                    {
                        diagArrow.GetComponent<diagSelectorScript>().shiftFocus(2);
                        diagChoice = 2;
                    }
                    if ((Input.GetAxisRaw("Vertical") == -1) && (amount > 2))
                    {
                        diagArrow.GetComponent<diagSelectorScript>().shiftFocus(3);
                        diagChoice = 3;
                    }
                } else if (diagChoice == 2)
                {
                    if (Input.GetAxisRaw("Horizontal") == -1)
                    {
                        diagArrow.GetComponent<diagSelectorScript>().shiftFocus(1);
                        diagChoice = 1;
                    }
                } 
                if (amount == 3)
                {
                    if (diagChoice == 1)
                    {
                        if (Input.GetAxisRaw("Vertical") == -1)
                        {
                            diagArrow.GetComponent<diagSelectorScript>().shiftFocus(3);
                            diagChoice = 3;
                        }
                    }
                    if (diagChoice == 2)
                    {
                        if (Input.GetAxisRaw("Vertical") == -1)
                        {
                            diagArrow.GetComponent<diagSelectorScript>().shiftFocus(3);
                            diagChoice = 3;
                        }
                    }
                    if (diagChoice == 3)
                    {
                        if (Input.GetAxisRaw("Vertical") == 1)
                        {
                            diagArrow.GetComponent<diagSelectorScript>().shiftFocus(1);
                            diagChoice = 1;
                        }
                    }
                }
                if (amount == 4)
                {
                    if (diagChoice == 2)
                    {
                        if (Input.GetAxisRaw("Vertical") == -1)
                        {
                            diagArrow.GetComponent<diagSelectorScript>().shiftFocus(4);
                            diagChoice = 4;
                        }
                        if (Input.GetAxisRaw("Horizontal") == -1)
                        {
                            diagArrow.GetComponent<diagSelectorScript>().shiftFocus(1);

                            diagChoice = 1;
                        }
                    }
                    if (diagChoice == 3)
                    {
                        if (Input.GetAxisRaw("Vertical") == 1)
                        {
                            diagArrow.GetComponent<diagSelectorScript>().shiftFocus(1);
                            diagChoice = 1;
                        }
                        if (Input.GetAxisRaw("Horizontal") == 1)
                        {
                            diagArrow.GetComponent<diagSelectorScript>().shiftFocus(4);
                            diagChoice = 4;
                        }
                    }
                    else if (diagChoice == 4)
                    {
                        if (Input.GetAxisRaw("Horizontal") == -1)
                        {
                            diagArrow.GetComponent<diagSelectorScript>().shiftFocus(3);
                            diagChoice = 3;
                        }
                        if (Input.GetAxisRaw("Vertical") == 1)
                        {
                            diagArrow.GetComponent<diagSelectorScript>().shiftFocus(2);
                            diagChoice = 2;
                        }
                    }
                }
            }
            int returnedValue = 1;

            if ((Input.GetKey(KeyCode.A))&&(loopCount > 10))
            {
                returnedValue = endDiagChoice();
            }
            loopCount++;
            yield return returnedValue;
        }
    }

    public int endDiagChoice()
    {
        diagChoicePickerActive = false;
        
        GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;
        //diagOptionDivider.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 1f);
        //diagArrow.GetComponent<Image>().color -= new Color(0f, 0f, 0f, 1f);
        //diagTimer.GetComponent<Image>().color -= new Color(0, 0, 0, 1);
        diagOptionDivider.SetActive(false);
        diagArrow.SetActive(false);
        diagTimer.SetActive(false);

        speechBubbleAnim.SetTrigger("EndDiag");
        GameObject.Find("Option1Text").GetComponent<TextMeshPro>().text = "";
        GameObject.Find("Option2Text").GetComponent<TextMeshPro>().text = "";
        GameObject.Find("Option3Text").GetComponent<TextMeshPro>().text = "";
        GameObject.Find("Option4Text").GetComponent<TextMeshPro>().text = "";

        //Debug.Log("Dialogue option chosen: " + diagArrow.GetComponent<diagSelectorScript>().highlightedChoice);

        //Returns the highlighted dialogue.
        return diagArrow.GetComponent<diagSelectorScript>().highlightedChoice;
    }

}


public class textColorMod
{
    public string textHex;
    public int[] range;

    textColorMod(string hex, int startIndex, int endIndex)
    {
        textHex = hex;
        range = new int[2] { startIndex, endIndex };
    }
}