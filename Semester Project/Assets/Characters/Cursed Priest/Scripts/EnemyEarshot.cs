using UnityEngine;

public class EnemyEarshot : MonoBehaviour
{
    public bool inEarshot = false;     // enemy can hear player
    private Vector3 lastHeard;

    public bool IsInEarshot() {  return inEarshot; }
    
    public Vector3 GetLastHeardPos() { return new Vector3(-25f, 4.103967f, 20f); /*lastHeard;*/ }  // temp test values

    private void OnTriggerEnter(Collider other)
    {
        // TODO sound trigger
        if (other.tag == "Sound")  
        {
            Debug.Log("You have been heard.");
            inEarshot = true;                                       // player entered enemy earshot
            lastHeard = other.GetComponent<Transform>().position;   // save last heard position
            Invoke(nameof(mustHaveBeenTheWind), 5f);                // time out, player is no longer in earshot
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Sound")
        {
            Debug.Log("Exiting enemy earshot.");
            inEarshot = false;      // player exited enemy earshot
        }
    }

    private void mustHaveBeenTheWind()
    {
        if (inEarshot == true)
        {
            Debug.Log("Must have been the wind.");
            inEarshot = false;
        }
    }
}