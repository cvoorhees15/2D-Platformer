using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehavior : MonoBehaviour
{
    // Public var
    #region
    public Animator anim;
    public GameObject Heart;
    #endregion

    // Start explained
    #region
    /* 
     *Start is called right before the first frame update when the application is launched
     *In this case the hearts are deactivated at the start until the player accesses them through a chest
     */
    #endregion
    private void Start()
    {
        Heart.SetActive(false); // Heart not able to be picked up yet
    }

    // OnTriggerEnter2D explained
    #region
    /* 
     * OnTriggerEnter2D functions are used to identify when one game object passes through another, these objects are identified by their tags
     * In this case when objects tagged as "player" pass through a chest (aka walk past) the chest opening animation plays
     */
    #endregion
    void OnTriggerEnter2D(Collider2D trig) // When player walks past chest open chest
    {
        if (trig.gameObject.tag == "Player")
        {
            anim.SetBool("isOpen", true);           
        }
    }

    // Animation event explained
    #region
    /*
     * This is an animation event, meaning it gets called at a specific time during an animation. This event time is defined in Unity.
     * In this case the function is called at the end of the chest open animation to stop the chest animator as it is no longer needed
     * and to reactivate the heart object creating an effect looking like the chest drops the heart.
     */
    #endregion
    void DropHeart () // Stop animating after chest opens and drop a heart for the player
    {
        GetComponent<Animator>().enabled = false;
        Heart.SetActive(true);
    }
}