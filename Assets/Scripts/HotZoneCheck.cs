using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZoneCheck : MonoBehaviour
{
    // Private var
    #region
    private Enemy enemyParent;
    private bool inRange;
    private Animator anim;
    #endregion

    // Awake explained
    #region
    /*
     *Awake is called during the loading process anything done in this function will be applied before the game starts
     *In this case variables are created to reference the variables and animator from the Enemy script
     */
    #endregion
    private void Awake()
    {
        enemyParent = GetComponentInParent<Enemy>(); 
        anim = GetComponentInParent<Animator>();
    }

    // Update explained
    #region
    /* 
     *This is called once every time a frame is played (many times each second)
     *so that it is constantly evaluating when the player gives input
     *In this case it is constantly evaluating if an enemy is attacking, and
     *if the player is within attack range. If these conditions are met the enemy sprite knows it can flip
     */
    #endregion
    private void Update()
    {
        if (inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            enemyParent.Flip();
        }
    }

    // OnTriggerEnter2D explained
    #region
    /* 
     *OnTriggerEnter2D functions are used to identify when one game object passes through another, these objects are identified by their tags
     *In this case there is a large box collider around the enemy that defines the aggro range of the enemy and is triggered by anything tagged as player
     */
    #endregion
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }
    // OnTriggerExit2D explained
    #region
    /*
     *OnTriggerExit2D functions are used to identify when a box collider of a specific tag exits the range of another
     *In this case the function is detecting when something tagged as "player" exits the range of the HotZoneCheck box collider
     *When this happens the enemy stops following the player by setting inRange to false and the trigger area is set back on awaiting the player to enter it again
     */
    #endregion
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            inRange = false;
            gameObject.SetActive(false);
            enemyParent.triggerArea.SetActive(true);
            enemyParent.inRange = false;
            enemyParent.SelectTarget();
        }
    }
}
