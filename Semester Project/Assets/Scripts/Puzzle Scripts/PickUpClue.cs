using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpClue : MonoBehaviour
{
    // Get any necessary GameObjects for Inventory
    GameObject Player;
    GameObject JournalUI;
    GameObject Journal;

    public bool pickedUp;
    public bool showClue;
    public bool showInteract;
    public bool interacting;
    string leftPageContent;
    string rightPageContent;

    // Start is called before the first frame update
    void Start()
    {
        leftPageContent = "No Content Text Assigned to this Clue Object";
        rightPageContent = this.name;
        pickedUp = false;
        showClue = true;
        showInteract = true;
        interacting = false;
        Player = GameObject.Find("Magistrate").transform.GetChild(0).gameObject;
        JournalUI = GameObject.Find("JournalUI");
        Journal = JournalUI.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // pick up clue with "e"
        if (Input.GetKeyDown(KeyCode.E) && interacting)
        {
            Player.GetComponent<PlayerUI>().DisableInteractUI();
            pickedUp = true;
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            
        }
        // open journal with "j"
        if (Input.GetKeyDown(KeyCode.J) && interacting && pickedUp)
        {
            // player presses "j" New Journal Entry is displayed to open journal
            Player.GetComponent<PlayerUI>().DisableHintUI();
        }
        // player picked up clue, add to journal
        if (pickedUp && showClue)
        {
            // show message on hint UI
            Player.GetComponent<PlayerUI>().DisplayHintUI("New Journal Entry (J)");
            // add clue to journal
            setJournalContent();
            JournalUI.GetComponent<ReadJournal>().AddEntry(this.name, createEntryContent());
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

    //set left and right page content to prepare to add to player journal
    private void setJournalContent()
    {
        // check for correct clue to show
        if (this.name == "MissingCornerPipe")
        {
            leftPageContent = "It is a corner pipe. Maybe I can use this for\n" +
                "something?";
            rightPageContent = "";
        }
        if (this.name == "SecretDoorBook")
        {
            leftPageContent = "As light refracts from Infrared to Ultraviolet,\n" +
                "so too must stone pathways be lit.";
            rightPageContent = "";
        }
        if (this.name == "DungeonDoorKey")
        {
            leftPageContent = "It's a key, it must open something...";
            rightPageContent = "";
        }
        if (this.name == "MagistrateBook")
        {
            leftPageContent = "TenthMonth, 5th day: I have been charged by the High Judge to\n" +
                "investigate the spate of disappearances recently outside of Blune. \n" +
                "I suspect I might find something over at the old Blythe Manor.\n\n" +
                "TenthMonth, 14th day: I have made it to Blune, the local constabulary\n" +
                "has confirmed my hunch... those who are missing were last seen heading\n" +
                "in the direction of Blythe Manor in a daze accompanied by a large\n" +
                "stranger... Time to investigate the old home of the famous bard:\n" +
                "Feron Blythe.";
            rightPageContent = "TenthMonth...? I've lost track of the days. I escaped once but it took\n" +
                "me too long to figure out Blythe's damn light puzzle. I understand\n" +
                "now, but I don't think the priest will make the same mistake with\n" +
                "the key. The High Judge surely sent another Magistrate... if you\n" +
                "are reading this I hope you fare better than I. The pattern is:\n" +
                "Red, Yellow, Green, Cyan, Blue, Purple...\n" +
                "the order of the visible light spectrum. Good Luck and..." +
                "BEWARE THE PRIEST!!!!";

        }
        showClue = false;   // stop updating
    }

    // creates Journal entry content of form List[0] = left page content, List[1] = right page content
    private List<string> createEntryContent()
    {
        List<string> content = new List<string>
        {
            leftPageContent,
            rightPageContent
        };
        return content;
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
