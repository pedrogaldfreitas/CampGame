using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public enum Day { ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT };
    [SerializeField]
    static public Day currentDay;

    private CutsceneManager cutsceneManager;

    public class Mission {
        public string titleText;
        public Mission()
        {
            titleText = "";
        }
        public Mission(string title)
        {
            titleText = title;
        }
    }

    [SerializeField]
    public Mission activeMission;

    //LIST OF MISSIONS (DAY ONE)
    private Mission D1_M0 = new Mission(), 
        D1_M1 = new Mission("Go to the opening campfire"),
        D1_M2 = new Mission("Find the missing girl"),
        D1_M3 = new Mission("Regroup with the campers");

    public void Start()
    {
        currentDay = Day.ONE;
        cutsceneManager = GameObject.Find("UI").GetComponent<CutsceneManager>();

        switch (currentDay) {
            case Day.ONE:
                activeMission = D1_M1;
                break;
            case Day.TWO:
                break;
            case Day.THREE:
                break;
            case Day.FOUR:
                break;
            case Day.FIVE:
                break;
            case Day.SIX:
                break;
            default:
                break;
        }
        TransitionMission(activeMission);
    }

    //Ends current mission (if it exists) and transitions to selected mission.
    public void TransitionMission(Mission missionID)
    {
        activeMission = missionID;
        Debug.Log("PEDROLOG: mission text = " + missionID.titleText);
        if (missionID.titleText != "") StartCoroutine(cutsceneManager.ShowMissionText(missionID.titleText));

        return;
    }
    /*
    static public class DayOne
    {
        static public bool gotStick = false;
        public enum Objective
        {
            GETOFFBOAT, GOTOOPENINGCAMPFIRE, RESCUEGIRL, MEETWITHBOYS, GOTOCABINFORSETUP,
            TALKTOVICTOR, GATHERTIEDYEOBJECTS, TIEDYEWITHTHEBOYS, GATHERBASEBALLITEMS, PLAYBASEBALLWITHTHEBOYS,
            GATHERCANOEOBJECTS, GOCANOEINGWITHTHEBOYS, GOFORDINNER, BASKETBALLWITHVICTOR, NIGHTCAMPFIRE,
            FINDCOUNSELLOR1, FINDCOUNSELLOR2, FINDCOUNSELLOR3, HEADTOMAGICWELL, GETTHEBOOK, GOTOCABINTOSLEEP
        };

        public static Objective currentDay1Objective = 0;

        static public class Morning
        {
            static public bool spokeToVicAfterBoat = false;
            static public bool sentToGetGirl = false;
            static public bool rescuedGirl = false;
            static public bool obtainedOrcaStonePiece = false;

            static public bool metTheBoys = false;
            static public bool hadLunch = false;
        }
        static public class Afternoon
        {

        }

        static public class Evening
        {

        }
    }
    static public class DayTwo
    {

        static public class Morning
        {

        }
        static public class Afternoon
        {

        }

        static public class Evening
        {

        }
    }
    static public class DayThree
    {

        static public class Morning
        {

        }
        static public class Afternoon
        {

        }

        static public class Evening
        {

        }
    }
    static public class DayFour
    {

        static public class Morning
        {

        }
        static public class Afternoon
        {

        }

        static public class Evening
        {

        }
    }
    static public class DayFive
    {

        static public class Morning
        {

        }
        static public class Afternoon
        {

        }

        static public class Evening
        {

        }
    }
    static public class DaySix
    {

        static public class Morning
        {

        }
        static public class Afternoon
        {

        }

        static public class Evening
        {

        }

        static public class Night
        {

        }
    }
    static public class DaySeven
    {
        static public bool gotStick = false;
        public enum Objective
        {
            GODOWNSTAIRS, PLAYWITHMILEYINLIVINGROOM, PLAYWITHGIRLINHERROOM, FINDHIDINGPLACE1, COUNTTOTEN,
            FINDMILEYHIDDEN, CHASEMILEY, FINDHIDINGPLACE2, GOTOKITCHEN, LEAVETHEHOME, HEADINTOWOODS,
            CROSSDEEPFOREST, GETTOTOPOFWATERFALL, SOLVEPUZZLEINSIDEWATERFALL, TRAVERSETHROUGHCAVE,
            FIGHTTERRY, MEETSHALA, GETTOFINALFIGHTPOINT, DEFEATGURI, PLACESTONEINPEAKOFMOUNTAIN,
            GOTOPARTY, DANCETHENIGHTAWAY
        };

        public static Objective currentDay7Objective = 0;

        static public class Morning
        {

        }
        static public class Afternoon
        {

        }

        static public class Evening
        {

        }
    }
    static public class DayEight
    {

        static public class Morning
        {

        }
        static public class Afternoon
        {

        }

        static public class Evening
        {

        }
    }*/

}
