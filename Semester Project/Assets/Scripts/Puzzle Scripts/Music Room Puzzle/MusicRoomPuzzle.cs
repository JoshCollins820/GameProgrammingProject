using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicRoomPuzzle : MonoBehaviour
{
    // Player Objects
    public GameObject Magistrate;
    public GameObject Player;
    public GameObject PlayerMesh;
    public GameObject PlayerHair;

    // Puzzle Rune Objects
    public GameObject Rune_A;
    public GameObject Rune_B;
    public GameObject Rune_C;
    public GameObject Rune_D;
    public GameObject Rune_E;
    public GameObject Rune_F;
    public GameObject Rune_G;

    // To open Puzzle Chest
    public GameObject ChestHinge;

    // UI Notes Pressed Hint
    public GameObject NotesPressed;

    [SerializeField] GameObject PuzzleCamera;
    [SerializeField] GameObject PlayerCamera;
    [SerializeField] GameObject ChestCamera;
    CharacterController PlayerController;

    // booleans to help control puzzle states
    public bool puzzleStarted;
    public bool puzzleSolved;
    public bool interacting;
    public bool aActive;
    public bool bActive;
    public bool cActive;
    public bool dActive;
    public bool eActive;
    public bool fActive;
    public bool gActive;

    // keep track of and compare to solution string DGFEFD
    public string puzzleOrder;
    string puzzleSolution;

    // check how many notes player has tried
    public int notesAttempted;

    // Start is called before the first frame update
    void Start()
    {
        Magistrate = GameObject.Find("Magistrate");
        Player = Magistrate.transform.GetChild(0).gameObject;
        PlayerHair = Player.transform.GetChild(1).gameObject;
        PlayerMesh = Player.transform.GetChild(0).gameObject;
        PlayerController = Player.GetComponent<CharacterController>();
        PuzzleCamera = this.transform.gameObject.transform.Find("Music Room Puzzle VCamera").gameObject;
        PlayerCamera = Magistrate.transform.gameObject.transform.Find("Normal VCamera").gameObject;
        ChestCamera = this.transform.gameObject.transform.Find("Chest VCamera").gameObject;

        Rune_A = GameObject.Find("Rune_A");
        Rune_B = GameObject.Find("Rune_B");
        Rune_C = GameObject.Find("Rune_C");
        Rune_D = GameObject.Find("Rune_D");
        Rune_E = GameObject.Find("Rune_E");
        Rune_F = GameObject.Find("Rune_F");
        Rune_G = GameObject.Find("Rune_G");

        ChestHinge = GameObject.Find("MusicRoomChest").transform.GetChild(2).gameObject;

        NotesPressed = GameObject.Find("NotesPressed");
        NotesPressed.GetComponent<TextMeshProUGUI>().text = "";

        puzzleStarted = false;
        puzzleSolved = false;
        interacting = false;
        aActive = false;
        bActive = false;
        cActive = false;
        dActive = false;
        eActive = false;
        fActive = false;
        gActive = false;

        puzzleOrder = "";
        puzzleSolution = "DGFEFD";

        notesAttempted = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interacting && !puzzleSolved)  // player is starting the puzzle
            {
                Player.GetComponent<PlayerUI>().DisableHintUI();
                Player.GetComponent<PlayerUI>().DisableInteractUI();
                Player.GetComponent<PlayerStats>().canMove = false;
                PlayerCamera.SetActive(false);
                PuzzleCamera.SetActive(true);
                PlayerMesh.SetActive(false);
                PlayerHair.SetActive(false);
                Player.GetComponent<CharacterController>().enabled = false;
                PlayerController.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                puzzleStarted = true;
            }
            //if (puzzleStarted) // player is stopping the puzzle
            //{
            //    returnPlayerControl();             
            //}   
        }
        // handle rune pressed
        if (Rune_A.GetComponent<RuneOnClick>().activated && !aActive)
        {
            aActive = true;
            Invoke(nameof(ActivateRuneA), 0.1f);
        }
        if (Rune_B.GetComponent<RuneOnClick>().activated && !bActive)
        {
            bActive = true;
            Invoke(nameof(ActivateRuneB), 0.1f);
        }
        if (Rune_C.GetComponent<RuneOnClick>().activated && !cActive)
        {
            cActive = true;
            Invoke(nameof(ActivateRuneC), 0.1f);
        }
        if (Rune_D.GetComponent<RuneOnClick>().activated && !dActive)
        {
            dActive = true;
            Invoke(nameof(ActivateRuneD), 0.1f);
        }
        if (Rune_E.GetComponent<RuneOnClick>().activated && !eActive)
        {
            eActive = true;
            Invoke(nameof(ActivateRuneE), 0.1f);
        }
        if (Rune_F.GetComponent<RuneOnClick>().activated && !fActive)
        {
            fActive = true;
            Invoke(nameof(ActivateRuneF), 0.1f);
        }
        if (Rune_G.GetComponent<RuneOnClick>().activated && !gActive)
        {
            gActive = true;
            Invoke(nameof(ActivateRuneG), 0.1f);
        }

        // update notes pressed
        if (interacting && !puzzleSolved)
        {
            NotesPressed.GetComponent<TextMeshProUGUI>().text = puzzleOrder;
        }

        // once 6 notes have been played check if puzzle is solved
        if (notesAttempted == 6)
        {
            checkSolution();
        }

    }

    private void returnPlayerControl()
    {
        PuzzleCamera.SetActive(false);
        PlayerMesh.SetActive(true);
        PlayerHair.SetActive(true);
        Player.GetComponent<CharacterController>().enabled = true;
        PlayerController.enabled = true;
        PlayerCamera.SetActive(true);
        Player.GetComponent<PlayerStats>().canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        puzzleStarted = false;
        notesAttempted = 0;
        puzzleOrder = "";
        if (!puzzleSolved)
        {
            Player.GetComponent<PlayerUI>().DisplayInteractUI("Press Runes");
        }
    }

    private void checkSolution()
    {
        if (puzzleOrder == puzzleSolution)
        {
            Debug.Log("Puzzle Solved");
            // move camera over to chest
            PuzzleCamera.SetActive(false);
            ChestCamera.SetActive(true);
            interacting = false;
            resetPuzzle();
            // open chest
            ChestHinge.GetComponent<OpenChest>().Open();
        }
        else
        {
            Debug.Log("Puzzle Failed");
            resetPuzzle();
            Player.GetComponent<PlayerUI>().DisplayHintUI("Failed Puzzle");
            returnPlayerControl();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && !puzzleSolved)
        {

            Player.GetComponent<PlayerUI>().DisplayInteractUI("Press Runes");
            Debug.Log("Puzzle zone entered.");
            interacting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            interacting = false;
            Player.GetComponent<PlayerUI>().DisableInteractUI();
            Player.GetComponent<PlayerUI>().DisableHintUI();
            resetPuzzle();
        }
    }

    private void resetPuzzle()
    {
        aActive = false;
        bActive = false;
        cActive = false;
        dActive = false;
        eActive = false;
        fActive = false;
        gActive = false;
        puzzleOrder = "";
        notesAttempted = 0;
    }

    private void ActivateRuneA()
    {
        Rune_A.GetComponent<RuneOnClick>().ActivateRune();
        Invoke(nameof(DeactivateRuneA), 1f);
    }

    private void ActivateRuneB()
    {
        Rune_B.GetComponent<RuneOnClick>().ActivateRune();
        Invoke(nameof(DeactivateRuneB), 1f);
    }

    private void ActivateRuneC()
    { 
        Rune_C.GetComponent<RuneOnClick>().ActivateRune();
        Invoke(nameof(DeactivateRuneC), 1f);
    }

    private void ActivateRuneD()
    {  
        Rune_D.GetComponent<RuneOnClick>().ActivateRune();
        Invoke(nameof(DeactivateRuneD), 1f);
    }

    private void ActivateRuneE()
    {
        Rune_E.GetComponent<RuneOnClick>().ActivateRune();
        Invoke(nameof(DeactivateRuneE), 1f);
    }
    private void ActivateRuneF()
    {       
        Rune_F.GetComponent<RuneOnClick>().ActivateRune();
        Invoke(nameof(DeactivateRuneF), 1f);
    }

    private void ActivateRuneG()
    {
        Rune_G.GetComponent<RuneOnClick>().ActivateRune();
        Invoke(nameof(DeactivateRuneG), 1f);
    }

    private void DeactivateRuneA()
    {
        puzzleOrder += "A";
        notesAttempted++;
        aActive = false;
        Rune_A.GetComponent<RuneOnClick>().DeactivateRune();
    }

    private void DeactivateRuneB()
    {
        puzzleOrder += "B";
        notesAttempted++;
        bActive = false;
        Rune_B.GetComponent<RuneOnClick>().DeactivateRune();
    }

    private void DeactivateRuneC()
    {
        puzzleOrder += "C";
        notesAttempted++;
        cActive = false;
        Rune_C.GetComponent<RuneOnClick>().DeactivateRune();
    }

    private void DeactivateRuneD()
    {
        puzzleOrder += "D";
        notesAttempted++;
        dActive = false;
        Rune_D.GetComponent<RuneOnClick>().DeactivateRune();
    }

    private void DeactivateRuneE()
    {
        puzzleOrder += "E";
        notesAttempted++;
        eActive = false;
        Rune_E.GetComponent<RuneOnClick>().DeactivateRune();
    }
    private void DeactivateRuneF()
    {
        puzzleOrder += "F";
        notesAttempted++;
        fActive = false;
        Rune_F.GetComponent<RuneOnClick>().DeactivateRune();
    }

    private void DeactivateRuneG()
    {
        puzzleOrder += "G";
        notesAttempted++;
        gActive = false;
        Rune_G.GetComponent<RuneOnClick>().DeactivateRune();
    }
}
