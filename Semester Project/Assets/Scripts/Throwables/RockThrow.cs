using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrow : MonoBehaviour
{
    public bool midAir; // bool for if rock is midAir
    public AudioSource audio_rockHit;

    // Start is called before the first frame update
    void Start()
    {
        midAir = true;
        audio_rockHit = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(midAir == true)
        {
            Debug.Log("TEST MIDAIR");
        }
    }

    public void enableRockMidAir()
    {
        midAir = true; // enable midAir to true
        GetComponent<SphereCollider>().enabled = false; // turn on sphere collider so player can pick it up // turn off sphere collider so player cant grab it midair
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Floor" && midAir == true) // if rock hits the floor and hasn't hit floor before
        {
            audio_rockHit.Play(); // play rock hit sound
            midAir = false; // rock is no longer mid air
            Vector3 location_hit = this.transform.position; // get pos
            Debug.Log("Rock landed at: (" + location_hit.x + "," + location_hit.y + "," + location_hit.z);
            GetComponent<SphereCollider>().enabled = true; // turn on sphere collider so player can pick it up
            // call stop routines function
            if(GameObject.Find("Player").GetComponent<PlayerStats>().enemiesSpawned == true) // if enemies are spawned
            {
                // call functions that calls Priest to location_hit
                GameObject.Find("Priest").GetComponent<EnemyAIFSMTest>().StopCoroutines();
                StartCoroutine(GameObject.Find("Priest").GetComponent<EnemyAIFSMTest>().GoToPoint(location_hit));
            }    
        }
    }
}
