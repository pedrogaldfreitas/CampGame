using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInventory : MonoBehaviour
{
    //PRIMARY ITEMS: STORY
    //Stick, Water gun, climbing powder
    public string activePrimary;

    public bool stickObtained;
    public bool waterGunObtained;
    public bool climbingPowderObtained;

    //SECONDARY ITEMS: WORLD
    //Basketball, Bird Seeds, Acorn, Baseball, Big Rock, Secondary Watergun Capsule...
    public string equippedSecondary;
    public GameObject equippedSecondaryObject;

    //STORY ITEMS
    //Camp Map, Stone Fragments (3), Book, Christie's Key, Shovel, Treasure Map
    public bool campMapObtained;
    public bool stonePart1Obtained;
    public bool stonePart2Obtained;
    public bool stonePart3Obtained;
    public bool bookObtained;
    public bool shovelObtained;
    public bool christiesKeyObtained;
    public bool treasureMapObtained;

    //TRIGGERED EVENTS
    //First step on acorn
    public bool firstAcornCutscenePlayed;

    private void Start()
    {
        equippedSecondary = "";
    }
    

    //Returns 1 if successful; 0 if unsuccessful.
    public int equipItem(string itemName)
    {
        if (itemName == "STICK")
        {
            if (stickObtained != true)
            {
                return 0;
            }
            activePrimary = "STICK";
            return 1;

        }
        return 0;
    }

    public void obtainItem(GameObject item, string itemType)
    {
        if (itemType == "STICK")
        {
            stickObtained = true;
            equipItem("STICK");
        }
        if (itemType == "PINECONE")
        {
            equippedSecondary = "PINECONE";
            equippedSecondaryObject = item;
        }
        return;
    }

    public void dropItem(string itemType)
    {
        if (itemType == "STICK")
        {
            stickObtained = false;
            //Because this is a primary item, make sure it moves on to the next primary item.
        }
        if (itemType == "SECONDARY")
        {
            equippedSecondary = "";
        }
        return;
    }
}
