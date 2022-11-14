using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehavior : MonoBehaviour
{
    // Public var
    #region
    public Slider Slider;
    public Color Low;
    public Color High;
    public Vector3 Offset;
    #endregion

    // SetHealth explained
    #region
    /*
     * This method is referenced in the Enemy and PlayerHealth scripts to link their health variables to this health bar object
     * Once those floats are fed into this method the health bar object that all enemies and the player have is updated and shown
     * as a slider above their sprite. The slider is only visible after damage is taken to reduce screen clutter.
     */
    #endregion
    public void SetHealth(float health, float maxHealth)
    {
        Slider.gameObject.SetActive(health < maxHealth);
        Slider.value = health;
        Slider.maxValue = maxHealth;
        Slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, Slider.normalizedValue);
    }

    // Update function explained
    #region
    /* 
     *This is called once every time a frame is played (many times each second)
     *so that it is constantly evaluating where the parent object (either player or enemy)
     *is located so that the health bar follows them when they move
     */
    #endregion
    void Update()
    {
        Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
    }
}
