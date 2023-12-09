using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseForge : MonoBehaviour
{
    public GameObject SwordPieces;
    public GameObject Player;

    public bool interacting;
    public bool reforge;

    // Start is called before the first frame update
    void Start()
    {
        SwordPieces = GameObject.Find("SwordPieces");
        Player = GameObject.Find("Magistrate").transform.GetChild(0).gameObject;
        interacting = false;
        reforge = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            interacting = true;
            
        }
    }
}
