using Cinemachine;
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

    public void enableMove()
    {
        canMove = true;
        theRealMainCamera.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = 0.4f;
    }

    public void disableMove()
    {
        canMove = false;
    }

    public void enableGame()
    {
        gameStarted = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Invoke("EnableMainCamera", 8f);
        Invoke("enableMove", 12f);//default 12
    }

    public void disableGame()
    {
        gameStarted = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void EnableMainCamera()
    {
        // Enable the mainCamera GameObject
        if (maincamera != null)
        {
            maincamera.SetActive(true);
        }
    }

    private void DisablePlayer()
    {

    }
}
