using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public int touchDamage;
    public int enemyHealth;
    private int baseEnemyHealth;

    //Player damage variables
    public float knockbackPower;

    //Knockback Variables
    public float knockbackVerticalVel;
    public float knockbackGroundVel;

    private Transform parent;
    private Rigidbody2D parentRB;
    public bool movementBlocked;
    private Transform playerShadow;

    //A light enemy can be launched back by being wacked with a stick. (Raccoon is light, bear is not)
    public bool lightEnemy;

    Transform healthBar;

    private void Start()
    {
        baseEnemyHealth = enemyHealth;
        movementBlocked = false;
        parent = transform.parent;
        healthBar = transform.Find("HealthBar").Find("Filled");
        parentRB = parent.GetComponent<Rigidbody2D>();
        playerShadow = GameObject.Find("Player").transform.Find("Shadow");
        //Physics2D.IgnoreCollision(transform.parent.Find("Shadow").GetComponent<BoxCollider2D>(), GameObject.Find("Player").transform.Find("Shadow").GetComponent<BoxCollider2D>(), true);
    }

    void Update()
    {
        if (enemyHealth <= 0) {
            Destroy(transform.parent.gameObject);
        }
        if (parent.GetComponent<FakeHeightObject>().isGrounded)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Player") && (collision.GetType() == typeof(BoxCollider2D)) && (CheckForBaseCollision(collision)))
        {
            DamageOther(collision.gameObject, touchDamage);
        }
    }

    public void TakeDamage(int hitForce, Vector2 direction, float groundSpeedToLaunch)
    {
        enemyHealth -= hitForce;
        healthBar.localScale = new Vector3(((float)enemyHealth / (float)baseEnemyHealth)*16f, healthBar.localScale.y, healthBar.localScale.z);

        StartCoroutine(GetKnockedBack(hitForce, direction, groundSpeedToLaunch));

        return;
    }

    //This runs when the enemy is hit and knocked back.
    public IEnumerator GetKnockedBack(int hitForce, Vector2 direction, float groundSpeedMultiplier)
    {
        Transform playerParentTransform = GameObject.Find("Player").transform;

        movementBlocked = true;
        Transform landTarget = parent.Find("LandTarget");
        Vector2 moveSpot = landTarget.position - playerParentTransform.Find("LandTarget").position;
        parent.GetComponent<FakeHeightObject>().Jump(moveSpot*knockbackGroundVel* groundSpeedMultiplier, knockbackVerticalVel * hitForce/3.5f);

        while (!parent.GetComponent<FakeHeightObject>().isGrounded)
        {
            yield return new WaitForEndOfFrame();
        }

        movementBlocked = false;
    }

    //Currently only works when damaging the player.
    public void DamageOther(GameObject other, int dmg)
    {
        if ((other.name == "Player") && (other.GetComponent<ObjectProperties>().invincible == false))
        {
            other.GetComponent<PlayerHealth>().DamagePlayer(dmg, transform.position, knockbackPower);
            other.GetComponent<ObjectProperties>().invincible = true;
        }
        return;
    }

    //Checks if the criteria is filled for a collision with another object. (shadows collide + objects collide)
    bool CheckForBaseCollision(Collider2D collision)
    {
        Collider2D thisCollider = transform.parent.Find("LandTarget").GetComponent<BoxCollider2D>();
        Collider2D otherCollider = collision.transform.parent?.Find("LandTarget")?.GetComponent<BoxCollider2D>();
        bool bothCollidersExist = thisCollider != null && otherCollider != null;
        bool collidersAreNotTheSame = (thisCollider != otherCollider);

        if (bothCollidersExist && collidersAreNotTheSame && Physics2D.Distance(thisCollider, otherCollider).isOverlapped)
        {
            return true;
        }
        return false;
    }

    public IEnumerator Whacked(float force)
    {
        float rotateAmount = force*5;
        int negativeMultiplier;
        float slowDown = 0.01f;

        if (parent.GetComponent<FakeHeightObject>().groundVelocity.x < 0)
        {
            negativeMultiplier = 1;
        } else
        {
            negativeMultiplier = -1;
        }

        if (force >= 2)
        {
            while (!parent.GetComponent<FakeHeightObject>().isGrounded)
            {
                transform.Rotate(0f, 0f, (rotateAmount * negativeMultiplier) - (slowDown * negativeMultiplier));

                slowDown *= 1.15f;
                slowDown = Mathf.Clamp(slowDown, -rotateAmount+1, rotateAmount-1);
                yield return new WaitForSeconds(0.01f);
            }
        }

    }
}
