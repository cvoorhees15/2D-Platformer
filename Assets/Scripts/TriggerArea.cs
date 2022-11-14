using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    private Enemy enemyParent;

    // Awake explained
    #region
    /*
     *Awake is called during the loading process anything done in this function will be applied before the game starts
     *In this case a variable is created to reference the variables from the Enemy script
     */
    #endregion
    private void Awake()
    {
        enemyParent = GetComponentInParent<Enemy>(); 
    }

    // OnTriggerEnter2D explained
    #region
    /* 
     *OnTriggerEnter2D functions are used to identify when one game object passes through another, these objects are identified by their tags
     *In this case there is a long rectangle stretching in front on the enemies, when a game object tagged as "player" passes through it the
     *larger HotZoneCheck box collider activates surrounding the enemy and acting as it's aggro range
     */
    #endregion
    private void OnTriggerEnter2D(Collider2D collider) // Activates aggro range of hotzone
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            enemyParent.target = collider.transform;
            enemyParent.inRange = true;
            enemyParent.hotZone.SetActive(true);
        }
    }
}
