using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // PLAYER STATS
    [Header("Health")] // HEALTH section -----------------------
    public bool playerDamaged = false;                          // indicates if the player is currently damaged

    [Header("Stamina")] // STAMINA section ---------------------
    public float playerStamina = 100f;                          // current stamina
    public float maxStamina = 100f;                             // max stamina
    public bool isExausted = false;                             // indicates if stamina bar is fully regenerated/player is able to use stamina or not
    [SerializeField] private float staminaDrain = 0.5f;         // rate at which stamina drains
    [SerializeField] private float staminaRegen = 0.5f;         // rate at which stamina recovers 

    [Header("Movement")] // MOVEMENT section -------------------
    public bool isWalking = false;                              // indicates if player is walking
    public bool isRunning = false;                              // indicates if player is running

    // weapons


    // ammo



    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning == false)
        {
            staminaRegenerate();
        }
    }


    // regens stamina
    void staminaRegenerate()
    {
        // if player is not max stamina
        if(playerStamina <= maxStamina - 0.00)
        {
            playerStamina += staminaRegen * Time.deltaTime; // increase stamina
            
            if(playerStamina >= maxStamina)
            {
                isExausted = false;
            }
        }
    }

    // public API function to deplete stamina, parameter is amount of stamina that will be depleted
    public void useStamina()
    {
        if(isExausted == false) // if player is able to use stamina
        {
            isRunning = true;
            playerStamina -= staminaDrain * Time.deltaTime;

            if(playerStamina <= 0)
            {
                isExausted = true;
                isRunning = false;
            }
        }
    }

    // public API function to damaging player
    public void damagePlayer()
    {
        // if player is not damaged
        if (playerDamaged == false)
        {
            playerDamaged = true ; // player is now damaged
        }
    }

    // public API function to damaging player
    public void healPlayer()
    {
        // if player is damaged
        if (playerDamaged == true)
        {
            playerDamaged = false; // player no longer damaged
        }
    }
}
