using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Private var
    #region
    private float timeBtwAttack;
    private bool isAttacking = false;    
    #endregion

    // Public var: All of these are adjustable in the Unity inspector tab
    #region
    public Transform attackPos;
    public bool isHit = false;
    public float startTimeBtwAttack;
    public float attackRange;
    public int damage;
    public LayerMask whatIsEnemies;
    public Animator anim;
    public Rigidbody2D rb;
    #endregion

    // Update function explained
    #region
    /* 
     *This is called once every time a frame is played (many times each second)
     *so that it is constantly evaluating when the player gives input
     */
    #endregion
    void Update()
    {
        // Reference to PlayerMovement explained
        #region
        /* 
         *This is where I reference the canMove bool from the PlayerMovement script.
         *I also am accessing the PlayerLedgeGrab script so that I dont have to make this same if statement in that script when the player is hanging off a ledge
         *This is basically saying if the player is attacking, being attacked, or hanging off a ledge the player cannot move
         */
        #endregion
        if (isAttacking == true || isHit == true || GetComponent<PlayerLedgeGrab>().isHanging == true)
        {
            GetComponent<PlayerMovement>().canMove = false;
        }
        else
        {
            GetComponent<PlayerMovement>().canMove = true;
        }

        // Player Attack system explained
        #region
        /* 
         *This is an essential block of code to the game, this is where enemies being hit are identified and damage is applied to them
         *I use an OverlapCircleAll to define a circle in front of the player that is effectively his attack range used to identify anything overlapping it that is on the "enemy layer"
         *When the player presses attack (right shift) all enemies (objects on the enemy layer) overlapping with the circle are loaded into an array that I loop though to apply damage to each
         *Damage is applied by referencing the TakeDamage function from the Enemy script
         *There is also a cooldown system used here with Time.deltatime just like the roll function, it determines if enough time has passed since the last attack to allow another one to be executed
         */ 
        #endregion
        if (timeBtwAttack <= 0) // Then you can attack
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
            {          
                Attack();
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies); // Attack range detecting if you hit enemy
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage); // Deal damage to all enemies within hit range                    
                }
                timeBtwAttack = startTimeBtwAttack;               
            }          
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;           
        }
    }

    void Attack() // Function to play attack animation and notify in the code that an attack is happening
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
    }

    private void OnDrawGizmosSelected() // Function to show area of effect for the overlap circle that defines attack range
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    // Animation Event
    #region
    /*
     *Animation event (function called at some point during an animation)
     *Called at the end of the attack animation
     */
    #endregion
    void AttackFinished()
    {
        isAttacking = false;
    }
}
