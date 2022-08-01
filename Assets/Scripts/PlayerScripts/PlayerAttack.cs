using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public LayerMask whatIsEnemies;

    private GameObject attackPos;
    private Transform stickObj;
    public Transform attackCirclePos;
    public float attackRange;

    public Animator attackAnimator;

    private playerInventory playerInventory;

    private PlayerFacing PlayerFacing;

    public float xPosCircle;
    public float yPosCircle;

    public int damage;

    int swingNum;
    public bool currentlySwinging;
    public float dashForwardSpeed;
    IEnumerator currentSwingStickCoroutine;

    private void Start()
    {
        timeBtwAttack = 0;
        currentlySwinging = false;
        attackPos = GameObject.Find("AttackPos");
        stickObj = transform.Find("Stick");
        PlayerFacing = GetComponent<PlayerFacing>();

        playerInventory = this.GetComponent<playerInventory>();
        swingNum = 0;
        currentSwingStickCoroutine = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInventory.activePrimary == "STICK")
        {
            if ((Input.GetKeyDown(KeyCode.Z) == true) && !currentlySwinging)
            {
                Debug.Log("SWUNG!");
                if (currentSwingStickCoroutine != null)
                {
                    StopCoroutine(currentSwingStickCoroutine);
                }
                StartCoroutine(swingStick());
                //currentlySwinging = true;
                timeBtwAttack = startTimeBtwAttack;
            }

           //if (!attackAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            //{
                //currentlySwinging = false;
           // }

            if (timeBtwAttack > 0)
            {
                timeBtwAttack -= Time.deltaTime;
            }
            else if (timeBtwAttack <= 0)
            {
                timeBtwAttack = 0;
                currentlySwinging = false;
            }
        }
    }

    IEnumerator swingStickPlayerMovement(Vector2 dirOfMovement)
    {
        transform.parent.GetComponent<PlayerController>().disableMovement();
        float step = dashForwardSpeed * Time.deltaTime;
        currentlySwinging = true;

        switch (swingNum)
        {
            default:
                //Movement based on time instead of position.
                for (int i = 0; i < (14/ attackAnimator.GetCurrentAnimatorStateInfo(0).speed); i++)
                {
                    Vector2 positionToMoveTo = (Vector2)transform.parent.position + dirOfMovement * 15;
                    transform.parent.position = Vector2.MoveTowards(transform.parent.position, positionToMoveTo, step*1.5f);
                    yield return new WaitForSeconds(0.01f / attackAnimator.GetCurrentAnimatorStateInfo(0).speed);
                }
                currentlySwinging = false;
                //for (int i = 0; i < 6; i) //CONTINUE HERE TMR: HAVE LERP EFFECT FOR LAST 0.06 SECONDS
                break;
        }

        transform.parent.GetComponent<PlayerController>().enableMovement();
        currentSwingStickCoroutine = null;
    }

    IEnumerator swingStick()
    {
        swingNum++;
        attackAnimator.SetFloat("SwingNum", swingNum);


        Vector2 directionOfMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        yield return new WaitForSeconds(0.08f);
        //If player is facing forwards, move the attack position to the proper position.
        if (PlayerFacing.playerFacingDir == PlayerFacing.facingDir.DOWN)
        {
            Debug.Log("Attacked Down.");
            attackPos.transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y + 0.78f, transform.position.z);
        }
        else if (PlayerFacing.playerFacingDir == PlayerFacing.facingDir.UP)
        {
            Debug.Log("Attacked Up.");
            attackPos.transform.position = new Vector3(transform.position.x, transform.position.y + 0.19f, transform.position.z);
        }
        else if (PlayerFacing.playerFacingDir == PlayerFacing.facingDir.LEFT)
        {
            Debug.Log("Attacked Left.");
            attackPos.transform.position = new Vector3(transform.position.x - 0.06f, transform.position.y, transform.position.z);
        }
        else if (PlayerFacing.playerFacingDir == PlayerFacing.facingDir.RIGHT)
        {
            Debug.Log("Attacked Right.");
            attackPos.transform.position = new Vector3(transform.position.x + 0.06f, transform.position.y, transform.position.z);
        }


        //Animation

        if (currentSwingStickCoroutine != null)
        {
            StopCoroutine(currentSwingStickCoroutine);
        }

        adjustStickTransform();
        currentSwingStickCoroutine = swingStickPlayerMovement(directionOfMovement);
        StartCoroutine(currentSwingStickCoroutine);
        attackAnimator.SetTrigger("Zkey");

        //The actual attack code.

        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackCirclePos.position, attackRange, whatIsEnemies);

        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (PlayerFacing.playerFacingDir == PlayerFacing.facingDir.DOWN)
            {

                if (enemiesToDamage[i].transform.position.y <= attackCirclePos.position.y)
                {
                    if (enemiesToDamage[i].tag == "InteractObject")
                    {
                        enemiesToDamage[i].GetComponent<breakableObjectScript>().TakeDamage(damage, true);
                    }
                    else
                    {
                        hitEnemy(enemiesToDamage[i]);
                        //enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                    }
                }
            }
            else if (PlayerFacing.playerFacingDir == PlayerFacing.facingDir.UP)
            {
                if (enemiesToDamage[i].transform.position.y >= attackCirclePos.position.y)
                {
                    if (enemiesToDamage[i].tag == "InteractObject")
                    {
                        enemiesToDamage[i].GetComponent<breakableObjectScript>().TakeDamage(damage, true);
                    }
                    else
                    {
                        hitEnemy(enemiesToDamage[i]);
                        // enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                    }
                }
            }
            else if (PlayerFacing.playerFacingDir == PlayerFacing.facingDir.LEFT)
            {
                if (enemiesToDamage[i].transform.position.x <= attackCirclePos.position.x)
                {
                    if (enemiesToDamage[i].tag == "InteractObject")
                    {
                        enemiesToDamage[i].GetComponent<breakableObjectScript>().TakeDamage(damage, true);
                    }
                    else
                    {
                        hitEnemy(enemiesToDamage[i]);
                        //enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                    }
                }
            }
            else if (PlayerFacing.playerFacingDir == PlayerFacing.facingDir.RIGHT)
            {
                if (enemiesToDamage[i].transform.position.x >= attackCirclePos.position.x)
                {
                    if (enemiesToDamage[i].tag == "InteractObject")
                    {
                        enemiesToDamage[i].GetComponent<breakableObjectScript>().TakeDamage(damage, true);
                    }
                    else
                    {
                        hitEnemy(enemiesToDamage[i]);
                        //enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                    }
                }
            }
        }

        if (swingNum == 3)
        {
            swingNum = 0;
        }

    }

    void adjustStickTransform()
    {
        switch (swingNum)
        {
            case 1:
                Debug.Log("Case 1");
                stickObj.localPosition = new Vector3(0.41f, 1.39f, -0.5f);
                stickObj.GetComponent<SpriteRenderer>().flipX = false;
                break;
            case 2:
                Debug.Log("Case 2 P1 - " + stickObj.localPosition);
                stickObj.localPosition = new Vector3(-0.36f, 1.28f, -0.5f);
                Debug.Log("Case 2 P2 - " + stickObj.localPosition);
                stickObj.GetComponent<SpriteRenderer>().flipX = true;
                break;
            case 3:
                Debug.Log("Case 3 P1 - " + stickObj.localPosition);
                stickObj.localPosition = new Vector3(0.41f, 1.39f, -0.5f);
                stickObj.GetComponent<SpriteRenderer>().flipX = false;
                break;
        }

        return;
    }

    private void hitEnemy(Collider2D enemy)
    {
        Debug.Log("hitEnemy works.");

        float hitForce = swingNum == 3 ? 2 : 0.7f;
        damage = swingNum == 3 ? damage * 2 : damage;

        enemy.GetComponent<Enemy>().TakeDamage(damage);
        if (enemy.gameObject.GetComponent<Enemy>().lightEnemy)
        {
            launchBackEnemy(enemy.gameObject, hitForce);
        }
    }

    //parameter "enemy" takes the parent object of the enemy.
    private void launchBackEnemy(GameObject enemy, float hitForce)
    {
        //float oldVerticalVel = enemy.transform.parent.GetComponent<FakeHeightObject>().verticalVelocity;
        //enemy.transform.parent.GetComponent<FakeHeightObject>().verticalVelocity = Mathf.Abs(oldVerticalVel)*hitForce + hitForce;

        //Vector2 oldGroundVel = enemy.transform.parent.GetComponent<FakeHeightObject>().groundVelocity;


       // oldGroundVel = swingNum == 3 ? oldGroundVel : oldGroundVel * 2;

        Vector2 newGroundVel = Vector2.MoveTowards(enemy.transform.position, transform.position, (hitForce * 0.75f));

        //enemy.transform.parent.GetComponent<FakeHeightObject>().groundVelocity = new Vector2(-oldGroundVel.x, -oldGroundVel.y)*(hitForce*0.75f);
        enemy.transform.parent.GetComponent<FakeHeightObject>().groundVelocity = newGroundVel;
        enemy.transform.parent.GetComponent<FakeHeightObject>().verticalVelocity = hitForce*30f;

        StartCoroutine(enemy.GetComponent<Enemy>().Whacked(hitForce));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCirclePos.position, attackRange);
        //Gizmos.DrawWireSphere(new Vector3(transform.position.x + xPosCircle, transform.position.y + yPosCircle), attackRange);

    }

}
