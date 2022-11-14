using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeGrab : MonoBehaviour
{
    private bool greenBox, redBox;
    private Rigidbody2D rb;
    private Animator anim;
    private float startingGrav;    

    public float redXOffset, redYOffset, redXSize, redYsize, greenXOffset, greenYOffset, greenXSize, greenYsize;
    public LayerMask groundMask;
    public bool isHanging;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startingGrav = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Two bools as size adjustable boxes placed above the players head to specify where ledge grab can  and cant occur
        greenBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (greenXOffset * transform.localScale.x), transform.position.y + greenYOffset), new Vector2(greenXSize, greenYsize), 0f, groundMask);
        redBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (redXOffset * transform.localScale.x), transform.position.y + redYOffset), new Vector2(redXSize, redYsize), 0f, groundMask);

        if (greenBox && !redBox && anim.GetBool("Grabbing") == false && anim.GetBool("isGrounded") == false) // if the green box is overlapping with another collider and the red box isnt and the player is mid jump perform ledge grab
        {
            anim.SetBool("Grabbing", true);
        }

        if (anim.GetBool("Grabbing") == true) // if ledge grab is happening player cannot move and isn't effected by gravity
        {
            rb.velocity = new Vector2(0f, 0f);
            rb.gravityScale = 0f;
            isHanging = true;
        }
        if (anim.GetBool("Grabbing") == true && Input.GetButtonDown("Jump")) // if player jumps release ledge grab and perform a jump
        {
            rb.gravityScale = startingGrav;
            anim.SetBool("Grabbing", false);
            GetComponent<PlayerMovement>().Jump();
            isHanging = false;
        }
    }

    private void OnDrawGizmosSelected() // Draw markers for where ledge grab can and cant happen
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (redXOffset * transform.localScale.x), transform.position.y + redYOffset), new Vector2(redXSize, redYsize));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (greenXOffset * transform.localScale.x), transform.position.y + greenYOffset), new Vector2(greenXSize, greenYsize));
    }
}
