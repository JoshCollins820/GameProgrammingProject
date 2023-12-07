using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpClue : MonoBehaviour
{
    // Get any necessary GameObjects for Inventory
    GameObject Player;
    GameObject JournalUI;
    GameObject SwordPieces;

    // Bools to help with interacting with clue or lore
    public bool pickedUp;
    public bool showClue;
    public bool showInteract;
    public bool interacting;



    // Writing to go into journal about the clue or lore
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
        SwordPieces = GameObject.Find("SwordPieces");
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
        if (this.name == "Bag1")
        {
            SwordPieces.GetComponent<TrackPieces>().AddSwordPiece("Hilt");
            leftPageContent = "There is an ancient sword hilt inside!...\n\n" +
                "Also this handwritten note:\n" +
                "You undead idiot! I told you this sword could harm\n" +
                "you! Get that blacksmith you captured to break it\n" +
                "into pieces at the forge in the basement.\n" +
                "Then I want you to hide the pieces out of the way\n" +
                "so those other little imbeciles don't cut\n" +
                "themselves on it like you did. Maybe in Feron's" +
                "silly puzzles I don't care where. \n" +
                "We are trying to RAISE an army of the dead,\n" +
                "not collect body parts.\n" +
                "\t-Lord Master";
            rightPageContent = "I've got to find the rest of these\n" +
                "sword pieces then re-forge them if I\n" +
                "want to make it out of here alive!";
        }
        if (this.name == "Bag2")
        {
            SwordPieces.GetComponent<TrackPieces>().AddSwordPiece("Base");
            leftPageContent = "I have found another piece of the sword!\n" +
                "That makes " + SwordPieces.GetComponent<TrackPieces>().GetSwordPieces() +
                " pieces so far. This looks like it goes\n" +
                "just above the hilt...";
            rightPageContent = "";
        }
        if (this.name == "Bag3")
        {
            SwordPieces.GetComponent<TrackPieces>().AddSwordPiece("Middle");
            leftPageContent = "I have found another piece of the sword!\n" +
                "That makes " + SwordPieces.GetComponent<TrackPieces>().GetSwordPieces() +
                " pieces so far. This looks like it goes\n" +
                "in the middle. There is a channel as well.\n" +
                "I've seen these in magic items before,\n" +
                "this is meant to conduct magic current...";
            rightPageContent = "";
        }
        if (this.name == "Bag4")
        {
            SwordPieces.GetComponent<TrackPieces>().AddSwordPiece("Tip");
            leftPageContent = "I have found another piece of the sword!\n" +
                "That makes " + SwordPieces.GetComponent<TrackPieces>().GetSwordPieces() +
                " pieces so far. This looks\n" +
                "like it is the tip. I need to be very\n" +
                "careful, even after all these years it\n" +
                "is still incredibly sharp...";

            rightPageContent = "";
        }
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
        if (this.name == "FeronsMusicJournal")
        {
            leftPageContent = "Journal of one Feron Blythe\n" +
                "I found arcane stone runes in my most recent adventure\n" +
                "abroad. They are made of a material that had a most peculiar\n" +
                "ability to resonate with sound. So naturally I had to have them\n" +
                "for my latest contraption. After discovering that recordings of\n" +
                "sounds could be kept on a cylinder, that when spun with the\n" +
                "pressure of air, will emit their melodious tones from horns\n" +
                "I made special for the task, I knew I would be immortalized.";
            rightPageContent = "Bahahaha, the contraption is complete! By pressing the\n" +
                "runes, which I have laid in order to represent musical notation\n" +
                "rather than those horridly mundane letters ABCDEFG, tones will\n" +
                "sound gloriously from the resonating device I have set up within!\n" +
                "By playing the notes DGFEFD, a haunting tune of mine, one will\n" +
                "gain access to my sitting room... Ingenious no?";
        }

        if (this.name == "SittingRoomKey")
        {
            leftPageContent = "The key to Feron's Sitting Room...\n" +
                "Whatever a Sitting Room is...";
            rightPageContent = "";
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
