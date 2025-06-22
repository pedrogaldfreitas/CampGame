using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    bool menuOpen;
    [SerializeField]
    private GameObject settingsPanelObj;
    private TMP_Dropdown vSyncDropdown;

    // Start is called before the first frame update
    void Start()
    {
        SetDefaultSettings();
        menuOpen = false;
        settingsPanelObj.SetActive(false);
        vSyncDropdown = settingsPanelObj.transform.Find("VSyncDropdown").GetComponent<TMP_Dropdown>();
        vSyncDropdown.onValueChanged.AddListener(SetVSync);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuOpen = !menuOpen;
            settingsPanelObj.SetActive(menuOpen);
        }
    }

    public void SetVSync(int index)
    {
        switch (index)
        {
            case 0: // On
                QualitySettings.vSyncCount = 1;
                break;
            case 1: // Off
                QualitySettings.vSyncCount = 0;
                break;
        }
    }

    private void SetDefaultSettings()
    {
        SetVSync(0);
    }
}
