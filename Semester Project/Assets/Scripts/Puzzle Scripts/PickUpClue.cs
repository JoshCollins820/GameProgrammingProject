using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpClue : MonoBehaviour
{
    // Get any necessary GameObjects for Inventory
    GameObject Player;

    public bool pickedUp;
    public bool showClue;
    public bool showInteract;
    public bool interacting;
    string clueText;

    // Start is called before the first frame update
    void Start()
    {
        clueText = "No Clue Text Assigned to this Object";
        pickedUp = false;
        showClue = true;
        showInteract = true;
        interacting = false;
        Player = GameObject.Find("Magistrate").transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interacting)
        {
            Player.GetComponent<PlayerUI>().DisableInteractUI();
            pickedUp = true;
            // TODO: Put into inventory
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;

            
        }
        if (pickedUp && showClue)
        {
            // show message on hint UI
            Player.GetComponent<PlayerUI>().DisplayHintUI(getClueText());
            showClue = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && !pickedUp && showInteract)
        {
            interacting = true;
            Player.GetComponent<PlayerUI>().DisplayInteractUI("Pick Up");
            showInteract = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            interacting = false;
            Player.GetComponent<PlayerUI>().DisableHintUI();
            Player.GetComponent<PlayerUI>().DisableInteractUI();
            showInteract = true;
        }
    }

    private string getClueText()
    {
        // check for correct clue to show
        if (this.name == "SecretDoorBook")
        {
            clueText = "As light refracts from Infrared to Ultraviolet,\n" +
                "so too must stone pathways be lit.";
        }
        if (this.name == "DungeonDoorKey")
        {
            clueText = "It's a key, it must open something...";
        }
        if (this.name == "MagistrateBook")
        {
            clueText = "ThirdMonth, 5th day: I have been charged by the High Judge to\n" +
                "investigate the spate of disappearances recently outside of Blune. \n" +
                "I suspect I might find something over at the old Blythe Manor.\n\n" +
                "ThirdMonth... I've lost track of the days. I escaped once but it took\n" +
                "me too long to figure out Blythe's damn light puzzle. I understand\n" +
                "now, but I don't think the priest will make the same mistake with\n" +
                "the key. The High Judge surely sent another Magistrate... if you\n" +
                "are reading this I hope you fare better than I. The pattern is:\n" +
                "Red, Yellow, Green, Cyan, Blue, Purple...\n" +
                "the order of the visible light spectrum. Good Luck and..." +
                "BEWARE THE PRIEST!!!!";

        }
        showClue = false;   // stop updating
        return clueText;
    }

    //private void OnMouseDown()
    //{
    //    if (!pickedUp)
    //    {
    //        // pick up (make disappear and put into inventory)
    //        pickedUp = true;
    //        this.gameObject.SetActive(false);

    //        // TODO: Put into inventory
    //    }
    //}
}
