using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrow : MonoBehaviour
{
    public bool midAir; // bool for if rock is midAir

    // Start is called before the first frame update
    void Start()
    {
        midAir = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(midAir == true)
        {
            Debug.Log("TEST MIDAIR");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // other.tag == "Floor"
        if (midAir == true) // if rock hits the floor and hasn't hit floor before
        {
            midAir = false; // rock is no longer mid air
            Vector3 location_hit = this.transform.position;
            Debug.Log("Rock landed at: (" + location_hit.x + "," + location_hit.y + "," + location_hit.z);
            // call function that calls Priest to location_hit
        }
    }
}
