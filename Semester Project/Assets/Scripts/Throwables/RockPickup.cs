using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPickup : MonoBehaviour
{
    public GameObject player; // player object
    public bool collected; // bool for if rock is collected by player
    private bool inVicinity;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        collected = false;
        inVicinity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(inVicinity == true)
        {
            // if player presses E to pick up rock
            if (Input.GetKey(KeyCode.E) && collected == false)
            {
                collected = true; // rock is picked up
                player.GetComponent<PlayerInventory>().count_rock += 1; // add a rock to players inventory
                Destroy(this.gameObject); // destroy object
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if player enters rock vicinity
        if(other.tag == "Player")
        {
            player.GetComponent<PlayerUI>().DisplayInteractUI("Pick up Rock"); // show text
            inVicinity = true; // player is in vicinity
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // if player leaves rock vicinity
        if (other.tag == "Player")
        {
            player.GetComponent<PlayerUI>().DisableInteractUI(); // hidetext
            inVicinity = false; // player is no longer in vicinity
        }
    }
}
