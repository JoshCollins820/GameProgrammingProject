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
        if (Input.GetKey(KeyCode.LeftShift)) // when the player holds down LSHIFT
        {
            player.GetComponent<PlayerStats>().useStamina();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) // when the player stops holding LSHIFT
        {
            player.GetComponent<PlayerStats>().isRunning = false;
        }

        if (Input.GetKeyDown(KeyCode.X)) // placeholder for event that damages player
        {
            player.GetComponent<PlayerStats>().damagePlayer();
        }
        if (Input.GetKeyDown(KeyCode.C)) // placeholder for event that heals player
        {
            player.GetComponent<PlayerStats>().healPlayer();
        }
        if (Input.GetKeyDown(KeyCode.V)) // placeholder for event where Interact E should appear
        {
            player.GetComponent<PlayerUI>().DisplayInteractUI("Test");
        }
        if (Input.GetKeyDown(KeyCode.B)) // placeholder for event where Interact E should not longer appear
        {
            player.GetComponent<PlayerUI>().DisableInteractUI();
        }
    }
}
