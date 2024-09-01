using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public enum Day { ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT };
    [SerializeField]
    static public Day currentDay;

    public class LocationState {
        string areaName;
        Vector2 position;

        LocationState(string areaInput, Vector2 positionInput)
        {
            areaName = areaInput;
            position = positionInput;
        }
    }

    #region Character Locations

    public LocationState MilesLoc;
    public LocationState VictorLoc;
    public LocationState VickyLoc;

    #endregion Character Locations

    private CutsceneManager cutsceneManager;

    public class Mission {
        public string missionID;
        public string titleText;
        public Mission(string ID)
        {
            missionID = ID;
            titleText = "";
        }
        public Mission(string ID, string title)
        {
            missionID = ID;
            titleText = title;
        }
    }

    [SerializeField]
    public Mission activeMission;
    public bool settingStates = true;

    //LIST OF MISSIONS (DAY ONE)
    /*
     WARNING: For each mission created, you MUST add it into dayOneMissions list in order for the missions debug panel to work properly.
     */
    public List<Mission> dayOneMissions = new List<Mission>();
   
    public void Start()
    {
        currentDay = Day.ONE;
        cutsceneManager = GameObject.Find("UI").GetComponent<CutsceneManager>();

        dayOneMissions.Add(new Mission("D1_M0"));
        dayOneMissions.Add(new Mission("D1_M1", "Go to the opening campfire"));
        dayOneMissions.Add(new Mission("D1_M2", "Find the missing girl"));
        dayOneMissions.Add(new Mission("D1_M3", "Regroup with the campers"));

        switch (currentDay) {
            case Day.ONE:
                activeMission = GetMissionFromID("D1_M1");
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

        settingStates = false;
    }

    //Gets mission object based on ID.
    public Mission GetMissionFromID(string missionID)
    {
        return dayOneMissions.Find((mission) => { return mission.missionID == missionID; });
    }

    //Ends current mission (if it exists) and transitions to selected mission.
    public void TransitionMission(Mission mission)
    {
        activeMission = mission;
        if (mission.titleText != "") StartCoroutine(cutsceneManager.ShowMissionText(mission.titleText));

        //HERE: Set the positions of all items, characters, world stats, etc. depending on the mission.
        switch(mission.missionID)
        {
            case "D1M1":

                break;
            default:
                break;
        }

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
