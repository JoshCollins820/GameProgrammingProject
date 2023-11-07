using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStuff : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Dummy Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(player.GetComponent<PlayerStats>().playerStamina > 0) 
                player.GetComponent<PlayerStats>().playerStamina -= 5;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (player.GetComponent<PlayerStats>().playerStamina < 100)
                player.GetComponent<PlayerStats>().playerStamina += 5;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            player.GetComponent<PlayerStats>().playerDamaged = true;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            player.GetComponent<PlayerStats>().playerDamaged = false;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            player.GetComponent<PlayerUI>().DisplayInteractUI("Test");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            player.GetComponent<PlayerUI>().DisableInteractUI();
        }
    }
}
