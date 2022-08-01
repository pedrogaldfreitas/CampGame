using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public enum Day { ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT };
    [SerializeField]
    static public Day currentDay;

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
    }

}
