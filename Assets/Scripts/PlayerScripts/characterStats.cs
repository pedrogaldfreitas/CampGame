using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterStats : MonoBehaviour
{
    //This script is for determining the player's body part positions.
    public Animator playerAnimator;

    public string rightArmPos;
    public string leftArmPos;
    public string headPos;
    public string mouthPos;
    public string nosePos;
    public string eyesPos;
    public string legsPos;
    public string torsoPos;

    //Respect System Variables
    public int respectLevel;
    public GameObject[] respectBars;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        //PLACEHOLDER
       /*if (this.gameObject.name == "NPCVictor")
       {
            StartCoroutine(changeRespectLevel(1));
       }*/
       if (this.gameObject.name == "Wally")
        {
            changeCharacterSprite("legs", "SITTING");
        }

    }

    private void Update()
    {

        switch (legsPos)
        {
            case "DEFAULT":
                playerAnimator.SetFloat("LegsSprite", 0f);
                break;
            case "STILL":
                playerAnimator.SetFloat("LegsSprite", 0.5f);
                break;
            case "SITTING":
                playerAnimator.SetFloat("LegsSprite", 1f);
                break;
        }
        switch (torsoPos)
        {
            case "DEFAULT":
                playerAnimator.SetFloat("TorsoSprite", 0f);
                break;
            case "STICKHEADBONK":
                playerAnimator.SetFloat("TorsoSprite", 1f);
                break;
        }
        switch (rightArmPos)
        {
            case "DEFAULT":
                playerAnimator.SetFloat("RightArmSprite", 0f);
                break;
            case "FIDDLE":
                playerAnimator.SetFloat("RightArmSprite", 1f);
                break;
            case "STICKHEADBONK":
                playerAnimator.SetFloat("RightArmSprite", 2f);
                break;
        }
        switch (leftArmPos)
        {
            case "DEFAULT":
                playerAnimator.SetFloat("LeftArmSprite", 0f);
                break;
            case "FIDDLE":
                playerAnimator.SetFloat("LeftArmSprite", 1f);
                break;
            case "STICKHEADBONK":
                playerAnimator.SetFloat("LeftArmSprite", 2f);
                break;
        }
        switch (mouthPos)
        {
            case "DEFAULT":
                playerAnimator.SetFloat("MouthSprite", 0f);
                break;
            case "WONKYSMILE":
                playerAnimator.SetFloat("MouthSprite", 1f);
                break;
            case "SMILETALK":
                playerAnimator.SetFloat("MouthSprite", 2f);
                break;
            case "NOTSOSURE":
                playerAnimator.SetFloat("MouthSprite", 3f);
                break;
            case "BITINGLIP1":
                playerAnimator.SetFloat("MouthSprite", 4f);
                break;
            case "NERVOUSTALKING":
                playerAnimator.SetFloat("MouthSprite", 5f);
                break;
            case "STICKHEADBONK":
                playerAnimator.SetFloat("MouthSprite", 6f);
                break;
        }
        switch (eyesPos)
        {
            case "DEFAULT":
                playerAnimator.SetFloat("EyesSprite", 0f);
                break;
            case "STRANGE":
                playerAnimator.SetFloat("EyesSprite", 1f);
                break;
            case "SCARED":
                playerAnimator.SetFloat("EyesSprite", 2f);
                break;
            case "SCAREDWIDE":
                playerAnimator.SetFloat("EyesSprite", 3f);
                break;
            case "STICKHEADBONK":
                playerAnimator.SetFloat("EyesSprite", 4f);
                break;
        }
        switch (nosePos)
        {
            case "DEFAULT":
                playerAnimator.SetFloat("NoseSprite", 0f);
                break;
            case "STICKHEADBONK":
                playerAnimator.SetFloat("NoseSprite", 1f);
                break;
        }
        switch (headPos)
        {
            case "DEFAULT":
                playerAnimator.SetFloat("HeadSprite", 0f);
                break;
            case "STICKHEADBONK":
                playerAnimator.SetFloat("HeadSprite", 1f);
                break;
        }

    }

    public void changeCharacterSprite(string bodyPart, string spriteType)
    {
        if (bodyPart == "legs")
        {
            legsPos = spriteType;
        }
        if (bodyPart == "eyes")
        {
            eyesPos = spriteType;
        }
        if (bodyPart == "mouth")
        {
            mouthPos = spriteType;
        }
        if (bodyPart == "nose")
        {
            nosePos = spriteType;
        }
        if (bodyPart == "torso")
        {
            torsoPos = spriteType;
        }
        if (bodyPart == "rightarm")
        {
            rightArmPos = spriteType;
        }
        if (bodyPart == "leftarm")
        {
            leftArmPos = spriteType;
        }
        if (bodyPart == "head")
        {
            headPos = spriteType;
        }
    }

    IEnumerator changeRespectLevel(int amount)
    {
        //PLACEHOLDER VALUE
        respectLevel = 6;

        if (respectLevel < -amount)
        {
            amount = -respectLevel;
        }
        if (respectLevel + amount > 16)
        {
            amount = 16 - respectLevel;
        }

        for (int j = 0; j < respectLevel; j++)
        {
            respectBars[j].GetComponent<SpriteRenderer>().color = new Color((150f - (j * 2f)) / 255f, (150f + (j * 5f)) / 255f, (150f - (j * 5f)) / 255f, 0);
        }

        StartCoroutine(flashRespectBars(amount));
        yield return StartCoroutine(fadeInRespectMeter());
        yield return StartCoroutine(respectMeterShowTime());
        StopAllCoroutines();
        yield return StartCoroutine(fadeOutRespectMeter());
        respectLevel += amount;
        yield break;
    }

    IEnumerator fadeInRespectMeter()
    {
        SpriteRenderer respectMeterRenderer = GameObject.Find("RespectMeter").GetComponent<SpriteRenderer>();
        for (float i = 0; i<=1f; i+=0.05f)
        {
            respectMeterRenderer.color = new Color(255, 255, 255, i);
            for (int j = 0; j < respectLevel; j++)
            {
                respectBars[j].GetComponent<SpriteRenderer>().color = new Color(respectBars[j].GetComponent<SpriteRenderer>().color.r, respectBars[j].GetComponent<SpriteRenderer>().color.g, respectBars[j].GetComponent<SpriteRenderer>().color.b, i);
            }
            yield return new WaitForSeconds(0.005f);
        }
        respectMeterRenderer.color = new Color(255, 255, 255, 1);

    }

    IEnumerator flashRespectBarsNegative(int amount)
    {
        for (int f = respectLevel; f < respectLevel + amount; f++)
        {
            respectBars[f].GetComponent<SpriteRenderer>().color = new Color(1f, (109f / 255f), (70f / 255f), 0f);
        }
        while (true)
        {
            for (float i = 0; i <= 1f; i += 0.05f)
            {
                for (int j = respectLevel; j < respectLevel + amount; j++)
                {
                    respectBars[j].GetComponent<SpriteRenderer>().color = new Color(respectBars[j].GetComponent<SpriteRenderer>().color.r, respectBars[j].GetComponent<SpriteRenderer>().color.g, respectBars[j].GetComponent<SpriteRenderer>().color.b, i);
                }
                yield return new WaitForSeconds(0.005f);
            }

            for (float i = 1; i >= 0; i -= 0.05f)
            {
                for (int j = respectLevel; j < respectLevel + amount; j++)
                {
                    respectBars[j].GetComponent<SpriteRenderer>().color = new Color(respectBars[j].GetComponent<SpriteRenderer>().color.r, respectBars[j].GetComponent<SpriteRenderer>().color.g, respectBars[j].GetComponent<SpriteRenderer>().color.b, i);
                }
                yield return new WaitForSeconds(0.005f);
            }
        }
    }

    IEnumerator flashRespectBars(int amount)
    {
        if (amount > 0)
        {
            for (int f = respectLevel; f < respectLevel + amount; f++)
            {
                respectBars[f].GetComponent<SpriteRenderer>().color = new Color((130f/255f), (1f), 1f, 0f);
            }
        } else
        {
            for (int f = respectLevel+amount; f < respectLevel; f++)
            {
                respectBars[f].GetComponent<SpriteRenderer>().color = new Color(1f, (109f/255f), (70f/255f), 0f);
            }
        }
        
        while (true)
        {
            for (float i = 0; i <= 1f; i += 0.05f)
            {
                if (amount > 0)
                {
                    for (int j = respectLevel; j < respectLevel + amount; j++)
                    {
                        respectBars[j].GetComponent<SpriteRenderer>().color = new Color(respectBars[j].GetComponent<SpriteRenderer>().color.r, respectBars[j].GetComponent<SpriteRenderer>().color.g, respectBars[j].GetComponent<SpriteRenderer>().color.b, i);
                    }
                } else
                {
                    for (int j = respectLevel + amount; j < respectLevel; j++)
                    {
                        respectBars[j].GetComponent<SpriteRenderer>().color = new Color(respectBars[j].GetComponent<SpriteRenderer>().color.r, respectBars[j].GetComponent<SpriteRenderer>().color.g, respectBars[j].GetComponent<SpriteRenderer>().color.b, i);
                    }
                }
                yield return new WaitForSeconds(0.005f);
            }

            for (float i = 1; i >= 0; i -= 0.05f)
            {
                if (amount > 0)
                {
                    for (int j = respectLevel; j < respectLevel + amount; j++)
                    {
                        respectBars[j].GetComponent<SpriteRenderer>().color = new Color(respectBars[j].GetComponent<SpriteRenderer>().color.r, respectBars[j].GetComponent<SpriteRenderer>().color.g, respectBars[j].GetComponent<SpriteRenderer>().color.b, i);
                    }
                } else
                {
                    for (int j = respectLevel+amount; j < respectLevel; j++)
                    {
                        respectBars[j].GetComponent<SpriteRenderer>().color = new Color(respectBars[j].GetComponent<SpriteRenderer>().color.r, respectBars[j].GetComponent<SpriteRenderer>().color.g, respectBars[j].GetComponent<SpriteRenderer>().color.b, i);
                    }
                }
                yield return new WaitForSeconds(0.005f);
            }
        }
    }

    IEnumerator fadeOutRespectMeter()
    {
        SpriteRenderer respectMeterRenderer = GameObject.Find("RespectMeter").GetComponent<SpriteRenderer>();
        for (float i = 1; i >= 0; i -= 0.05f)
        {
            respectMeterRenderer.color = new Color(255, 255, 255, i);

            for (int j = 0; j < respectLevel; j++)
            {
                respectBars[j].GetComponent<SpriteRenderer>().color = new Color(respectBars[j].GetComponent<SpriteRenderer>().color.r, respectBars[j].GetComponent<SpriteRenderer>().color.g, respectBars[j].GetComponent<SpriteRenderer>().color.b, i);
            }

            yield return new WaitForSeconds(0.005f);
        }
        respectMeterRenderer.color = new Color(255, 255, 255, 0);

        for (int j = 0; j < 16; j++)
        {
            respectBars[j].GetComponent<SpriteRenderer>().color = new Color(respectBars[j].GetComponent<SpriteRenderer>().color.r, respectBars[j].GetComponent<SpriteRenderer>().color.g, respectBars[j].GetComponent<SpriteRenderer>().color.b, 0);
        }
    }

    IEnumerator respectMeterShowTime()
    {
        yield return new WaitForSeconds(5f);
    }

}
