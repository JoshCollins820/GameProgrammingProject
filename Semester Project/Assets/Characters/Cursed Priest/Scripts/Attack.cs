using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public bool inReach = false;

    public bool IsInReach() { return inReach; }


    // Update is called once per frame
    void Update()
    {
        
    }

    // Player enters enemy's attack zone
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Attack zone entered.");
            inReach = true;
        }
    }

    // Player leaves enemy's attack zone
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Attack zone exited.");
            inReach = false;
        }
    }
}
