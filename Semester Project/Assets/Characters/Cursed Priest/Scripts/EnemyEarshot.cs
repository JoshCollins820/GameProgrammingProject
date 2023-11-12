using System.Collections;
using UnityEngine;

public class EnemyEarshot : MonoBehaviour
{
    public bool inEarshot = false;     // enemy can hear player
    private Vector3 lastHeard;

    public bool IsInEarshot() {  return inEarshot; }
    
    public Vector3 GetLastHeardPos() { return new Vector3(-25f, 4.103967f, 20f); /*lastHeard;*/ }  // temp test values

    private GameObject player;



    private void Start()
    {
        player = GameObject.Find("Player");
    }


    private void Update()
    {
        // If the player is within earshot and moving
        if(inEarshot && player.GetComponent<InputsManager>().move != Vector2.zero)
        {
            Debug.Log("You have been heard!");
            // Save the current position of player (where sound was last heard)
            lastHeard = player.GetComponent<Transform>().position;

            // Something about changing to Alert state


        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            inEarshot = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            inEarshot = false;
        }
    }



    /*
    // If the player enters the enemy's hearing range
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && other.GetComponent<InputsManager>().move != Vector2.zero) // Check if the player is moving as well
        {
            Debug.Log("You have been heard.");
            inEarshot = true;                                       // player entered enemy earshot
            lastHeard = other.GetComponent<Transform>().position;   // save last heard position
            //Invoke(nameof(MustHaveBeenTheWind), 5f);                // time out, player is no longer in earshot
        }
    }

    // If the player leaves the enemy's hearing range
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Exiting enemy earshot.");
            inEarshot = false;      // player exited enemy earshot
        }
    }

    private void MustHaveBeenTheWind()
    {
        if (inEarshot == true)
        {
            Debug.Log("Must have been the wind.");
            inEarshot = false;
        }
    }
    */

}