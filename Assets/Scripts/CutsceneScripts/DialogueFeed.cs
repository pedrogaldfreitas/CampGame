using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueFeed : MonoBehaviour
{

    //Eg.: diag

    //MISC
    public string diagD1_FIRSTACORN;

    // VICTOR/DAY1MORN
    [System.NonSerialized]
    public List<string> diagV1M1 = new List<string>();
    public List<string> diagV1M2 = new List<string>();
    public List<string> diagV1M3 = new List<string>();
    /*public string diagV1M0, diagV1M1, diagV1M2, diagV1M3, diagV1M4, diagV1M5_1_1, diagV1M5_1_2, diagV1M5_1_3, 
        diagV1M5_2_1, diagV1M5_2_2, diagV1M5_2_3, diagV1M5_2_4, diagV1M5, diagV1M6, diagV1M7, diagV1M8, diagV1M9, diagV1M10, 
        diagV1M11, diagV1M12, diagV1M13, diagV1M14, diagV1M15, diagV1M16, diagV1M17;*/

    // WALLY/DAY1MORN
    [System.NonSerialized]
    public string diagW1M0, diagW1M1, diagW1M2, diagW1M3, diagW1M4, diagW1M5, diagW1M6,
        diagW1M7, diagW1M8, diagW1M9, diagW1M10, diagW1M11, diagW1M12, diagW1M13, diagW1M14,
        diagW1M15, diagW1M16, diagW1M17;

    // SEAWEED/DAY1MORN
    [System.NonSerialized]
    public string diagSW1M0, diagSW1M1, diagSW1M2, diagSW1M3, diagSW1M4, diagSW1M5, diagSW1M6,
        diagSW1M7, diagSW1M8, diagSW1M9, diagSW1M10, diagSW1M11, diagSW1M12, diagSW1M13, diagSW1M14,
        diagSW1M15, diagSW1M16, diagSW1M17;

    // GIRL/DAY1MORN
    [System.NonSerialized]
    public string diagG1M0, diagG1M1, diagG1M2, diagG1M3, diagG1M4, diagG1M5, diagG1M6,
        diagG1M7, diagG1M8, diagG1M8_1_1, diagG1M8_2_1, diagG1M9, diagG1M10, diagG1M11, diagG1M11_1_1, diagG1M11_1_2, 
        diagG1M11_2_1, diagG1M11_2_2, diagG1M11_3_1, diagG1M11_3_2, diagG1M11_4_1, diagG1M11_4_2, diagG1M11_4_3, 
        diagG1M12, diagG1M13, diagG1M14, diagG1M15, diagG1M16, diagG1M17;

    // TYLER/DAY1MORN
    [System.NonSerialized]
    public string diagT1M0, diagT1M1, diagT1M2, diagT1M3, diagT1M4, diagT1M5, diagT1M6,
        diagT1M7, diagT1M8, diagT1M9, diagT1M10, diagT1M11, diagT1M12, diagT1M13, diagT1M14,
        diagT1M15, diagT1M16, diagT1M17;

    // EVAN/DAY1MORN
    public string diagE1M0;
    public string diagE1M1;
    public string diagE1M2;
    public string diagE1M3;
    public string diagE1M4;
    public string diagE1M5;
    public string diagE1M6;
    public string diagE1M7;
    public string diagE1M8;

    // OBJECTS/MILESROOM
    public string diagMR_BP;
    public string diagMR_TV;
    public string diagMR_CMC;
    public string diagMR_BB;

    //OBJECTS/DAY1
    public string diagD1M_LOG1;

    public void Awake()
    {
        setMainDialogueDay1Victor();
        setMainDialogueDay1Wally();
        setMainDialogueDay1Girl();
    }

    void setMainDialogueDay1Victor()
    {
        diagV1M1.Add("Oh jeez, the opening campfire's gonna start any minute now.");
        diagV1M1.Add("I gotta go do something real quick before it starts.");
        diagV1M1.Add("Just wait for me there, 'kay?? Campfire's that way! I'll be quick!!");


        diagV1M2.Add("Ta-dahh...");
        diagV1M2.Add("Fresh outta the Camp Awestruck merch store!");
        diagV1M2.Add("Whatchya think, Miles?");

        diagV1M2.Add("Heh-heh. I knew I'd look rad wearing it.");
        diagV1M2.Add("You know what? Because you said that, I've decided I'm gonna keep it on.");
        diagV1M2.Add("See? Like I was just telling you. Your words are already having an impact on others!");

        diagV1M2.Add("Oh. Is it really that bad?");
        diagV1M2.Add("Guess I'll just take it off then.");
        diagV1M2.Add("See? Your words are already having an impact on others!");
        diagV1M2.Add("If you hadn't said that, I probably woulda kept the hat on the whole week.");

        diagV1M3.Add("This week's gonna be awesome...");
        diagV1M3.Add("On day five, we usually go on an outtrip! It's the best part.");

        //Victor + Wally talk by campfire
        diagV1M3.Add("Yeah, outtrip! We'll get to spend a night out off-campsite. There's gonna be smores, cliff jum--");

        diagV1M3.Add("Yeah!");

        diagV1M3.Add("...Uhh, it IS possible. But I--");

        diagV1M3.Add("The wha--?");

        diagV1M3.Add("Dude, what on earth are you talking about??");

        diagV1M3.Add("Oh God, Miles, MAKE HIM STOP!");

        //Seaweed (still known as "Wasabi") pulls up.
        /*
        diagV1M.Add("Bears? Well, yeah. The island's mostly just wilderness.");
        diagV1M.Add("But you don't have to worry about them! Unless you cover yourself in honey and head to the woods, you won't run into any.");

        diagV1M.Add("The Wolfman? You mean that monster that lurks around in rural areas at night and preys on kids?");
        diagV1M.Add("Don't worry, dude. That's all folklore.");

        diagV1M.Add("Uh... I've never... maybe some people... I mean, you-- probably don't...");
        diagV1M.Add("You're starting to freak me out a little, man.");*/

        diagV1M3.Add("Wasabi!! I'm eleven now, I think you'll get to be me and Miles' camp counsellor this year!!");

        diagV1M3.Add("Be quick, Miles. Don't forget to use that map I gave you if you need to.");
        /*
        diagV1M0 = "Oh jeez, the opening campfire's gonna start any minute now.";
        diagV1M1 = "I gotta go do something real quick before it starts.";
        diagV1M2 = "Just wait for me there, 'kay?? Campfire's that way! I'll be quick!!";


        diagV1M3 = "Ta-dahh...";
        diagV1M4 = "Fresh outta the Camp Awestruck merch store!";
        diagV1M5 = "Whatchya think, Miles?";

        diagV1M5_1_1 = "Heh-heh. I knew I'd look rad wearing it.";
        diagV1M5_1_2 = "You know what? Because you said that, I've decided I'm gonna keep it on.";
        diagV1M5_1_3 = "See? Like I was just telling you. Your words are already having an impact on others!";

        diagV1M5_2_1 = "Oh. Is it really that bad?";
        diagV1M5_2_2 = "Guess I'll just take it off then.";
        diagV1M5_2_3 = "See? Your words are already having an impact on others!";
        diagV1M5_2_4 = "If you hadn't said that, I probably woulda kept the hat on the whole week.";

        diagV1M6 = "This week's gonna be awesome...";
        diagV1M7 = "On day five, we usually go on an outtrip! It's the best part.";

        //Victor + Wally talk by campfire
        diagV1M8 = "Yeah, outtrip! We'll get to spend a night out off-campsite. There's gonna be smores, cliff jum--";

        diagV1M9 = "Yeah!";

        diagV1M10 = "...Uhh, it IS possible. But I--";

        diagV1M11 = "The wha--?";

        diagV1M12 = "Dude, what on earth are you talking about??";

        diagV1M13 = "Oh God, Miles, MAKE HIM STOP!";

        //Seaweed (still known as "Wasabi") pulls up.
        diagV1M7 = "Bears? Well, yeah. The island's mostly just wilderness.";
        diagV1M8 = "But you don't have to worry about them! Unless you cover yourself in honey and head to the woods, you won't run into any.";

        diagV1M9 = "The Wolfman? You mean that monster that lurks around in rural areas at night and preys on kids?";
        diagV1M10 = "Don't worry, dude. That's all folklore.";

        diagV1M11 = "Uh... I've never... maybe some people... I mean, you-- probably don't...";
        diagV1M12 = "You're starting to freak me out a little, man.";

        diagV1M13 = "Wasabi!! I'm eleven now, I think you'll get to be me and Miles' camp counsellor this year!!";

        diagV1M14 = "Be quick, Miles. Don't forget to use that map I gave you if you need to.";*/

    }

    void setMainDialogueDay1Wally()
    {
        diagW1M0 = ".....";
        diagW1M1 = "Hey, did you say outtrip?";

        diagW1M2 = "S-Sleeping o-o-out?? In the wilderness??";
        diagW1M3 = "Can't we run into... bears??";

        diagW1M4 = "Oh no, oh no";
        diagW1M5 = "";
        diagW1M6 = "";
        diagW1M7 = "";
        diagW1M8 = "";
        diagW1M9 = "";
        diagW1M10 = "";
        diagW1M11 = "";
        diagW1M12 = "";
        diagW1M13 = "";
        diagW1M14 = "";
        diagW1M15 = "";
        diagW1M16 = "";
        diagW1M17 = "";
    }
    void setMainDialogueDay1Girl()
    {

        diagG1M0 = "Sniff... The raccoon... scratched my knee... BWAAAAH!!";
        diagG1M1 = "Sniff... *Snort*...";
        diagG1M2 = "My...";
        diagG1M3 = "HERO!!";
        diagG1M4 = "I'm Vicky... Hee hee hee. What's your name, hmmmmm?";
        diagG1M5 = "Miles? Oh, Miles, the way you fought off that mean, fierce creature...";
        diagG1M6 = "You're such a noble, manly, brave, noble, righteous, stoic, magnificent, endearing, and...";
        diagG1M7 = "...did I already say noble?...";
        diagG1M8 = "HERO!";
        //Insert dialogue options here (1 = My Spine! 2 = T-Thanks!)
        diagG1M8_1_1 = "Oops. Hee hee. Sorry!";

        diagG1M8_2_1 = "EEEEEE!! You're such a polite boy, too!!";

        diagG1M9 = "Alright, let's talk real talk. I'm only six years old, and you're...";
        diagG1M10 = "...ten? I guess that's a pretty big age difference. But then again, my mom and dad married seven years apart.";
        diagG1M11 = "You wouldn't let age get in the way of us being together, would you, my wittle tootie patootie?";
        //Insert dialogue options here (1 = Uhh... Yes? 2 = I have to pee. 3 = We're too young to think about this! 4 = ...Not age, no!)
        diagG1M11_1_1 = "What? Why??";
        diagG1M11_1_2 = "Are you TRYING to break my heart? I'm only six!";

        diagG1M11_2_1 = ".......";
        diagG1M11_2_2 = "Boys pee?";

        diagG1M11_3_1 = "Aw, come on, no we're not! We're just being mature!";
        diagG1M11_3_2 = "Besides, what's wrong with being in LOVE?";

        diagG1M11_4_1 = "Yay!! So then it's settled!";
        diagG1M11_4_2 = "Meet me under the only tree in the empty field at Deer Lake park in Burnaby 15 years from now.";
        diagG1M11_4_3 = "Only then, if we both show up, we'll know for sure we're soulmates!";

        diagG1M12 = "...Oh, I can hear everyone walking up to the food court for lunch! Criminy, I'm hungry.";
        diagG1M13 = "I'll DEFINITELY see you around, my oogie woogie boobie woobie.";
        //Funny: Miles thinks "boobie?"
    }

    public string getProperDialogueOptionVictor()
    {
        string diag;

        //PUT CODE INSTEAD:
        diag = diagV1M1[0];

        return diag;
    }

    public string getProperDialogueOptionObject(GameObject obj)
    {
        string diag;

        if (obj.GetComponent<interactObjectInfo>().objectType == "MILESROOM-BDAYPHOTO")
        {
            diag = diagMR_BP;
        } else if (obj.GetComponent<interactObjectInfo>().objectType == "MILESROOM-COMICBOOKS") {
            diag = diagMR_CMC;
        } else if (obj.GetComponent<interactObjectInfo>().objectType == "MILESROOM-TVSET") {
            diag = diagMR_TV;
        } else if (obj.GetComponent<interactObjectInfo>().objectType == "MILESROOM-BEANBAG") {
            diag = diagMR_BB;
        } else {
            return null;
        }

        return diag;
    }

}
