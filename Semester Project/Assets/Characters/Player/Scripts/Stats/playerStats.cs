using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStats : MonoBehaviour
{
    // player stats
    public bool playerDamaged; // default: false
    public float playerStamina; // default: 100

    // weapons


    // ammo


    // bools
    public bool isWalking;
    public bool isRunning;





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
        
    }
}
