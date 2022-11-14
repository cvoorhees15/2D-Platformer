using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    // Private var
    #region
    private Animator anim;
    #endregion

    // Public var: All of these are adjustable in the Unity inspector tab
    #region
    public HealthBarBehavior HealthBar;
    public int currentHealth;
    public int maxHealth;
    public Rigidbody2D rb;
    public bool isDead;
    #endregion

    // Start explained
    #region
    /* 
     *Start is called right before the first frame update when the application is launched
     *This is helpful to get things set how you want them when a game starts   
     */
    #endregion
    void Start()
    {
        currentHealth = maxHealth;
        HealthBar.SetHealth(currentHealth, maxHealth);
        anim = GetComponent<Animator>();
        isDead = false;
    }

    // Update function explained
    #region
    /* 
     *This is called once every time a frame is played (many times each second)
     *so that it is constantly evaluating when the player gives input
     */
    #endregion
    void Update()
    {
        if (GetComponent<Transform>().position.y < -30f) // If player falls off the map respawn
        {
            Die();
            Respawn();
        }
    }

    // OnTriggerEnter2D explained
    #region
    /* 
     *OnTriggerEnter2D functions are used to identify when one game object passes through another, these objects are identified by their tag
     *In this case it identifies when the player collider is hit by something tagged as "enemy"
     *This is used to know when to apply damage to the player which includes updating his health and playing a "hit" animation
     *It also evaluates if the player shpuld die after each hit
     */
    #endregion
    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Enemy")
        {
            currentHealth -= 1;
            HealthBar.SetHealth(currentHealth, maxHealth);
            anim.SetTrigger("Hurt");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // OnCollisionEnter2D explained
    #region
    /*
     *OnCollisionEnter2D functions are used to identify when one game object collides with another, these objects are identified by their tag
     *In this case it identifies when the player collides with a game object tagged as a "heart"
     *Upon collision the heart will disappear and give the player health if his health is not already at 100%, if his health is at 100% the heart fill float around and bounce off the player
     *There is also a different collision with a game object tagged "coin". This collision is what I use to end the level.
     */
    #endregion
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Heart 1" && currentHealth < maxHealth) // If the player is less than max health he will pick it up and gain 1 health back
        {
            currentHealth += 1;
            HealthBar.SetHealth(currentHealth, maxHealth);
            GameObject.Find("Heart 1").SetActive(false);
        }

        if (collision.gameObject.tag == "Heart 2" && currentHealth < maxHealth)
        {
            currentHealth += 1;
            HealthBar.SetHealth(currentHealth, maxHealth);
            GameObject.Find("Heart 2").SetActive(false);
        }

        if (collision.gameObject.tag == "Heart 3" && currentHealth < maxHealth)
        {
            currentHealth += 1;
            HealthBar.SetHealth(currentHealth, maxHealth);
            GameObject.Find("Heart 3").SetActive(false);
        }

        if (collision.gameObject.tag == "Coin")
        {            
            GameObject.Find("Coin").SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    // Die explained
    #region
    /*
     *This function is called when player health reachs, or goes below 0. It is also called if the player falls off the level.
     *It deactivates every aspect of the player game object and plays a death animation
     */
    #endregion
    public void Die()
    {
        isDead = true;
        Debug.Log("player died.");
        anim.SetBool("isDead", true);
        anim.SetBool("isRunning", false);
        GetComponent<PlayerMovement>().canMove = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerAttack>().enabled = false;
    }

    // Respawn explained
    #region
    /*
     *The original player gameobject that was just deactivated is now deleted fully and a new instance of the player is created at the set respawn point
     *We are able to do this because of something called a prefab. This is a saved version of a gameobject that we can basically drag and drop into a level at any time
     *This functionality behind the actual respawn is referenced from the LevelManager script so that we can use it here
     */
    #endregion
    void Respawn() 
    {
        Destroy(gameObject);
        LevelManager.instance.Respawn();
    }
}