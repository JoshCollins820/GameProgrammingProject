using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpClue : MonoBehaviour
{
    // Get any necessary GameObjects for Inventory
    GameObject Player;
    GameObject JournalUI;
    GameObject SwordPieces;
    public GameObject SwordRebuilt;
    public GameObject SwordInHand;
    public GameObject Workshop;

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
        Workshop = GameObject.Find("Workshop");
        SwordRebuilt = Workshop.transform.GetChild(3).gameObject;
        SwordInHand = GameObject.Find("Sword_Stormbringer");
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

        // Sword Pieces
        //_____________________________________________________________________________
        if (this.name == "Sword_Stormbringer")
        {
            SwordInHand.SetActive(true);
            leftPageContent = "I have finally rebuilt the lost legendary sword: " +
                "Stormbringer... It is time to bring the storm...";
            rightPageContent = "I should Kill the Priest so I can go report " +
                "back to the High Judge what has happened here.\n\n" +
                "Cody... you will be avenged...";
        }
        if (this.name == "Bag1")
        {
            SwordPieces.GetComponent<TrackPieces>().AddSwordPiece("Hilt");
            leftPageContent = "There is an ancient sword hilt inside!...\n\n" +
                "Also this handwritten note:\n" +
                "You undead idiot! I told you this sword could harm " +
                "you! Get that blacksmith you captured to break it " +
                "into pieces at the forge in the basement. " +
                "Then I want you to hide the pieces out of the way " +
                "so those other little imbeciles don't cut " +
                "themselves on it like you did. Maybe in Feron's" +
                "silly puzzles I don't care where.  " +
                "We are trying to RAISE an army of the dead, " +
                "not collect body parts.\n " +
                "\t-Lord Master";
            rightPageContent = "I've got to find the rest of these\n" +
                "sword pieces then re-forge them if I " +
                "want to make it out of here alive!";
        }
        if (this.name == "Bag2")
        {
            SwordPieces.GetComponent<TrackPieces>().AddSwordPiece("Base");
            leftPageContent = "I have found another piece of the sword! " +
                "That makes " + SwordPieces.GetComponent<TrackPieces>().GetSwordPieces() +
                " pieces so far. This looks like it goes " +
                "just above the hilt...";
            rightPageContent = "";
        }
        if (this.name == "Bag3")
        {
            SwordPieces.GetComponent<TrackPieces>().AddSwordPiece("Middle");
            leftPageContent = "I have found another piece of the sword! " +
                "That makes " + SwordPieces.GetComponent<TrackPieces>().GetSwordPieces() +
                " pieces so far. This looks like it goes " +
                "in the middle. There is a channel as well. " +
                "I've seen these in magic items before, " +
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
        if (this.name == "Cover")
        {
            leftPageContent = "I thought I smelled burning...\n" +
                "Under this cover there is a recently used forge. " +
                "It looks like it is of Dwarven-make. I guess if " +
                "I need any improvised tools made I can come back " +
                "here... ";
            rightPageContent = "";
        }
        //_____________________________________________________________________________

        //Feron's Room Puzzle
        //_____________________________________________________________________________
        if (this.name == "FeronsJournal")
        {
            leftPageContent = "Reaching between the barricade I find a small " +
                "notebook... \n\n" +
                "Journal of Feron Blythe:\n" +
                "I found another use for those rune resonances! " +
                "You see, I can also make a certain magical attraction " +
                "much akin to magnetism between any object I enchant " +
                "and a given rune. \n\n" +
                "I have taken 4 of them and inlaid them into my desk. " +
                "As a reminder of where I have been in life I have chosen " +
                "the runes for WAR, INVENTION, ABUNDANCE, and DESTINY.";

            rightPageContent = "I have completed the enchantments on symbolic " +
                "memorabilia. For WAR I have chosen my miniature dwarven cannon, " +
                "to always remind me of those we lost in the ruins below Karnak. " +
                "For INVENTION, I selected a model of the recording device I " +
                "created: my greatest invention. For ABUNDANCE, of course, " +
                "Tymora's Goblet, in remembrance of both all the great treasures " +
                "I have discovered as well as the copious amounts of wine I " +
                "have imbibed... out of that very goblet no less. Finally, for DESTINY " +
                "my magical compass. This will be the key since it never leaves my side. " +
                "With this compass I have always been directed towards my destiny, " +
                "unerringly and unapologetically. ";
        }
        if (this.name == "Cannon")
        {
            this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
            leftPageContent = "It is a miniature cannon. It feels warm as if " +
                "having been fired earlier. There is no way it tore apart this " +
                "wall... could it have?";

            rightPageContent = "";
        }
        if (this.name == "Compass")
        {
            leftPageContent = "This is a well-used compass. There seems " +
                "to be a magical aura to it. The compass point almost " +
                "looks holographic. Is it pointing to the desk?";

            rightPageContent = "";
        }
        if (this.name == "Goblet")
        {
            leftPageContent = "This expensive Goblet looks out of place " +
                "here. Holding it close to my ear, it sounds like " +
                "there is a faint humming coming from this.";

            rightPageContent = "";
        }
        if (this.name == "Mini_Recorder")
        {
            this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            leftPageContent = "This must be the device that Feron Blythe " +
                "was famous for inventing. It's just a miniature of the real " +
                "one in the study. Still, it almost feels like the air around " +
                "it is vibrating.";

            rightPageContent = "";
        }
        if (this.name == "Combination")
        {
            leftPageContent = "There is a note with a series of numbers " +
                "on it... I think this is some kind of combination for " +
                "a lockbox of some sort.\n\n" +
                "Wasn't there a safe in the basement?";
            rightPageContent = "";
        }
        //_____________________________________________________________________________

        //Music Room Puzzle
        //_____________________________________________________________________________
        if (this.name == "FeronsMusicJournal")
        {
            leftPageContent = "Journal of one Feron Blythe:\n" +
                "I found arcane stone runes in my most recent\n" +
                "adventure abroad. They are made of a material\n" +
                "that had a most peculiar ability to resonate\n" +
                "with sound. So naturally I had to have them\n" +
                "for my latest contraption. After discovering\n" +
                "that recordings of sounds could be kept on a\n" +
                "disk that when spun with the pressure of air,\n" +
                "will emit their melodious tones from horns I\n" +
                "made special for the task, I knew I would be\n" +
                "immortalized for something... this is it.";
            rightPageContent = "Bahahaha, the contraption is complete! By\n" +
                "pressing the runes, which I have laid\n" +
                "in order to represent musical notation,\n" +
                "rather than those horridly mundane letters\n" +
                "ABCDEFG, tones will sound gloriously from the\n" +
                "resonating device I have set up within! By\n" +
                "playing the notes DGFEFD, a haunting tune of\n" +
                "mine, one will gain access to my sitting room\n" +
                "... Ingenious no?";
        }
        if (this.name == "SittingRoomKey")
        {
            leftPageContent = "The key to Feron's Sitting Room...\n" +
                "Whatever a Sitting Room is...";
            rightPageContent = "";
        }
        if (this.name == "MissingCornerPipe")
        {
            leftPageContent = "It is a corner pipe. Maybe I can use this for\n" +
                "something?";
            rightPageContent = "";
        }
        //_____________________________________________________________________________

        //Stone Puzzle
        //_____________________________________________________________________________
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
            leftPageContent = "TenthMonth, 5th day: I have been charged by the High Judge to " +
                "investigate the spate of disappearances recently outside of Blune. " +
                "I suspect I might find something over at the old Blythe Manor.\n\n" +
                "TenthMonth, 14th day: I have made it to Blune, the local constabulary " +
                "has confirmed my hunch... those who are missing were last seen heading " +
                "in the direction of Blythe Manor in a daze accompanied by a large " +
                "stranger... Time to investigate the old home of the famous bard: " +
                "Feron Blythe.";
            rightPageContent = "TenthMonth...? I've lost track of the days. I escaped once but it took " +
                "me too long to figure out Blythe's damn light puzzle. I understand " +
                "now, but I don't think the priest will make the same mistake with " +
                "the key. The High Judge surely sent another Magistrate... if you " +
                "are reading this I hope you fare better than I. The pattern is: " +
                "Red, Yellow, Green, Cyan, Blue, Purple... " +
                "the order of the visible light spectrum. Good Luck and...\n" +
                "BEWARE THE PRIEST!!!!";
        }
        //_____________________________________________________________________________

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
