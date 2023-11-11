using UnityEngine;

public class EnemyEyesight : MonoBehaviour
{
    public bool inSight = false;   // enemy can see player

    public bool IsInSight() { return inSight; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            Debug.Log("You've been seen.");
            inSight = true;                     // player entered enemy line of sight
            Invoke(nameof(ImOuttaHere), 2f);    // time out, player is no longer in line of sight
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Exiting enemy line of sight.");
            inSight = false;    // player exited enemy line of sight
        }
    }

    private void ImOuttaHere()
    {
        if (inSight == true)
        {
            Debug.Log("I'm outta here");
            inSight = false;
        }
    }
}