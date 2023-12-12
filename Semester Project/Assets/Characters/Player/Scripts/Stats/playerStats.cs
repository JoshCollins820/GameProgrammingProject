using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // PLAYER STATS
    [Header("Health")] // HEALTH section -----------------------
    public bool playerDamaged = false;                          // indicates if the player is currently damaged
    public bool playerDead = false;                             // indicates if the player is alive/dead
    public bool deathSequence = false;                          // if the death sequence has begun, makes sure the player can only die once
    public bool isRecovering = false;                           // indicates if player is in process of being healed
    public float recoverTime = 10f;                             // time it takes to recover from being damaged
    public bool canBeHurt = true;                               // if the player can be hurt
    public float invinc_period = 1f;                            // period of time in seconds that player is invincible
    public AudioSource audio_hit;
    public AudioSource audio_die;

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

    [Header("Other")] // OTHER section -------------------------
    public bool throwMode = false;                              // indicates if player is in throw mode
    public Animator anim;
    public bool enemiesSpawned = false;                         // indicates when enemies are spawned

    [Header("Throwable")] // THROWABLE section -----------------
    public GameObject rockPrefab;                               // rock prefab
    public Transform rockSpawn;                                 // rock spawn location
    public float rock_speed = 100;                               // rock speed
    public bool canThrow = true;                                // can throw rock?
    public AudioSource audio_rockThrow;

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
        //rockSpawn = GameObject.Find("RockSpawn").transform;
        audio_rockThrow = GameObject.Find("Throw_Sound").GetComponent<AudioSource>();
        audio_hit = GameObject.Find("Hit_Sound").GetComponent<AudioSource>();
        audio_die = GameObject.Find("Die_Sound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning == false) // if player isn't running
        {
            staminaRegenerate(); // regen stamina
        }
        // if player is damaged, start healing timer
        if (playerDamaged == true && isRecovering == false && playerDead == false)
        {
            isRecovering = true; // player is healing
            Invoke(nameof(healPlayer), 10); // heal player after 10 seconds
        }
        // throw mode: if player presses a specific button (currently F) switch to throw mode
        // if they are not hiding, 
        if (Input.GetKey(KeyCode.F) && player.GetComponent<PlayerController>().hiding == false && canMove == true)
        {
            if (player.GetComponent<PlayerInventory>().count_rock > 0 && canThrow == true)
            {
                //play rock throw sound effect
                audio_rockThrow.Play();
                canThrow = false; // make it so player can't throw again until cooldown is met
                Invoke(nameof(ThrowRock),0.58f); // call the throw rock function
                player.GetComponent<PlayerController>().walkSpeed = 0f;
                player.GetComponent<PlayerController>().sprintSpeed = 0f;
                anim.SetTrigger("Throw");// trigger throw animation
                player.GetComponent<PlayerUI>().DisableInteractUI(); // hidetext
            }
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

    // way to turn off invicinciblity
    public void canBeHurtToggle()
    {
        canBeHurt = true;
    }

    // public API function to damaging player
    public void damagePlayer()
    {
        // if player is not damaged
        if (playerDamaged == false && canBeHurt == true)
        {
            audio_hit.Play();
            playerDamaged = true ; // player is now damaged
            canBeHurt = false;
            Invoke(nameof(canBeHurtToggle), invinc_period);
        }
        if(playerDamaged == true && playerDead == false && canBeHurt == true)
        {
            audio_die.Play();
            GameObject.Find("Priest").GetComponent<EnemyAIFSMTest>().DoNothing(); // stop coroutines
            GameObject.Find("Normal VCamera").GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = 85; // set fov to 85
            playerDead = true; // enable dead
            canMove = false; // disable movement
            anim.SetTrigger("Dead");// trigger throw animation
            Invoke(nameof(killPlayer),4);
            Invoke(nameof(canBeHurtToggle), invinc_period);
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

    public void killPlayer()
    {

        // Lock mouse onto screen
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Death Scene"); // swap to death scene
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

    // enables canthrow so that player can throw again
    private void EnableCanThrow()
    {
        canThrow = true; // reset can throw to true so that player can throw rock
        anim.ResetTrigger("Throw");
        player.GetComponent<PlayerController>().walkSpeed = 2f;
        player.GetComponent<PlayerController>().sprintSpeed = 7f;
    }

    // throws a rock projectile
    private void ThrowRock()
    {
        Debug.Log("Rock thrown");
        player.GetComponent<PlayerInventory>().count_rock -= 1; // decrease rock count
        var rock = (GameObject)Instantiate(
                rockPrefab,
                rockSpawn.position,
                rockSpawn.rotation); // instantiate rock
        rock.GetComponent<Rigidbody>().velocity = rock.transform.forward * rock_speed; // apply force to rock
        rock.GetComponent<RockThrow>().enableRockMidAir();
        Invoke(nameof(EnableCanThrow), 0.3f); // rock throw cooldown
    }

    public void enableEnemiesSpawned()
    {
        enemiesSpawned = true;
    }
}
