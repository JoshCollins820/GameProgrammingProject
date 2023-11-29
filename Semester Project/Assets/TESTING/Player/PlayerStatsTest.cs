using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsTest : MonoBehaviour
{
    // PLAYER STATS
    [Header("Health")] // HEALTH section -----------------------
    public bool playerDamaged = false;                          // indicates if the player is currently damaged
    public bool playerDead = false;                             // indicates if the player is alive/dead
    public bool deathSequence = false;                          // if the death sequence has begun, makes sure the player can only die once
    public bool isRecovering = false;                           // indicates if player is in process of being healed
    public float recoverTime = 10f;                             // time it takes to recover from being damaged

    [Header("Stamina")] // STAMINA section ---------------------
    public float playerStamina = 100f;                          // current stamina
    public float maxStamina = 100f;                             // max stamina
    public bool isExausted = false;                             // indicates if stamina bar is fully regenerated/player is able to use stamina or not
    [SerializeField] private float staminaDrain = 0.5f;         // rate at which stamina drains
    [SerializeField] private float staminaRegen = 0.5f;         // rate at which stamina recovers 

    [Header("Movement")] // MOVEMENT section -------------------
    public bool isWalking = false;                              // indicates if player is walking
    public bool isRunning = false;                              // indicates if player is running
    public bool canMove = false;                                // indicates if player is free to move
    public bool gameStarted = false;

    public GameObject maincamera; // player "Normal VCamera"
    public GameObject theRealMainCamera; // Main Camera
    public GameObject player; // Player

    // weapons


    // ammo



    // Start is called before the first frame update
    void Start()
    {
        theRealMainCamera = GameObject.Find("Main Camera");
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning == false) // if player isn't running
        {
            staminaRegenerate(); // regen stamina
        }
        // if player is damaged, start healing timer
        if (playerDamaged == true && isRecovering == false)
        {
            isRecovering = true; // player is healing
            Invoke(nameof(healPlayer), 5); // heal player after 10 seconds
        }
    }


    // regens stamina
    void staminaRegenerate()
    {
        // if player is not max stamina
        if(playerStamina <= maxStamina - 0.00)
        {
            playerStamina += staminaRegen * Time.deltaTime; // increase stamina
            
            if(playerStamina >= maxStamina) // once stamina is fully refilled
            {
                isExausted = false; // no longer exausted, can not run
            }
        }
    }

    // public API function to deplete stamina, parameter is amount of stamina that will be depleted
    public void useStamina()
    {
        if(isExausted == false) // if player is able to use stamina
        {
            isRunning = true; // player is running
            playerStamina -= staminaDrain * Time.deltaTime; // decrease stamina

            if(playerStamina <= 0) // if stamina runs out
            {
                isExausted = true; // player is tired
                isRunning = false; // player is not running anymore
            }
        }
    }

    // public API function to damaging player
    public void damagePlayer()
    {
        // if player is not damaged
        if (playerDamaged == false)
        {
            playerDamaged = true; // player is now damaged
        }
        if (playerDamaged == true && playerDead == false)
        {
            playerDead = true;
        }
    }

    // public API function to damaging player
    public void healPlayer()
    {
        // if player is damaged
        if (playerDamaged == true)
        {
            playerDamaged = false; // player no longer damaged
            isRecovering = false; // player is no longer recovering
        }
    }
}
