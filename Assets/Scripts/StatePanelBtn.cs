using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePanelBtn : MonoBehaviour
{
    [SerializeField]
    private GameObject panelGUI;

    public void OnClickBtn()
    {
        panelGUI.SetActive(!panelGUI.activeSelf);
    }
}
