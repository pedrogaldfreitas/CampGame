using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakableObjectScript : MonoBehaviour
{
    [SerializeField]
    int objectHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (objectHealth <= 0)
        {
            //play an animation for destroying the object, and change its appearance to destroyed. also make it "go-throughable".
        }
    }

    //function for everytime this object is struck by the player's stick, it does a certain amount of damage.
    //IDEA: Damage the stick 1/10 the amount the opposing object was damaged.
    public void TakeDamage(int damage, bool newtonsLaw)
    {
        if (newtonsLaw == true)
        {
            //damage the player's stick.
        }
        objectHealth -= damage;
        //play animation, and depending on health of object, change its appearance. (becoming more broken after every few hits).

        return;
    }
}
