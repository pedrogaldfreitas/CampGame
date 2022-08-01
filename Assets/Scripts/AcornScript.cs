using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornScript : MonoBehaviour
{
    public GameObject squirrel1;
    public GameObject squirrel2;
    public GameObject squirrel3;

    private void Start()
    {
        squirrel1 = null;
        squirrel2 = null;
        squirrel3 = null;
    }

    private void Update()
    {
        if ((GameObject.Find("Player").GetComponent<playerInventory>().firstAcornCutscenePlayed == false)&&(Vector2.Distance((GameObject.Find("Player").transform.Find("PlayerLegs").transform.position), this.transform.position) < 20))
        {
            GameObject.Find("UI").GetComponent<CutsceneManager>().playCutsceneUsingID("D1ACORNSTEPWARNING()");
            GameObject.Find("Player").GetComponent<playerInventory>().firstAcornCutscenePlayed = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Add squirrel to acorn.
        if ((collision.tag == "Squirrel")&&(collision.GetType() == typeof(CircleCollider2D))) {
            if (squirrel1 == null)
            {
                squirrel1 = collision.gameObject;
            } else if (squirrel2 == null)
            {
                squirrel2 = collision.gameObject;
            } else if (squirrel3 == null)
            {
                squirrel3 = collision.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.tag == "Squirrel") && (collision.GetType() == typeof(CircleCollider2D)))
        {
            if (collision.gameObject == squirrel1)
            {
                squirrel1 = null;
            } else if (collision.gameObject == squirrel2)
            {
                squirrel2 = null;
            } else if (collision.gameObject == squirrel3)
            {
                squirrel3 = null;
            }
        }
    }

    public void acornStep()
    {
        if (squirrel1 != null)
        {
            squirrel1.GetComponent<SquirrelAI>().changeState("ANGRY");
        }
        if (squirrel2 != null)
        {
            squirrel2.GetComponent<SquirrelAI>().changeState("ANGRY");
        }
        if (squirrel3 != null)
        {
            squirrel3.GetComponent<SquirrelAI>().changeState("ANGRY");
        }
    }
}
