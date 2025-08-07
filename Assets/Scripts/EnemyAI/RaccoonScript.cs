using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaccoonScript : MonoBehaviour
{
    /*
     * ---- RACCOON ----
     * Raccoons are the first enemy you'll encounter and have to deal with. Must have basic attacks and mannerisms.
     * 
     * ROUTINE:
     * -When the player is close to the raccoon, the raccoon will stand up on two legs, alert. You have seconds to get away.
     * -If the player does not get away, the raccoon will get angry. It will then chase the player to attack. Sometimes, it will jump
     *  at the player, which would be a quicker and less predictable attack.
     * 
     */
    GameObject playerParent;

    void Start()
    {
        playerParent = GameObject.Find("Player");
    }

    void Update()
    {
        
    }


}
