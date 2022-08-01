using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperties : MonoBehaviour
{

    [SerializeField]
    public float height;
    public bool grounded;
    public float floorHeight;

    public GameObject parentObj;
    public GameObject mainObj;
    public GameObject shadowObj;


    public bool checkingForEdge;

    [Range(2f, 10f)]public float raycastDistanceMultiplier;

    public Rigidbody2D rb;

    public float yval;
    public Vector2 heightpos;

    //Damage and recovery
    public bool invincible;
    public float recoveryTime;
    //THE FOLLOWING IS TRUE IF THE hitRecovery COROUTINE IS RUNNING.
    private bool hitRecoveryCR_running;

    //NOTE: Bounciness must be a number between 0 and 1.
    [SerializeField]
    public float bounciness;

    public float prevXVal;
    public float prevYVal;

    void Start()
    {
        if (this.gameObject.name == "Player")
        {
            parentObj = this.transform.parent.gameObject;
        }
        yval = transform.position.y;

        checkingForEdge = true;
        invincible = false;
        hitRecoveryCR_running = false;
        recoveryTime = 1f;

        rb = GetComponent<Rigidbody2D>();

        heightpos = new Vector2(transform.position.x, transform.position.y + height);

        transform.position = heightpos;
        grounded = true;
    }

    private void Update()
    {
        if (invincible == true)
        {
            if (hitRecoveryCR_running == false)
            {
                StartCoroutine(hitRecovery(recoveryTime));
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            }

        }

        if (this.name == "Raccoon")
        {
            yval = transform.parent.Find("Shadow").transform.position.y;
        }

       
        prevXVal = transform.position.x;
        prevYVal = transform.position.y;
    }

    IEnumerator hitRecovery(float waitTime)
    {
        hitRecoveryCR_running = true;

        yield return new WaitForSeconds(waitTime);
        invincible = false;
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

        hitRecoveryCR_running = false;
    }

        
    public void changeYVal(float newYVal)
    {
        this.yval = newYVal;
        return;
    }


}
