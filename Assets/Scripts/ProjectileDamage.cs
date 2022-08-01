using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    public int damagedhealth;
    private GameObject playerobj;
    private float yvalue;


    private void Start()
    {
        playerobj = GameObject.Find("Player");
        damagedhealth = playerobj.GetComponent<PlayerHealth>().health;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {

            yvalue = this.GetComponent<ObjectProperties>().yval;

            if (col.GetType() == typeof(PolygonCollider2D))
            {
                //IF THE SHADOW COLLIDES WITH THE PLAYER'S LOWER HITBOX:
                if ((yvalue <= playerobj.transform.position.y - 3.05145) && (yvalue >= playerobj.transform.position.y - 4.08435))
                {
                    Debug.Log("Shadow Collided");
                    damagedhealth = damagedhealth - damage;
                }
            }
        }
    }
}
