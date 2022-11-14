using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    // Public var: All of these are adjustable in the Unity inspector tab
    #region
    private ParticleSystem Fire1;
    private ParticleSystem Glow1;
    private ParticleSystem Spark1;
    private ParticleSystem Fire2;
    private ParticleSystem Glow2;
    private ParticleSystem Spark2;
    #endregion

    //Private var
    #region
    public static LevelManager instance;
    public Transform respawnPoint;
    public GameObject playerPrefab;
    public CinemachineVirtualCameraBase cam;
    public GameObject bonFire1;
    public GameObject bonFire2;
    #endregion

    // Awake explained
    #region
    /*
     *Awake is called during the loading process anything done in this function will be applied before the game starts
     *In this case all bonfire particle systems are deactivated before the game starts
     *They can only be reactivated when the player walks through the bonfire to set it as his new respawn point aka checkpoint
     */
    #endregion
    private void Awake() // Deactivate bon fire particle systems at the start of the game
    {
        instance = this;
        Fire1 = GameObject.Find("Fire1").GetComponent<ParticleSystem>();
        Glow1 = GameObject.Find("Glow1").GetComponent<ParticleSystem>();
        Spark1 = GameObject.Find("Spark1").GetComponent<ParticleSystem>();
        Fire2 = GameObject.Find("Fire2").GetComponent<ParticleSystem>();
        Glow2 = GameObject.Find("Glow2").GetComponent<ParticleSystem>();
        Spark2 = GameObject.Find("Spark2").GetComponent<ParticleSystem>();

        Fire1.Stop();
        Glow1.Stop();
        Spark1.Stop();
        Fire2.Stop();
        Glow2.Stop();
        Spark2.Stop();
    }

    // Respawn explained
    #region
    /*
     *This is where the actual respawn functionality is stored
     *The new player gameobject is instantiated using a prefab(saved copy of a gameobject) and the camera is reset to follow this new player
     */
    #endregion
    public void Respawn () // Used to recreate player as a prefab copy upon death
    {
        GameObject player = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        cam.Follow = player.transform;
    }

    // Checkpoint explained
    #region
    /*
     *This function is called in the PlayerMovement script when the player runs through (triggers) a gameobject tagged as a "bon fire"
     *Upon that trigger the particle systems for the fire are activated aka the fire lights and the position of the player respawn game object is moved to that bonfire
     */
    #endregion
    public void Checkpoint1 () 
    {
        respawnPoint.position = bonFire1.transform.position;
        Fire1.Play();
        Glow1.Play();
        Spark1.Play();
    }

    // Checkpoint explained
    #region
    /*
     *This function is called in the PlayerMovement script when the player runs through (triggers) a gameobject tagged as a "bon fire"
     *Upon that trigger the particle systems for the fire are activated aka the fire lights and the position of the player respawn game object is moved to that bonfire
     */
    #endregion
    public void Checkpoint2 () 
    {
        respawnPoint.position = bonFire2.transform.position;
        Fire2.Play();
        Glow2.Play();
        Spark2.Play();
    }
}
