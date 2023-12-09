using System.Collections;
using UnityEngine;

public class FeronsRoomPuzzle : MonoBehaviour
{
    [SerializeField] GameObject War_Rune;
    [SerializeField] GameObject Invention_Rune;
    [SerializeField] GameObject Abundance_Rune;
    [SerializeField] GameObject Destiny_Rune;


    [SerializeField] GameObject FRVCamera;
    [SerializeField] GameObject DrawerVCamera;
    [SerializeField] GameObject PlayerCamera;
    CharacterController PlayerController;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Magistrate;
    [SerializeField] GameObject PlayerMesh;
    [SerializeField] GameObject PlayerHair;
    [SerializeField] GameObject Cabinet;
    [SerializeField] GameObject Drawer1;
    [SerializeField] GameObject Drawer2;
    [SerializeField] GameObject FRCanvas;
    [SerializeField] GameObject Bag3;
    [SerializeField] GameObject Combination;
    [SerializeField] GameObject FeronsJournal;

    [SerializeField] bool interacting;
    [SerializeField] bool puzzleSolved;
    [SerializeField] bool foundCannon;
    [SerializeField] bool foundMiniRec;
    [SerializeField] bool foundGoblet;
    [SerializeField] bool foundCompass;
    [SerializeField] bool opening;
    [SerializeField] bool opened;
    [SerializeField] string objPlaced;
    [SerializeField] string puzzleSolution;
    [SerializeField] int objCollected;
    [SerializeField] int pickedObj;
    [SerializeField] int numPlaced;
    [SerializeField] Vector3 closedPosition1;
    [SerializeField] Vector3 closedPosition2;
    [SerializeField] Vector3 openedPosition1;
    [SerializeField] Vector3 openedPosition2;
    [SerializeField] float lerpDuration;

    // Start is called before the first frame update
    void Start()
    {
        Cabinet = transform.GetChild(0).gameObject;
        //drawer 1 is a part of SwordPieces so that sword-middle and Bag3 will Lerp with it
        Drawer1 = GameObject.Find("SwordPieces").transform.GetChild(2).GetChild(0).gameObject;
        Bag3 = Drawer1.transform.GetChild(0).gameObject;
        Drawer2 = Cabinet.transform.GetChild(0).gameObject;
        Combination = Drawer2.transform.GetChild(0).gameObject;
        War_Rune = transform.GetChild(1).gameObject;
        Invention_Rune = transform.GetChild(2).gameObject;
        Abundance_Rune = transform.GetChild(3).gameObject;
        Destiny_Rune = transform.GetChild(4).gameObject;
        FRVCamera = transform.GetChild(5).gameObject;
        FRCanvas = transform.GetChild(6).gameObject;
        DrawerVCamera = transform.GetChild(7).gameObject;
        FeronsJournal = transform.GetChild(8).gameObject;

        Magistrate = GameObject.Find("Magistrate");
        Player = Magistrate.transform.GetChild(0).gameObject;
        PlayerHair = Player.transform.GetChild(1).gameObject;
        PlayerMesh = Player.transform.GetChild(0).gameObject;
        PlayerController = Player.GetComponent<CharacterController>();
        PlayerCamera = Magistrate.transform.gameObject.transform.Find("Normal VCamera").gameObject;

        interacting = false;
        puzzleSolved = false;
        foundCannon = false;
        foundMiniRec = false;
        foundGoblet = false;
        foundCompass = false;
        opening = false;
        opened = false;
        objPlaced = "";
        puzzleSolution = "ca_mr_go_co_";
        pickedObj = 0;
        numPlaced = 1;
        objCollected = 0;
        
        closedPosition1 = Drawer1.transform.position;
        closedPosition2 = Drawer2.transform.position;
        openedPosition1 = new Vector3(closedPosition1.x, closedPosition1.y, closedPosition1.z - 0.5f);
        openedPosition2 = new Vector3(closedPosition2.x, closedPosition2.y, closedPosition2.z - 0.5f);
        lerpDuration = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interacting)  // player is starting the puzzle
            {
                if (!puzzleSolved)
                {
                    CheckObjCollected();
                    if (objCollected == 4)
                    {
                        Player.GetComponent<PlayerUI>().DisableHintUI();
                        Player.GetComponent<PlayerUI>().DisableInteractUI();
                        Player.GetComponent<PlayerStats>().canMove = false;
                        PlayerCamera.SetActive(false);
                        FRVCamera.SetActive(true);
                        PlayerMesh.SetActive(false);
                        PlayerHair.SetActive(false);
                        Player.GetComponent<CharacterController>().enabled = false;
                        PlayerController.enabled = false;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;

                        //display FR Canvas if Ferons Journal has been found
                        if (FeronsJournal.GetComponent<PickUpClue>().pickedUp && !FRCanvas.activeSelf)
                        {
                            FRCanvas.SetActive(true);
                        }
                    }
                    else
                    {
                        Player.GetComponent<PlayerUI>().DisableInteractUI();
                        Player.GetComponent<PlayerUI>().DisplayHintUI("Seems like 4 objects\n" +
                            "belong here");
                    }
                }
            }
        }
        if (numPlaced == 5) //every object has been placed, check positions
        {
            numPlaced = 0; //prevent another call
            Invoke(nameof(CheckSolution), 1f);
        }
        if (opening)
        {
            StartCoroutine(OpenDrawers());
        }
        if (opened)
        {
            opened = false;
            DrawerVCamera.SetActive(false);
            returnPlayerControl();
        }
    }

    //if obj has been picked up and isn't already on shelf, set active
    private void CheckObjCollected()
    {
        if (FeronsJournal.GetComponent<PickUpClue>().pickedUp && 
            !FRCanvas.activeSelf)
        {
            FRCanvas.SetActive(true);
        }
        if (War_Rune.transform.GetChild(5).GetComponent<PickUpClue>().pickedUp &&
             !foundCannon)
        {
            objCollected++;
            War_Rune.transform.GetChild(0).gameObject.SetActive(true);
            foundCannon = true;
        }
        if (Invention_Rune.transform.GetChild(5).GetComponent<PickUpClue>().pickedUp &&
             !foundMiniRec)
        {
            objCollected++;
            Invention_Rune.transform.GetChild(0).gameObject.SetActive(true);
            foundMiniRec = true;
        }
        if (Abundance_Rune.transform.GetChild(5).GetComponent<PickUpClue>().pickedUp &&
             !foundGoblet)
        {
            objCollected++;
            Abundance_Rune.transform.GetChild(0).gameObject.SetActive(true);
            foundGoblet = true;
        }
        if (Destiny_Rune.transform.GetChild(5).GetComponent<PickUpClue>().pickedUp &&
            !foundCompass)
        {
            objCollected++;
            Destiny_Rune.transform.GetChild(0).gameObject.SetActive(true);
            foundCompass = true;
        }
    }

    private void returnPlayerControl()
    {
        FRVCamera.SetActive(false);
        PlayerMesh.SetActive(true);
        PlayerHair.SetActive(true);
        Player.GetComponent<CharacterController>().enabled = true;
        PlayerController.enabled = true;
        PlayerCamera.SetActive(true);
        Player.GetComponent<PlayerStats>().canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
           
        if (!puzzleSolved)
        {
            Player.GetComponent<PlayerUI>().DisplayInteractUI("Place memorabilia");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            interacting = true;
            if (!puzzleSolved)
            {
                Player.GetComponent<PlayerUI>().DisplayInteractUI("Place memorabilia");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Player.GetComponent<PlayerUI>().DisableInteractUI();
            Player.GetComponent<PlayerUI>().DisableHintUI();
            interacting = false;
        }
    }

    // Called by RaycastPuzzle on shelf objects
    // Once object on shelf is clicked, store as pickedObj and deactivate then place on rune
    public void PickObject(string name)
    {
        if (name == "Cannon_Shelf")
        {
            War_Rune.transform.GetChild(0).gameObject.SetActive(false);
            pickedObj = 1;
        }
        else if (name == "Mini_Recorder_Shelf")
        {
            Invention_Rune.transform.GetChild(0).gameObject.SetActive(false);
            pickedObj = 2;
        }
        else if (name == "Goblet_Shelf")
        {
            Abundance_Rune.transform.GetChild(0).gameObject.SetActive(false);
            pickedObj = 3;
        }
        else if (name == "Compass_Shelf")
        {
            Destiny_Rune.transform.GetChild(0).gameObject.SetActive(false);
            pickedObj = 4;
        }
        PlaceObject();
    }

    // Set pickedObj on current unplaced Rune, then reset pickedObj
    private void PlaceObject()
    {
        if (pickedObj != 0)
            transform.GetChild(numPlaced).GetChild(pickedObj).gameObject.SetActive(true);
        // set solution
        if (pickedObj == 1)
        {
            objPlaced += "ca_";
        }
        else if(pickedObj == 2)
        {
            objPlaced += "mr_";
        }
        else if(pickedObj == 3) 
        {
            objPlaced += "go_";
        }
        else if(pickedObj == 4) 
        { 
            objPlaced += "co_"; 
        }
        numPlaced++;
        pickedObj = 0;  // prevent same obj getting placed onto next Rune
    }

    private void CheckSolution()
    {
        if (objPlaced == puzzleSolution)
        {
            //puzzle solved open drawers
            puzzleSolved = true;
            Bag3.SetActive(true);
            Combination.SetActive(true);
            opening = true; 
            FRVCamera.SetActive(false );
            DrawerVCamera.SetActive(true );
        }
        else
        {
            Player.GetComponent<PlayerUI>().DisplayHintUI("Puzzle Failed");
            returnPlayerControl();
            ResetPuzzle();
        }
    }

    // turn off all obj placed on Runes 
    private void ResetPuzzle()
    {
        for (int i = 1; i < 5; i++)
        {
            War_Rune.transform.GetChild(i).gameObject.SetActive(false);
            Invention_Rune.transform.GetChild(i).gameObject.SetActive(false);
            Abundance_Rune.transform.GetChild(i).gameObject.SetActive(false);
            Destiny_Rune.transform.GetChild(i).gameObject.SetActive(false);
        }
        pickedObj = 0;
        numPlaced = 1;
        objPlaced = "";
        FRCanvas.SetActive(false);
        War_Rune.transform.GetChild(0).gameObject.SetActive(true);
        Invention_Rune.transform.GetChild(0).gameObject.SetActive(true);
        Abundance_Rune.transform.GetChild(0).gameObject.SetActive(true);
        Destiny_Rune.transform.GetChild(0).gameObject.SetActive(true);
    }
    IEnumerator OpenDrawers()
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            Drawer1.transform.position = Vector3.Lerp(closedPosition1, openedPosition1, timeElapsed / lerpDuration);
            Drawer2.transform.position = Vector3.Lerp(closedPosition2, openedPosition2, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        opening = false;
        opened = true;
    }
}
