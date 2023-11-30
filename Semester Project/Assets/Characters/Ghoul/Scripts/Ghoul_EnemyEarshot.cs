using UnityEngine;

public class Ghoul_EnemyEarshot : MonoBehaviour
{
    public bool inEarshot = false;     // enemy can hear player
    public bool IsInEarshot() {  return inEarshot; }

    private Vector3 lastHeard;

    private GameObject player; // For checking if player is moving


    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        // If the player is within earshot and moving
        if(inEarshot && player.GetComponent<InputsManager>().move != Vector2.zero)
        {
            // Save the current position of player (where sound was last heard)
            lastHeard = player.GetComponent<Transform>().position;
        }
    }

    // When the player enters the hearing range
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inEarshot = true;
        }
    }

    // When the player exits the hearing range
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inEarshot = false;
        }
    }

    public Vector3 GetLastHeardPos()
    {
        return lastHeard;
    }


}