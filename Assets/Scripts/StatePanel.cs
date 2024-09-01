using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatePanel : MonoBehaviour
{
    private TMP_Dropdown missionDropdown;
    private StateManager stateManager;
    private List<TMP_Dropdown.OptionData> day1Missions;

    private CutsceneManager cutsceneManager;

    private void Start()
    {
        missionDropdown = GameObject.Find("MissionDropdown").GetComponent<TMP_Dropdown>();
        stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();
        cutsceneManager = GameObject.Find("UI").GetComponent<CutsceneManager>();

        day1Missions = new List<TMP_Dropdown.OptionData>();
        missionDropdown.ClearOptions();
        foreach (StateManager.Mission mission in stateManager.dayOneMissions)
        {
            day1Missions.Add(new TMP_Dropdown.OptionData(mission.missionID));
        }
        missionDropdown.AddOptions(day1Missions);
    }

    public void MissionStateUpdate()
    {
        StartCoroutine(MissionStateUpdateCoroutine());
    }

    private IEnumerator MissionStateUpdateCoroutine()
    {
        yield return StartCoroutine(cutsceneManager.FastFadeIn(0.008f));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(cutsceneManager.FastFadeOut(0.008f));
        stateManager.TransitionMission(stateManager.dayOneMissions.Find((mission) => { return mission.missionID == missionDropdown.options[missionDropdown.value].text; }));

    }
}
