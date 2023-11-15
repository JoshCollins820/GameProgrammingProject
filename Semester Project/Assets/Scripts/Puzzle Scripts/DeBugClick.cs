using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeBugClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            // Log a debug message
            Debug.Log("Left mouse button pressed!");
        }
    }
}
