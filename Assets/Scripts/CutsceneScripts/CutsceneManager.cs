using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static StateManager;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField]
    public string cutsceneID;

    public bool cutsceneActive;
    private GameObject player;
    private GameObject playerParent;

    //Fade in/out variables
    private Color fadeColor;
    private Image blackScreen;

    private DialogueManager dialogueManager;
    private DialogueFeed allDiagOptions;

    //Chat bubble related variables.
    RectTransform chatBubble;

    private void Start()
    {
        blackScreen = GameObject.Find("BlackScreen")?.GetComponent<Image>();
        cutsceneActive = false;
        playerParent = GameObject.Find("Player");
        player = playerParent.transform.Find("Body").gameObject;
        dialogueManager = GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>();
        allDiagOptions = GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>();

        chatBubble = GameObject.Find("ChatBubbleImage").GetComponent<RectTransform>();

        //StartCoroutine(testCutscene());
        //playCutsceneUsingID("D1M0");
        //StartCoroutine(DayTitles(1));

        //playCutsceneUsingID("TEST");
    }


    #region Cutscenes

    IEnumerator TEST()
    {
        float waitTime = 0.3f;
        while (true)
        {
            yield return StartCoroutine(WalkDown(GameObject.Find("Victor"), 15, -277f));

            yield return StartCoroutine(faceDirection(GameObject.Find("Victor"), "UP"));
            yield return StartCoroutine(wait(waitTime));
            yield return StartCoroutine(faceDirection(GameObject.Find("Victor"), "RIGHT"));
            yield return StartCoroutine(wait(waitTime));
            yield return StartCoroutine(faceDirection(GameObject.Find("Victor"), "DOWN"));
            yield return StartCoroutine(wait(waitTime));
            yield return StartCoroutine(faceDirection(GameObject.Find("Victor"), "LEFT"));
            yield return StartCoroutine(wait(waitTime));

            yield return StartCoroutine(WalkLeft(GameObject.Find("Victor"), 15, -352));

            yield return StartCoroutine(faceDirection(GameObject.Find("Victor"), "UP"));
            yield return StartCoroutine(wait(waitTime));
            yield return StartCoroutine(faceDirection(GameObject.Find("Victor"), "RIGHT"));
            yield return StartCoroutine(wait(waitTime));
            yield return StartCoroutine(faceDirection(GameObject.Find("Victor"), "DOWN"));
            yield return StartCoroutine(wait(waitTime));
            yield return StartCoroutine(faceDirection(GameObject.Find("Victor"), "LEFT"));
            yield return StartCoroutine(wait(waitTime));

            yield return StartCoroutine(WalkUp(GameObject.Find("Victor"), 15, -247));

            yield return StartCoroutine(faceDirection(GameObject.Find("Victor"), "UP"));
            yield return StartCoroutine(wait(waitTime));
            yield return StartCoroutine(faceDirection(GameObject.Find("Victor"), "RIGHT"));
            yield return StartCoroutine(wait(waitTime));
            yield return StartCoroutine(faceDirection(GameObject.Find("Victor"), "DOWN"));
            yield return StartCoroutine(wait(waitTime));
            yield return StartCoroutine(faceDirection(GameObject.Find("Victor"), "LEFT"));
            yield return StartCoroutine(wait(waitTime));

            yield return StartCoroutine(WalkRight(GameObject.Find("Victor"), 15, -322));

            yield return StartCoroutine(faceDirection(GameObject.Find("Victor"), "UP"));
            yield return StartCoroutine(wait(waitTime));
            yield return StartCoroutine(faceDirection(GameObject.Find("Victor"), "RIGHT"));
            yield return StartCoroutine(wait(waitTime));
            yield return StartCoroutine(faceDirection(GameObject.Find("Victor"), "DOWN"));
            yield return StartCoroutine(wait(waitTime));
            yield return StartCoroutine(faceDirection(GameObject.Find("Victor"), "LEFT"));
            yield return StartCoroutine(wait(waitTime));
        }
    }


    IEnumerator NEWDIAGBOXTEST()
    {
        playerParent.GetComponent<PlayerController>().disableMovement();
        cutsceneActive = true;
        StartCoroutine(OpenChatBubble(Color.white));

        yield return StartCoroutine(dialogue(allDiagOptions.testDiag[0]));
        yield return StartCoroutine(dialogue(allDiagOptions.testDiag[1]));

        StartCoroutine(CloseChatBubble());
        cutsceneActive = false;
        playerParent.GetComponent<PlayerController>().enableMovement();
    }

    //Meaning: Day1Morning0 (Coming off dock onto shore)
    IEnumerator D1M0()
    {
        playerParent.GetComponent<PlayerController>().disableMovement();
        cutsceneActive = true;

        yield return StartCoroutine(faceDirection(player, "UP"));
        changeArmsAndLegs(GameObject.Find("NPCWally"), "", "", "SITTING");

        //yield return StartCoroutine(dialogue(allDiagOptions.diagV1M0));
        changeFacialExpression(GameObject.Find("NPCVictor"), "", "", "SMILETALK");
        changeFacialExpression(player, "STRANGE", "", "");
        yield return StartCoroutine(faceDirection(player, "DOWN"));

        //yield return StartCoroutine(dialogue(allDiagOptions.diagV1M1));
        changeFacialExpression(player, "DEFAULT", "", "");
        yield return StartCoroutine(faceDirection(player, "UP"));

        //yield return StartCoroutine(dialogue(allDiagOptions.diagV1M2));

        //StartCoroutine(WalkLeft(GameObject.Find("NPCVictor"), 0, 100));
        StartCoroutine(WalkRight(GameObject.Find("NPCVictor"), 40, 110));
        yield return wait(3);
        GameObject.Find("MissionManager").GetComponent<missionManager>().setMission("D1M1");
        GameObject.Find("NPCVictor").transform.position = new Vector2(99999, 99999);

        cutsceneActive = false;
        //UpdateMissionState(DayOne.Objective.GOTOOPENINGCAMPFIRE);
        playerParent.GetComponent<PlayerController>().enableMovement();
        //DayOne.Morning.spokeToVicAfterBoat = true;
    }

    //Meaning: Day1Morning2 (Sitting on the log in front of campfire, being tasked with finding girl.)
    IEnumerator D1M2()
    {
        cutsceneActive = true;
        playerParent.GetComponent<PlayerController>().disableMovement();

        player.GetComponent<Animator>().SetFloat("Horizontal", 0);
        player.GetComponent<Animator>().SetFloat("Vertical", 0);
        yield return StartCoroutine(wait(0.01f));

        //yield return WalkRight(player, 20, GameObject.Find("CampfireSeat1").transform.position.x);
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagD1M_LOG1));

        yield return StartCoroutine(faceDirection(player, "UP"));
        GameObject.Find("VictorParent").transform.position = new Vector2(-227f, -103.9f);
        yield return StartCoroutine(WalkDown(GameObject.Find("NPCVictor"), 20, -157.6f));
        //yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagV1M1));
        yield return StartCoroutine(wait(2f));

        //yield return StartCoroutine(faceDirection(player, "DOWN"));

        //If player is to the right/left of log position, or above/below it.
        Debug.Log("PlayerTransform position: " + player.transform.parent.position + " log position: " + GameObject.Find("CampfireSeat1").transform.position);
        if (player.transform.parent.position.x > GameObject.Find("seat6").transform.position.x)
        {
            if (player.transform.parent.position.y < GameObject.Find("seat6").transform.position.y)
            {
                StartCoroutine(multipleCoroutines(WalkLeft(player, 20, GameObject.Find("seat6").transform.position.x), multipleCoroutines(WalkUp(player, 20, GameObject.Find("seat6").transform.position.y + 5), faceDirection(player, "DOWN"))));
            } else if (player.transform.parent.position.y > GameObject.Find("seat6").transform.position.y)
            {
                StartCoroutine(multipleCoroutines(WalkLeft(player, 20, GameObject.Find("seat6").transform.position.x), multipleCoroutines(WalkDown(player, 20, GameObject.Find("seat6").transform.position.y + 5), faceDirection(player, "DOWN"))));
            }
        }
        else if (player.transform.parent.position.x < GameObject.Find("seat6").transform.position.x)
        {
            if (player.transform.parent.position.y < GameObject.Find("seat6").transform.position.y)
            {
                StartCoroutine(multipleCoroutines(WalkRight(player, 20, GameObject.Find("seat6").transform.position.x), multipleCoroutines(WalkUp(player, 20, GameObject.Find("seat6").transform.position.y + 5), faceDirection(player, "DOWN"))));
            }
            else if (player.transform.parent.position.y > GameObject.Find("seat6").transform.position.y)
            {
                StartCoroutine(multipleCoroutines(WalkRight(player, 20, GameObject.Find("seat6").transform.position.x), multipleCoroutines(WalkDown(player, 20, GameObject.Find("seat6").transform.position.y+5), faceDirection(player, "DOWN"))));
            }
        }

        yield return StartCoroutine(multipleCoroutines(WalkRight(GameObject.Find("NPCVictor"), 20, -212.6f), WalkDown(GameObject.Find("NPCVictor"), 20, -175.5f)));

        /*
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagV1M2));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagW1M0));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagV1M3));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagW1M1));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagV1M4));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagW1M2));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagV1M5));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagW1M3));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagV1M6));*/

        //GameObject.Find("NPCElijah").transform.position = new Vector2(player.transform.position.x, player.transform.position.y - 70);
       // yield return StartCoroutine(WalkUp(GameObject.Find("NPCElijah"), 20, player.transform.position.y - 15));
       /*
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagE1M0));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagV1M7));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagE1M1));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagV1M8));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagW1M4));*/

        GameObject.Find("MissionManager").GetComponent<missionManager>().setMission("D1M2");

        playerParent.GetComponent<PlayerController>().enableMovement();
        cutsceneActive = false;
    }

    IEnumerator D1ACORNSTEPWARNING(GameObject acorn)
    {
        cutsceneActive = true;
        playerParent.GetComponent<PlayerController>().disableMovement();

        GameObject squirrel1 = acorn.GetComponent<AcornScript>().squirrel1;
        GameObject squirrel2 = acorn.GetComponent<AcornScript>().squirrel2;
        GameObject squirrel3 = acorn.GetComponent<AcornScript>().squirrel3;

        if (squirrel1 != null)
        {
            StartCoroutine(faceDirection(player, "RIGHT"));
            //squirrel 1 growl
            if (squirrel2 != null)
            {
                //squirrel 2 growl
            }
            if (squirrel3 != null)
            {
                //squirrel 3 growl
            }
            yield return StartCoroutine(WalkLeft(player, 10, player.transform.position.x - 10));
            yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagD1_FIRSTACORN));
           
        }

        yield return new WaitForEndOfFrame();

        playerParent.GetComponent<PlayerController>().enableMovement();
        cutsceneActive = false;
    }

    IEnumerator D1FIRSTSTICKDROP()
    {
        GameObject firstStickParent = GameObject.Find("FirstStickParent");
        firstStickParent.transform.position = new Vector2(player.transform.position.x + 8, player.transform.position.y);
        firstStickParent.GetComponent<FakeHeightObject>().isGrounded = false;
        while (firstStickParent.GetComponent<FakeHeightObject>().height > 20)
        {
            yield return new WaitForEndOfFrame();
        }

        cutsceneActive = true;
        playerParent.GetComponent<PlayerController>().disableMovement();

        firstStickParent.transform.position = GameObject.Find("Player").transform.Find("Shadow").transform.position;

        yield return StartCoroutine(wait(0.01f));
        yield return StartCoroutine(faceDirection(player, "DOWN"));

        changeArmsAndLegs(player, "STICKHEADBONK", "STICKHEADBONK");
        changeFacialExpression(player, "STICKHEADBONK", "STICKHEADBONK", "STICKHEADBONK");
        changeHead(player, "STICKHEADBONK");
        firstStickParent.GetComponent<FakeHeightObject>().Rise(7.51f);
        firstStickParent.GetComponent<FakeHeightObject>().Jump(new Vector2(15f, 0f), 40f, true, new Vector3(0, 0, -13f), default(Quaternion));
        yield return new WaitForSeconds(3);
        changeArmsAndLegs(player, "DEFAULT", "DEFAULT");
        changeFacialExpression(player, "DEFAULT", "DEFAULT", "DEFAULT");
        changeHead(player, "DEFAULT");

        playerParent.GetComponent<PlayerController>().enableMovement();
        cutsceneActive = false;
    }

    //Talking to girl after rescuing her.
    IEnumerator D1M3 ()
    {
        cutsceneActive = true;
        playerParent.GetComponent<PlayerController>().disableMovement();

        player.GetComponent<Animator>().SetFloat("Horizontal", -1);
        player.GetComponent<Animator>().SetFloat("Vertical", 0);
        yield return faceDirection(player, "LEFT");

        Animator anim = player.GetComponent<Animator>();
        //anim["milesTakenAback"].layer = ;

        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M0));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M1));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M2));
        anim.Play("milesTakenAback");
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M3));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M4));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M5));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M6));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M7));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M8));

        CoroutineWithData cr = new CoroutineWithData(this, dialogueManager.DialogueOptions(2, 0.5f, "My... Spine!", "T-thanks!"));
        yield return cr.coroutine;
       
        switch (cr.result)
        {
            case 1:
                yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M8_1_1));
                break;
            case 2:
                yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M8_2_1));
                break;
        }

        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M9));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M10));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M11));
        
        cr = new CoroutineWithData(this, dialogueManager.DialogueOptions(4, 0.5f, "Uhh... Yes?", "I have to pee.", "We're too young to think about this!", "...Not age, no!"));
        yield return cr.coroutine;

        switch (cr.result)
        {
            case 1:
                yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M11_1_1));
                yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M11_1_2));
                break;
            case 2:
                yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M11_2_1));
                yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M11_2_2));
                break;
            case 3:
                yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M11_3_1));
                yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M11_3_2)); 
                break;
            case 4:
                yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M11_4_1));
                yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M11_4_2));
                yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M11_4_3));
                break;
        }

        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M12));
        yield return StartCoroutine(dialogue(GameObject.Find("AllDiagOptions").GetComponent<DialogueFeed>().diagG1M13));


        // int diagResponse = yield return StartCoroutine(dialogueManager.DialogueOptions(2, 0.5f, "My... Spine!", "T-thanks!"));
        //int 
        //yield return StartCoroutine(dialogueManager.DialogueOptions(2, 0.5f, "NO!", "M-my spine..."));

        playerParent.GetComponent<PlayerController>().enableMovement();
        cutsceneActive = false;
    }

    IEnumerator testCutscene()
    {
        cutsceneActive = true;
        playerParent.GetComponent<PlayerController>().disableMovement();

        yield return StartCoroutine(WalkUp(GameObject.Find("Player"), 30, -230));
        yield return StartCoroutine(WalkLeft(GameObject.Find("Player"), 30, 95));
        yield return StartCoroutine(WalkDown(GameObject.Find("Player"), 30, -250));
        yield return StartCoroutine(WalkRight(GameObject.Find("Player"), 30, 130));

        playerParent.GetComponent<PlayerController>().enableMovement();
        cutsceneActive = false;
    }

    #endregion Cutscenes

    #region Cutscene Methods

    public void playCutsceneUsingID(string cutscene)
    {
        StartCoroutine(cutscene);
    }

    //WARNING: Time values should be at 0.01f and below. Otherwise, use SlowFadeIn.
    public IEnumerator FastFadeIn(float time)
    {
        for (float i = 0; i <= 1; i += 0.05f)
        {
            //fadeColor = blackScreen.GetComponent<SpriteRenderer>().color;
            fadeColor = blackScreen.color;
            fadeColor.a = i;
            blackScreen.color = fadeColor;
           // GameObject.Find("BlackScreen").GetComponent<SpriteRenderer>().color = fadeColor;
            yield return new WaitForSeconds(time);
        }
        fadeColor.a = 1;
        blackScreen.color = fadeColor;
    }

    //WARNING: Time values should be above 0.01f. Otherwise, use FastFadeIn.
    public IEnumerator SlowFadeIn(float time)
    {
        for (float i = 0; i <= 1; i += 0.005f)
        {
            fadeColor = blackScreen.color;
            fadeColor.a = i;
            blackScreen.color = fadeColor;
            yield return new WaitForSeconds(time);
        }
        fadeColor.a = 1;
        blackScreen.color = fadeColor;
    }

    //WARNING: Time values should be at 0.01f and below. Otherwise, use SlowFadeOut.
    public IEnumerator FastFadeOut(float time)
    {
        for (float i = 1; i >= 0; i -= 0.05f)
        {
            fadeColor = blackScreen.color;
            fadeColor.a = i;
            blackScreen.color = fadeColor;
            yield return new WaitForSeconds(time);
        }
        fadeColor.a = 0;
        blackScreen.color = fadeColor;
    }

    //WARNING: Time values should be above 0.01f. Otherwise, use FastFadeOut.
    public IEnumerator SlowFadeOut(float time)
    {
        for (float i = 1; i >= 0; i -= 0.005f)
        {
            fadeColor = blackScreen.color;
            fadeColor.a = i;
            blackScreen.color = fadeColor;
            yield return new WaitForSeconds(time);
        }
        fadeColor.a = 0;
        blackScreen.color = fadeColor;
    }

    public IEnumerator ShowMissionText(string missionText)
    {
        TextMeshProUGUI missionTitleText = GameObject.Find("UI").transform.Find("MissionTitleText").GetComponent<TextMeshProUGUI>();
        missionTitleText.text = missionText;

        for (float i = 0; i <= 1; i += 0.05f)
        {
            fadeColor = missionTitleText.color;
            fadeColor.a = i;
            missionTitleText.color = fadeColor;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(5f);

        for (float i = 1; i >= 0; i -= 0.05f)
        {
            fadeColor = missionTitleText.color;
            fadeColor.a = i;
            missionTitleText.color = fadeColor;
            yield return new WaitForSeconds(0.01f);
        }

        missionTitleText.text = "";
    }

    IEnumerator WalkUp(GameObject character, float speed, float yPos)
    {
        Transform parentOfChar = character.transform.parent;
        Transform landTargetTransform = parentOfChar.Find("LandTarget");
        Rigidbody2D parentRB = parentOfChar.GetComponent<Rigidbody2D>();
        character.GetComponent<Animator>().SetFloat("Vertical", 2);

        while (landTargetTransform.position.y < yPos)
        {
            parentRB.MovePosition(new Vector3(parentOfChar.position.x, parentOfChar.position.y + (speed * Time.deltaTime)));
            if (landTargetTransform.position.y >= yPos)
            {
                character.GetComponent<Animator>().SetFloat("Vertical", 0);
            }
            yield return new WaitForEndOfFrame();
        }

    }

    IEnumerator WalkDown(GameObject character, float speed, float yPos)
    {
        Transform parentOfChar = character.transform.parent;
        Transform landTargetTransform = parentOfChar.Find("LandTarget");
        Rigidbody2D parentRB = parentOfChar.GetComponent<Rigidbody2D>();
        character.GetComponent<Animator>().SetFloat("Vertical", -2);

        while (landTargetTransform.position.y > yPos)
        {
            parentRB.MovePosition(new Vector3(parentOfChar.position.x, parentOfChar.position.y - (speed * Time.deltaTime)));
            if (landTargetTransform.position.y <= yPos)
            {
                character.GetComponent<Animator>().SetFloat("Vertical", 0);
            }
            yield return new WaitForEndOfFrame();
        }

    }

    IEnumerator WalkRight(GameObject character, float speed, float xPos)
    {
        Transform parentOfChar = character.transform.parent;
        Transform landTargetTransform = parentOfChar.Find("LandTarget");
        Rigidbody2D parentRB = parentOfChar.GetComponent<Rigidbody2D>();
        character.GetComponent<Animator>().SetFloat("Horizontal", 2);

        while (landTargetTransform.position.x < xPos)
        {
            parentRB.MovePosition(new Vector3(parentOfChar.position.x + (speed * Time.deltaTime), parentOfChar.position.y));
            if (landTargetTransform.position.x >= xPos)
            {
                character.GetComponent<Animator>().SetFloat("Horizontal", 0);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator WalkLeft(GameObject character, float speed, float xPos, string facingDirection = "LEFT")
    {
        Transform parentOfChar = character.transform.parent;
        Transform landTargetTransform = parentOfChar.Find("LandTarget");
        Rigidbody2D parentRB = parentOfChar.GetComponent<Rigidbody2D>();

        if (facingDirection == "LEFT")
        {
            character.GetComponent<Animator>().SetFloat("Horizontal", -2);
        } else if (facingDirection == "RIGHT")
        {
            character.GetComponent<Animator>().SetFloat("Horizontal", 2);
        }


        while (landTargetTransform.position.x > xPos)
        {
            parentRB.MovePosition(new Vector3(parentOfChar.position.x - (speed * Time.deltaTime), parentOfChar.position.y));
            if (landTargetTransform.position.x <= xPos)
            {
                character.GetComponent<Animator>().SetFloat("Horizontal", 0);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator faceDirection(GameObject character, string dir)
    {
        character.GetComponent<Animator>().SetFloat("Horizontal", 0);
        character.GetComponent<Animator>().SetFloat("Vertical", 0);
        yield return StartCoroutine(wait(0.01f));
        if (dir == "UP")
        {
            character.GetComponent<Animator>().SetFloat("Vertical", 1);
            yield return new WaitForEndOfFrame();
            character.GetComponent<Animator>().SetFloat("Vertical", 0);
        }
        if (dir == "DOWN")
        {
            character.GetComponent<Animator>().SetFloat("Vertical", -1);
            yield return new WaitForEndOfFrame();
            character.GetComponent<Animator>().SetFloat("Vertical", 0);
        }
        if (dir == "RIGHT")
        {
            character.GetComponent<Animator>().SetFloat("Horizontal", 1);
            yield return new WaitForEndOfFrame();
            character.GetComponent<Animator>().SetFloat("Horizontal", 0);
        }
        if (dir == "LEFT")
        {
            character.GetComponent<Animator>().SetFloat("Horizontal", -1);
            yield return new WaitForEndOfFrame();
            character.GetComponent<Animator>().SetFloat("Horizontal", 0);
        }
           
        

    }

    //For string parameters, type 'curr' if you do not want to change it.
    public void changeFacialExpression(GameObject characterObj, string eyesSprite = "", string noseSprite = "", string mouthSprite = "")
    {
        if (eyesSprite != "")
        {
            characterObj.GetComponent<characterStats>().changeCharacterSprite("eyes", eyesSprite);
        }
        if (noseSprite != "")
        {
            characterObj.GetComponent<characterStats>().changeCharacterSprite("nose", noseSprite);
        }
        if (mouthSprite != "")
        {
            characterObj.GetComponent<characterStats>().changeCharacterSprite("mouth", mouthSprite);
        }

        return;
    }

    //For string parameters, type nothing if you don't want to change it.
    public void changeArmsAndLegs(GameObject characterObj, string leftArmSprite = "", string rightArmSprite = "", string legsSprite = "")
    {
        if (leftArmSprite != "")
        {
            characterObj.GetComponent<characterStats>().changeCharacterSprite("leftarm", leftArmSprite);
        }
        if (rightArmSprite != "")
        {
            characterObj.GetComponent<characterStats>().changeCharacterSprite("rightarm", rightArmSprite);
        }
        if (legsSprite != "")
        {
            characterObj.GetComponent<characterStats>().changeCharacterSprite("legs", legsSprite);
        }

        return;
    }

    public void changeTorso(GameObject characterObj, string torsoSprite = "")
    {
        if (torsoSprite != "")
        {
            characterObj.GetComponent<characterStats>().changeCharacterSprite("torso", torsoSprite);
        }

        return;
    }

    public void changeHead(GameObject characterObj, string headSprite = "")
    {
        if (headSprite != "")
        {
            characterObj.GetComponent<characterStats>().changeCharacterSprite("head", headSprite);
        }

        return;
    }

    IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

    IEnumerator OpenChatBubble(Color color)
    {
        StopCoroutine("OpenChatBubble");
        StopCoroutine("CloseChatBubble");

        Image chatBubbleImage = chatBubble.GetComponent<Image>();

        chatBubble.localScale = new Vector3(0.9f, 0.9f, 1);
        chatBubbleImage.color = new Color(color.r, color.g, color.b, 0);

        while (chatBubble.localScale.x < 0.99f || chatBubbleImage.color.a < 0.99f)
        {
            chatBubble.localScale = new Vector3(Mathf.Lerp(chatBubble.localScale.x, 1, 0.4f), Mathf.Lerp(chatBubble.localScale.y, 1, 0.4f), 1);
            chatBubbleImage.color += new Color(0, 0, 0, 0.2f);

            yield return new WaitForSeconds(0.01f);
        }

        chatBubble.localScale = new Vector3(1, 1, 1);
        chatBubbleImage.color = new Color(color.r, color.g, color.b, 1);
    }

    IEnumerator CloseChatBubble()
    {
        StopCoroutine("OpenChatBubble");
        StopCoroutine("CloseChatBubble");

        Image chatBubbleImage = chatBubble.GetComponent<Image>();

        while (chatBubble.localScale.x > 0.901f || chatBubbleImage.color.a > 0.01f)
        {
            chatBubble.localScale = new Vector3(Mathf.Lerp(chatBubble.localScale.x, 0.9f, 0.4f), Mathf.Lerp(chatBubble.localScale.y, 0.9f, 0.4f), 1);
            chatBubbleImage.color -= new Color(0, 0, 0, 0.2f);

            yield return new WaitForSeconds(0.01f);
        }

        chatBubble.localScale = new Vector3(0.9f, 0.9f, 1);
        chatBubbleImage.color = new Color(chatBubbleImage.color.r, chatBubbleImage.color.g, chatBubbleImage.color.b, 0);
    }

    IEnumerator dialogue(string diag)
    {
        dialogueManager.StartDialogue(diag);
        while (GameObject.Find("Player").GetComponent<DialogueTrigger>().dialogueActive == true)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    /*void UpdateMissionState(DayOne.Objective objective)
    {
        GameObject.Find("MissionProgressionPoints").GetComponent<setMissionProgressionPoints>().setMissionStateDay1(objective);
    }*/

    IEnumerator multipleCoroutines(IEnumerator coroutine1, IEnumerator coroutine2)
    {
        yield return StartCoroutine(coroutine1);
        yield return StartCoroutine(coroutine2);
    }
    IEnumerator DayTitles(int DayNumber)
    {
        switch (DayNumber)
        {
            case 1:
                GameObject.Find("DayText").GetComponent<TextMeshProUGUI>().text = "DAY ONE: An Uneasy Welcome";
                break;
            case 2:
                GameObject.Find("DayText").GetComponent<TextMeshProUGUI>().text = "DAY TWO: The Boys Vs. The World";
                break;
            case 3:
                GameObject.Find("DayText").GetComponent<TextMeshProUGUI>().text = "DAY THREE: Naomi Breaks Even";
                break;
            case 4:
                GameObject.Find("DayText").GetComponent<TextMeshProUGUI>().text = "DAY FOUR: Firefly Noir";
                break;
            case 5:
                GameObject.Find("DayText").GetComponent<TextMeshProUGUI>().text = "DAY FIVE: Seaweed Cracks Under Pressure";
                break;
            case 6:
                GameObject.Find("DayText").GetComponent<TextMeshProUGUI>().text = "DAY SIX: Miles' Awakening";
                break;
            case 7:
                GameObject.Find("DayText").GetComponent<TextMeshProUGUI>().text = "DAY SEVEN: Harrowingly Unearthed";
                break;
            case 8:
                GameObject.Find("DayText").GetComponent<TextMeshProUGUI>().text = "DAY EIGHT: Forever in Their Hearts";
                break;
        }

        for (float i = 0; i <= 1; i += 0.02f)
        {
            fadeColor = GameObject.Find("DayText").GetComponent<TextMeshProUGUI>().color;
            fadeColor.a = i;
            GameObject.Find("DayText").GetComponent<TextMeshProUGUI>().color = fadeColor;
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(5f);

        for (float i = 1; i >= 0; i -= 0.05f)
        {
            fadeColor = GameObject.Find("DayText").GetComponent<TextMeshProUGUI>().color;
            fadeColor.a = i;
            GameObject.Find("DayText").GetComponent<TextMeshProUGUI>().color = fadeColor;
            yield return new WaitForSeconds(0.02f);
        }
        GameObject.Find("DayText").GetComponent<TextMeshProUGUI>().color -= new Color(0, 0, 0, 1);
    }

    #endregion Cutscene Methods

}
