using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int heartSlots;
    public Transform playerParentTransform;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    private AudioSource audioSource;
    public AudioClip hurtSound1;
    public AudioClip hurtSound2;
    public AudioClip hurtSound3;

    private void Start()
    {
        playerParentTransform = transform.parent;
        audioSource = GetComponents<AudioSource>()[0];
    }

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

    public IEnumerator KnockbackPlayer(Vector2 damageOriginPoint, float knockbackPower)
    {
        //Vector2 parentTransformPosition = playerParentTransform.position;
        Vector2 playerToEnemyVector = (Vector2)playerParentTransform.Find("Shadow").position - damageOriginPoint;
        playerToEnemyVector.Normalize();
        for (int i = 0; i < 10; i++)
        {
            playerParentTransform.Translate(playerToEnemyVector*knockbackPower, Space.World);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void DamagePlayer(int dmg, Vector2 damageOriginPoint = new Vector2(), float knockbackPower = 0)
    {
        damageHealth(dmg);
        int rndNum = Random.Range(1, 4);
        switch (rndNum)
        {
            case 1:
                audioSource.clip = hurtSound1;
                break;
            case 2:
                audioSource.clip = hurtSound2;
                break;
            case 3:
                audioSource.clip = hurtSound3;
                break;
            default:
                audioSource.clip = hurtSound1;
                break; 
        }

        audioSource.Play();
        if (damageOriginPoint != Vector2.zero && knockbackPower != 0)
        {
            StartCoroutine(KnockbackPlayer(damageOriginPoint, knockbackPower));
        }
    }
}
