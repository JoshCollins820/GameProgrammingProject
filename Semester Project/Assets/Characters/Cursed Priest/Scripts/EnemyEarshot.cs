using UnityEngine;

public class EnemyEarshot : MonoBehaviour
{
    public bool inEarshot = false;     // enemy can hear player
    private Vector3 lastHeard;

    public bool IsInEarshot() {  return inEarshot; }
    
    public Vector3 GetLastHeardPos() { return new Vector3(-25f, 4.103967f, 20f); /*lastHeard;*/ }  // temp test values

    private GameObject player; // For checking if player is moving


    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        // If the player is within earshot and moving
        if (inEarshot && player.GetComponent<InputsManager>().move != Vector2.zero)
        {
            Debug.Log("You have been heard!");
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
}