using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setMissionProgressionPoints : MonoBehaviour
{
    public bool enabledPoints;

    Transform[] dayPoints;

    List<Transform> day1Points;
    List<Transform> day7Points;

    private void Start()
    {

        dayPoints = new Transform[8];

        day1Points = new List<Transform>();
        day7Points = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++) {
            dayPoints[i] = transform.GetChild(i);

            for (int d1 = 0; d1 < dayPoints[i].childCount; d1++)
            {
                day1Points.Add(dayPoints[i].GetChild(d1));
                day1Points[d1].gameObject.SetActive(false);
            }
        }

        //Initial state.
        if (enabledPoints)
        {
            switch(StateManager.currentDay)
            {
                case StateManager.Day.ONE:
                    //setMissionStateDay1(StateManager.DayOne.Objective.GETOFFBOAT);
                    //TEMPORARY FOR TESTING:
                    setMissionStateDay1(StateManager.DayOne.Objective.RESCUEGIRL);
                    break;
                case StateManager.Day.SEVEN:
                    setMissionStateDay7(StateManager.DaySeven.Objective.GODOWNSTAIRS);
                    break;
            }
        }
    }

    public void setMissionStateDay1(StateManager.DayOne.Objective objective)
    {
        switch (objective)
        {
            case StateManager.DayOne.Objective.GETOFFBOAT:
                day1Points.Find(point => point.name == "TalkToVictor").gameObject.SetActive(true);
                break;
            case StateManager.DayOne.Objective.GOTOOPENINGCAMPFIRE:
                day1Points.Find(point => point.name == "TalkToVictor").gameObject.SetActive(false);
                day1Points.Find(point => point.name == "FindASeat").gameObject.SetActive(true);
                break;
            case StateManager.DayOne.Objective.RESCUEGIRL:
                day1Points.Find(point => point.name == "FindASeat").gameObject.SetActive(false);
                day1Points.Find(point => point.name == "TalkToGirl").gameObject.SetActive(true);
                break;
            case StateManager.DayOne.Objective.MEETWITHBOYS:
                day1Points.Find(point => point.name == "TalkToGirl").gameObject.SetActive(false);
                day1Points.Find(point => point.name == "MeetTheBoys").gameObject.SetActive(false);
                break;
        }
    }

    public void setMissionStateDay7(StateManager.DaySeven.Objective objective)
    {
        switch (objective)
        {
            case StateManager.DaySeven.Objective.GODOWNSTAIRS:
                day7Points.Find(point => point.name == "GoDownstairs").gameObject.SetActive(true);
                break;
        }
    }
}
