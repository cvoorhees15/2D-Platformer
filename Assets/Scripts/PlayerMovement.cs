using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Private var
    #region
    private float timeBtwRoll;
    private float movementX;
    #endregion

    // Public var: All of these are adjustable in the Unity inspector tab
    #region
    public static PlayerMovement instance;
    public float startTimeBtwRoll; // number making it so roll can't be spammed
    public float jumpForce = 20f; // number determining player jump height
    public float movementSpeed; // number determining player movement speed
    public bool canMove = true;
    public Rigidbody2D rb;
    public Transform Feet;
    public Animator anim;
    public LayerMask groundLayers;
    #endregion

    private void Awake()
    {
        instance = this;
    }
    // Update function explained
    #region
    /* 
     *This is called once every time a frame is played (many times each second)
     *so that it is constantly evaluating when the player gives input
     */
    #endregion
    private void Update()
    {
        // canMove bool explained
        #region
        /*
         * canMove is accessed by other scripts to deactivate the movement system when the player is doing other things such as attacking or ledge hanging
         */
        #endregion
        if (canMove == true) 
        {           
            movementX = Input.GetAxisRaw("Horizontal"); // Establish that movement can only happen left to right not up and down
          
            // Jumping
            if (Input.GetButtonDown("Jump") && IsGrounded()) // if player presses jump while they are on the ground then do a jump
            {
                Jump(); 
            }

            // Running explained
            #region
            /* 
             *if player is moving along x axis in either direction play running animation.
             *Mathf.Abs is used to take the absolute value because moving to the left would return a negative x value
             */
            #endregion
            if (Mathf.Abs(movementX) > 0.05f) 
            {
                // Accesses the players animator and starts the running animation
                anim.SetBool("isRunning", true);
            }
            else
            {
                // Accesses the players animator and stops the running animation
                anim.SetBool("isRunning", false);
            }

            // Flip if statement explained
            #region
            /*
             *This if statement flips the players sprite dependent on what direction he is moving
             *if the player is moving to along the x axis to the right face right and vice versa
             */
            #endregion
            if (movementX > 0f) // if player is moving along x axis to the right face player right
            {
                transform.localScale = new Vector3(1f, 1f, 1f); // Flip the sprite
            }
            else if (movementX < 0f) // if player is moving along x axis to the left face player left
            {
                transform.localScale = new Vector3(-1f, 1f, 1f); // Flip the sprite
            }

            // Rolling explained
            #region
            /*
             *Roll has a cooldown so that it cant be spammed, this cooldown is stored in timeBtwRoll
             *Time.deltaTime is used as a timer to evaluate if the proper amount of time since the last roll has passed so that the player can roll again
             */
            #endregion
            if (timeBtwRoll <= 0)
            
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Roll();
                    timeBtwRoll = startTimeBtwRoll;
                }
            }
            else 
            {
                timeBtwRoll -= Time.deltaTime;
            }
        }
        anim.SetBool("isGrounded", IsGrounded()); // Constantly assess if player is grounded
    }

    // FixedUpdate function explained
    #region
    /* 
     *This is called once every time a FIXED frame is played, called less times each second than Update
     *Best practice is to use Update, but this can be utilized sometimes to help improve efficiency
     */
    #endregion
    private void FixedUpdate()
    {
        if (canMove == true)
        {            
            Vector2 movement = new Vector2(movementX * movementSpeed, rb.velocity.y); // Moves player along x axis at a defined speed (see movementSpeed^)
            rb.velocity = movement;
        }
    }
    // Jump explained
    #region
    /* 
     *Moves player along y axis using a defined amount of force (see float jumpForce^)
     *Keeps in mind rb.velocity.x which is the horizontal movement that is already occuring before/during the jump
     *This is what allows the player to jump over a gap and not just straight up and down
     */
    #endregion
    public void Jump() 
    {
        Vector2 movement = new Vector2(rb.velocity.x, jumpForce); 
        rb.velocity = movement;
    }

    // IsGrounded explained
    #region
    /*
     *Seperate collider is used in Unity to define the bottom edge of the player (aka his feet)
     *if the ground can be identified return true if not return false
     */
    #endregion
    public bool IsGrounded()
    {
        Collider2D groundCheck = Physics2D.OverlapCircle(Feet.position, 0.3f, groundLayers);

        if (groundCheck != null)  
        {
            return true;
        }
        return false;
    }

    void Roll() // Play roll animation which also shrinks the player collider height
    {
        anim.SetTrigger("Roll");
    }

    // OnTriggerEnter2D explained
    #region
    /* 
     *OnTriggerEnter2D functions are used to identify when one game object passes through another, these objects are identified by their tags
     *In this case it identifies when the player runs through an object tagged as a "bon fire" so that the respawn point can be moved there
     *The actual functionality to do this is referenced from the LevelManager script
     */
    #endregion
    private void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Bon Fire 1")
        {
            LevelManager.instance.Checkpoint1();
        }

        if (trig.gameObject.tag == "Bon Fire 2")
        {
            LevelManager.instance.Checkpoint2();
        }
    }
}
