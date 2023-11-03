using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    // objects
    public GameObject player; // player
    public GameObject stamina_bar; // stamina ui element
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Dummy Player");
        stamina_bar = GameObject.Find("stamina_bar");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
