using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPipe : MonoBehaviour
{
    public GameObject Player;
    public GameObject MissingCornerPipe;
    public GameObject FixedMissingCornerPipe;

    public bool interacting;
    public bool pipeFixed;
    public bool hasPipe;
    public bool showAction;



    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Magistrate").transform.GetChild(0).gameObject;
        MissingCornerPipe = GameObject.Find("MissingCornerPipe");
        FixedMissingCornerPipe = GameObject.Find("Music Player").transform.GetChild(1).gameObject;

        interacting = false;
        pipeFixed = false;
        showAction = true;
        hasPipe = false;
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interacting)
        {
            // check if player picked up pipe
            hasPipe = MissingCornerPipe.GetComponent<PickUpClue>().pickedUp;
            if (hasPipe)
            {
                FixedMissingCornerPipe.SetActive(true);
                pipeFixed = true;
            }
            else
            {
                Player.GetComponent<PlayerUI>().DisplayHintUI("Seems like a pipe is missing...");
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && !pipeFixed)
        {
            interacting = true;
            Player.GetComponent<PlayerUI>().DisplayInteractUI("Check contraption");
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Magistrate exited collider");
            interacting = false;
            showAction = false;
            Player.GetComponent<PlayerUI>().DisableHintUI();
            Player.GetComponent<PlayerUI>().DisableInteractUI();
        }
    }
}
