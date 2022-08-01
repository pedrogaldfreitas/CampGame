using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    int currentFPS;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            currentFPS = (int)(1f / Time.unscaledDeltaTime);
            GetComponent<Text>().text = "FPS: " + currentFPS;
            timer = 30;
        }
        timer--;
    }

}
