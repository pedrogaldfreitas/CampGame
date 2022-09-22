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
    private Transform playerParent;
    public float attackRange;

    public Animator attackAnimator;

    private playerInventory playerInventory;

    private PlayerFacing PlayerFacing;

    public float xPosCircle;
    public float yPosCircle;

    public int damage;

    public int swingNum;
    public float swingNumTimeBeforeReset;
    private Coroutine swingNumResetCoroutine;

    public bool blockSwinging;
    public float dashForwardSpeed;
    public float dashForwardSpeedDecreaseRate;
    public float finalSwingSpeedStill;
    public float finalSwingSpeedDash;
    IEnumerator currentSwingStickCoroutine;

    private void Start()
    {
        timeBtwAttack = 0;
        attackPos = GameObject.Find("AttackPos");
        stickObj = transform.Find("Stick");
        PlayerFacing = GetComponent<PlayerFacing>();
        playerParent = this.transform.parent;

        playerInventory = this.GetComponent<playerInventory>();
        swingNum = 0;
        currentSwingStickCoroutine = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInventory.activePrimary == "STICK")
        {
            if ((Input.GetKeyDown(KeyCode.Z) == true) && !blockSwinging)
            {
                if (currentSwingStickCoroutine != null)
                {
                    StopCoroutine(currentSwingStickCoroutine);
                }
                StartCoroutine(swingStick());
                timeBtwAttack = startTimeBtwAttack;
            }

            if (timeBtwAttack > 0)
            {
                timeBtwAttack -= Time.deltaTime;
            }
            else if (timeBtwAttack <= 0)
            {
                timeBtwAttack = 0;
            }
        }
    }

    public void BlockSwinging(int blockOrNot)
    {
        blockSwinging = blockOrNot == 1 ? true : false;
        return;
    }

    public void ToggleEnablePlayerMovement(int enableOrNot)
    {
        switch (enableOrNot) {
            case 0:
                transform.parent.GetComponent<PlayerController>().disableMovement();
                break;
            case 1:
                transform.parent.GetComponent<PlayerController>().enableMovement();
                break;
        }
        return;
    }

    public void SetSwingAnimationSpeed(float speed)
    {
        attackAnimator.SetFloat("AttackSpeed", speed);
        return;
    }

    public void HitEnemiesInSwingZone()
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("Enemy"));
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = true;
        List<Collider2D> enemiesToDamage = new List<Collider2D>();
        Physics2D.OverlapCollider(attackCirclePos.GetComponent<PolygonCollider2D>(), contactFilter, enemiesToDamage);

        for (int i = 0; i < enemiesToDamage.Count; i++)
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
    }

    public void ResetSwingNum()
    {
        swingNum = 0;
        return;
    }

    IEnumerator swingNumResetTimer()
    {
        yield return new WaitForSeconds(swingNumTimeBeforeReset);
        ResetSwingNum();
    }

    IEnumerator swingStickPlayerMovement(Vector2 dirOfMovement, int swingNum)
    {
        float speed = swingNum != 2 ? dashForwardSpeed : dashForwardSpeed * 1.4f;
        float speedDecreaseRate = swingNum != 2 ? dashForwardSpeedDecreaseRate : dashForwardSpeedDecreaseRate * 0.7f;

        while (speed > 0)
        {
            float step = speed * Time.deltaTime;
            Vector2 positionToMoveTo = (Vector2)playerParent.position + dirOfMovement * 15;
            transform.parent.position = Vector2.MoveTowards(transform.parent.position, positionToMoveTo, step * 2f);
            speed -= speedDecreaseRate;
            yield return new WaitForEndOfFrame();
        }

        currentSwingStickCoroutine = null;
    }

    IEnumerator swingStick()
    {
        attackAnimator.SetFloat("SwingNum", swingNum);
        attackAnimator.SetTrigger("Zkey");        
        
        Vector2 directionOfMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (swingNum == 2)
        {
            if (directionOfMovement == new Vector2(0, 0))
            {
                SetSwingAnimationSpeed(finalSwingSpeedStill);
            }
            else
            {
                SetSwingAnimationSpeed(finalSwingSpeedDash);
            }
        } else if (swingNum < 2)
        {
            SetSwingAnimationSpeed(1);
        }

        if (swingNumResetCoroutine != null)
        {
            StopCoroutine(swingNumResetCoroutine);
        }
        swingNumResetCoroutine = StartCoroutine(swingNumResetTimer());


        yield return new WaitForSeconds(0.08f);


        //Animation

        if (currentSwingStickCoroutine != null)
        {
            StopCoroutine(currentSwingStickCoroutine);
        }

        adjustStickTransform();
        currentSwingStickCoroutine = swingStickPlayerMovement(directionOfMovement, swingNum);
        StartCoroutine(currentSwingStickCoroutine);

        if (swingNum == 2)
        {
            swingNum = 0;
        }

        swingNum++;
    }

    void adjustStickTransform()
    {
        switch (swingNum)
        {
            case 1:
                stickObj.localPosition = new Vector3(0.41f, 1.39f, -0.5f);
                stickObj.GetComponent<SpriteRenderer>().flipX = false;
                break;
            case 2:
                stickObj.localPosition = new Vector3(-0.36f, 1.28f, -0.5f);
                stickObj.GetComponent<SpriteRenderer>().flipX = true;
                break;
            case 3:
                stickObj.localPosition = new Vector3(0.41f, 1.39f, -0.5f);
                stickObj.GetComponent<SpriteRenderer>().flipX = false;
                break;
        }

        return;
    }

    private void hitEnemy(Collider2D enemy)
    {
        Debug.Log("hitEnemy works.");

        float hitForce = swingNum == 2 ? 2 : 0.7f;
        damage = swingNum == 2 ? damage * 2 : damage;

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

}
