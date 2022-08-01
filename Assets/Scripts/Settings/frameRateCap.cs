using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frameRateCap : MonoBehaviour
{
    //FPS will be capped in order to ensure objects fall at proper speeds, etc.
    private void Awake()
    {
        QualitySettings.vSyncCount = 0; // Disable vSync.
        Application.targetFrameRate = 60; // Cap the game at 60fps.
    }
}
