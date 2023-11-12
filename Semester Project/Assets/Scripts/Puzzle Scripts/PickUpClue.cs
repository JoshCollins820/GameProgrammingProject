using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpClue : MonoBehaviour
{
    // Get any necessary GameObjects for Inventory

    public bool pickedUp;

    // Start is called before the first frame update
    void Start()
    {
        pickedUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (!pickedUp)
        {
            // pick up (make disappear and put into inventory)
            this.gameObject.SetActive(false);
            pickedUp = true;
            // TODO: Put into inventory
        }
    }
}
