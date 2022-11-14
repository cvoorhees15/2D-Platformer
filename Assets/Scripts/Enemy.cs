using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // Private var
    #region
    private Animator anim;    
    private float distance; // Store distance btw enemy and player
    private float intTimer;
    private bool attackMode;
    private bool cooling; // Check if enemy still has cd before next attack
    #endregion

    // Public var
    #region
    public HealthBarBehavior HealthBar;
    public Rigidbody2D rb;
    public int maxHealth;
    public int currentHealth;
    public float speed;
    public bool canMove = true;
    public float attackDistance; // Minimum distance for attack
    public float timer; // Timer for cooldown between attacks
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector]public bool inRange; // Check if player is in range
    [HideInInspector]public Transform target;
    public GameObject hotZone;
    public GameObject triggerArea;
    #endregion

    // Start explained
    #region
    /* 
     *Start is called right before the first frame update when the application is launched
     *In this case the enemies health is reset everytime the game starts and the visual health bar is updated with that info  
     */
    #endregion
    void Start()
    {
        currentHealth = maxHealth;
        HealthBar.SetHealth(currentHealth, maxHealth);
    }

    // Update function explained
    #region
    /* 
     *This is called once every time a frame is played (many times each second)
     *The update function is the heartbeat of this script. The enemy AI has two "modes" it is either attaempting to attack the player
     *or it is patrolling back and forth between two points. Here the update function is constantly evaluating which mode it is in based
     *on the information stored in the SelectTarget, and EnemyLogic methods.
     */
    #endregion
    void Update()
    {
            if (!attackMode) // This makes it so the enemy stops moving while executing an attack. This makes the animation more realistic
            {
                Move();
            }

            if (!InsideofLimits() &&!inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) // If nothing is around to trigger enemy, patrol from left to right
            {
                SelectTarget();
            }

            if (inRange) // this block de-aggros the enemy when the player runs out of range
            {
                EnemyLogic();
            }
        
    }

    // Awake explained
    #region
    /*
     *Awake is called during the loading process anything done in this function will be applied before the game starts
     *In this case the enemy identifies the two points it will be patrolling between and selects them as targets. It also gets the timer 
     *set to go that will be used as cooldown time between each attack. Lastly it links this script to the enemies animations in Unity.
     */
    #endregion
    void Awake()
    {
        SelectTarget();
        intTimer = timer;
        anim = GetComponent<Animator>();
    }
    // EnemyLogic explained
    #region
    /*
     * This is the method that controls the enemy after he coems into range of the player. If the player is in his aggro range he must first walk towarsd the player and get into
     * attack range before he can execute an attack. Once he gets in attack range he will attack if he is not currently on cooldown from his last attack. If he is on cooldown
     * he will stand idle and wait until the cooldown period has ended. In this case I played with the cooldown time in unity and its a little over one second.
     */
    #endregion
    void EnemyLogic() // Called when the player comes into predetermined range 
    {
        distance = Vector2.Distance(transform.position, target.position);

            if (distance > attackDistance) // if the player is in aggro range but not close enough to attack walk towards player
            {
                StopAttack();              
            }

            else if (attackDistance >= distance && cooling == false)
            {           
                Attack();
            }

            if (cooling)
            {
                Cooldown();
                anim.SetBool("Attack", false);
            }
        }

    // Move explained
    #region
    /*
     * This method is what is executed everytime the enemy is moving somewhere. It must first identify whether its target is the player (if he's in range)
     * or one of his patrol points that he goes back and forth with. It also must identify if an attack is currently happening. If it's not, then it can move 
     * towards whatever the target is
     */
    #endregion
    void Move() // Moves enemy if not already in the attack animation
    {
        anim.SetBool("isWalking", true);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {          
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, 5f);
        }
    } 

    void Attack()
    {
        timer = intTimer; // Reset timer when player enters attack range
        attackMode = true; // check if enemy can still attack or not
        anim.SetBool("isWalking", false);
        anim.SetBool("Attack", true);
    }

    void StopAttack() // Cancels an attack
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("Attack", false);
    }

    public void TriggerCooling() // Event called at the end of the attack animation to start cd timer
    {
        cooling = true;
    }

    void Cooldown() // Calculates time for cd
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    // Checks if Enemy has broken the bounds of its patrol area by chasing the player so that it can return to the patrol if player runs away
    private bool InsideofLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void SelectTarget () // Patroles back and forth between left and right limits
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        Flip();
    }

    public void Flip () // Flips enemy sprite
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        {
            rotation.y = 180f;            
        }
        else
        {
            rotation.y = 0f;
        }

        transform.eulerAngles = rotation;
    }

    // TakeDamage explained
    #region
    /*
     * This method is called in the PlayerAttack script to deal damage to the enemy when the player hits him. This method plays an
     * animation, doesn't allow walking to occur during that animation, subtracts one damage from health total and updates the health bar with that data
     * It also checks if health has reached zero each time it is run so that it can reference the Die method.
     */
    #endregion
    public void TakeDamage(int damage)
    {
        canMove = false; // stop moving
        anim.SetTrigger("Hurt"); // Play "isHit" animation
        anim.SetBool("isWalking", false); // Stop the walking animation
        currentHealth -= damage;
        HealthBar.SetHealth(currentHealth, maxHealth);
        Debug.Log("Enemy took damage. Health is " + currentHealth);

        if (currentHealth <= (maxHealth)*0.5)
        {
            anim.SetBool("inPhase2", true);
            speed = speed * 2;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void TakingDamageFinished() // Animation event called every time the take damage animation ends
    {
        anim.SetBool("isWalking", true); // Resume "walking" animation after "take damage" animation plays
        canMove = true; // start moving again
    }

    void Die() // Play death animation and delete enemy
    {
        Debug.Log("enemy died.");
        anim.SetBool("isDead", true);
        anim.SetBool("isWalking", false);
        this.enabled = false;
    }

    void DestroyObject()
    {
        gameObject.SetActive(false);
    }

    void ActivateSpell()
    {
        anim.SetTrigger("castSpell");
    }
}