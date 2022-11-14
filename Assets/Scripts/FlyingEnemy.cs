using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingEnemy : MonoBehaviour
{
    private Animator anim;

    public static FlyingEnemy instance;
    public AIPath aiPath;
    public int maxHealth;
    public int currentHealth;
    public HealthBarBehavior HealthBar;

    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        HealthBar.SetHealth(currentHealth, maxHealth);
    }

    void Update()
    {
        // Flip sprite based on direction enemy is traveling
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(3f, 3f, 1f);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(-3f, 3f, 1f);
        }
    }

    void Die()
    {
        Debug.Log("enemy died.");
        anim.SetBool("isDead", true);       
        this.enabled = false;
        aiPath.gravity.Set(0f, -9.81f, 0f);
    }

    void DestroyObject ()
    {
        gameObject.SetActive(false);
    }
}
