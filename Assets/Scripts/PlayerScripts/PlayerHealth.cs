using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int heartSlots;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    private void Update()
    {

        if (health > heartSlots*2)
        {
            health = heartSlots*2;
        }

        if (health < 0)
        {
            health = 0;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            health = health - 3;
        }

        //The for loop determines how many hearts and heart slots will be shown.
        for (int i = 0; i < hearts.Length ; i++)
        {
            if (i < health/2)
            {
                hearts[i].sprite = fullHeart;
            } else if ((i == health/2)&&(health%2 != 0))
            {
                hearts[i].sprite = halfHeart;
            } else if ((i >= health / 2))
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < heartSlots)
            {
                hearts[i].enabled = true;
            } else
            {
                hearts[i].enabled = false;
            }
        }

        //Returns to this script the health value after a collision on the ProjectileDamage.cs script.
        //health = GameObject.Find("Projectile").GetComponent<ProjectileDamage>().damagedhealth;
    }

    public void damageHealth(int dmg)
    {
        health = health - dmg;
        return;
    }
}
