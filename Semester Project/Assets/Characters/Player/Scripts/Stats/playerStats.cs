using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // player stats
    public bool playerDamaged; // default: false
    public float playerStamina; // default: 100

    // weapons


    // ammo


    // bools
    public bool isWalking;
    public bool isRunning;
    private bool usingStamina;



    // Start is called before the first frame update
    void Start()
    {
        // initialize player stats
        playerDamaged = false;
        playerStamina = 100;
        isWalking = true;
        isRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if not max stamina and not using stamina
        if (playerStamina < 100 && usingStamina == false)
        {
            staminaRegen();
        }
    }


    // regens stamina
    void staminaRegen()
    {
        playerStamina += 10; // increase stamina, subject to change, I want to increase it with a sigmoid-like function

        // if stamina overflows over max
        if (playerStamina > 100) 
        {
            playerStamina = 100; // reset to 100
        }
    }

    // public API function to deplete stamina, parameter is amount of stamina that will be depleted
    public void useStamina(float stamina_lost)
    {
        // if player has stamina left
        if (playerStamina > 0)
        {
            playerStamina -= stamina_lost; // decrease stamina
        }

        // if stamina goes under min
        if (playerStamina < 0)
        {
            playerStamina = 0; // reset to 0
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
