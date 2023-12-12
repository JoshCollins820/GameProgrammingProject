using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPieces : MonoBehaviour
{
    public GameObject Hilt;
    public GameObject Base;
    public GameObject Middle;
    public GameObject Tip;
    public GameObject JournalUI;
    public GameObject Magistrate;
    public GameObject Player;
    public GameObject PlayerMesh;
    public GameObject PlayerHair;
    [SerializeField] GameObject SwordCamera;
    [SerializeField] GameObject PlayerCamera;
    CharacterController PlayerController;


    public int swordPieces;
    public string swordPieceName;
    public string leftPageContent;
    public string rightPageContent;
    public bool complete;   // found all pieces
    public bool cutsceneRunning;
    
    // Start is called before the first frame update
    void Start()
    {
        Magistrate = GameObject.Find("Magistrate");
        Player = Magistrate.transform.GetChild(0).gameObject;
        PlayerHair = Player.transform.GetChild(1).gameObject;
        PlayerMesh = Player.transform.GetChild(0).gameObject;
        PlayerController = Player.GetComponent<CharacterController>();
        PlayerCamera = Magistrate.transform.Find("Normal VCamera").gameObject;
        JournalUI = GameObject.Find("JournalUI");
        Hilt = transform.GetChild(0).GetChild(1).gameObject;    //SwordPieces/Piece1/sword-hilt
        Base = transform.GetChild(1).GetChild(1).gameObject;
        Middle = transform.GetChild(2).GetChild(0).GetChild(1).gameObject; 
        Tip = transform.GetChild(3).GetChild(1).gameObject;
        //SwordCamera = transform.GetChild(0).GetChild(2).gameObject;
        swordPieces = 0;
        leftPageContent = "I now have all of the pieces of the sword!\n" +
            "Time to find the forge and complete the\n" +
            "weapon... I think it was in the basement...";
        rightPageContent = "";
        complete = false;
        cutsceneRunning = false;
        swordPieceName = "not assigned";
    }

    // Update is called once per frame
    void Update()
    {
        if (swordPieces == 4)
        {
            swordPieces++; // stop from repeating
            complete = true;    
            Invoke(nameof(UpdateJournal), 3f);
        }
        if (cutsceneRunning)
        {
            Debug.Log("Cutscene starting for " + swordPieceName);
            cutsceneRunning = false;
            swordCutscene(SwordCamera);
            StartCoroutine(DelayReturnToPlayer());
        }
    }

    public void AddSwordPiece(string piece)
    {
        Debug.Log("Picked up a piece");
        swordPieces++;
        swordPieceName = piece;
        if (swordPieceName == "Hilt")
        {
            Debug.Log("Picked up Hilt");
            SwordCamera = transform.GetChild(0).GetChild(2).gameObject;
        }
        if (swordPieceName == "Base")
        {
            Debug.Log("Picked up Base");
            SwordCamera = transform.GetChild(1).GetChild(2).gameObject;
        }
        if (swordPieceName == "Middle")
        {
            Debug.Log("Picked up Middle");
            SwordCamera = transform.GetChild(2).GetChild(1).gameObject;
        }
        if (swordPieceName == "Tip")
        {
            Debug.Log("Picked up Tip");
            SwordCamera = transform.GetChild(3).GetChild(2).gameObject;
        }
        cutsceneRunning = true;
    }
    

    private void swordCutscene(GameObject camera)
    {
        Debug.Log("Entered cutscene for " + camera.name);
        Player.GetComponent<PlayerUI>().DisableHintUI();
        Player.GetComponent<PlayerUI>().DisableInteractUI();
        Player.GetComponent<PlayerStats>().canMove = false;
        PlayerCamera.SetActive(false);
        Debug.Log("Player camera turned off");
        camera.SetActive(true);
        Debug.Log("Sword camera turned on");
        PlayerMesh.SetActive(false);
        PlayerHair.SetActive(false);
        Player.GetComponent<CharacterController>().enabled = false;
        PlayerController.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void returnPlayerControl(GameObject camera)
    {
        Debug.Log("Returning player control from " + camera.name);
        camera.SetActive(false);
        Debug.Log("Sword camera turned off");
        PlayerMesh.SetActive(true);
        PlayerHair.SetActive(true);
        Player.GetComponent<CharacterController>().enabled = true;
        PlayerController.enabled = true;
        PlayerCamera.SetActive(true);
        Debug.Log("Player camera turned on");
        Player.GetComponent<PlayerStats>().canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pickUpSwordPiece();
    }

    private void pickUpSwordPiece()
    {
        if (swordPieceName == "Hilt")
        {
            Hilt.SetActive(false);
        }
        if (swordPieceName == "Base")
        {
            Base.SetActive(false);
        }
        if (swordPieceName == "Middle")
        {
            Middle.SetActive(false);
        }
        if (swordPieceName == "Tip")
        {
            Tip.SetActive(false);
        }
        Player.GetComponent<PlayerUI>().DisplayHintUI("New Journal Entry (J)");
    }

    public string GetSwordPieces()
    {
        return swordPieces.ToString();
    }
    void UpdateJournal()
    {
        JournalUI.GetComponent<ReadJournal>().AddEntry(this.name, createEntryContent());
    }

    private List<string> createEntryContent()
    {
        List<string> content = new List<string>
        {
            leftPageContent,
            rightPageContent
        };
        return content;
    }

    IEnumerator DelayReturnToPlayer()
    {
        Debug.Log("Waiting for 2 seconds");
        yield return new WaitForSeconds(2f);
        returnPlayerControl(SwordCamera);
    }
}
