using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] GameObject PuzzleCamera;
    [SerializeField] GameObject PlayerCamera;
    [SerializeField] GameObject ChestCamera;
    CharacterController PlayerController;

    // booleans to help control puzzle states
    public bool puzzleStarted;
    public bool puzzleSolved;
    public bool interacting;

    // keep track of and compare to solution string DGFEFD
    string puzzleOrder;
    string puzzleSolution;

    // check how many notes player has tried
    int notesAttempted;

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

        puzzleStarted = false;
        puzzleSolved = false;
        interacting = false;

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
        if (Rune_A.GetComponent<RuneOnClick>().activated)
        {
            puzzleOrder += "A";
            notesAttempted++;
            Rune_A.GetComponent<RuneOnClick>().activated = false;
        }
        if (Rune_B.GetComponent<RuneOnClick>().activated)
        {
            puzzleOrder += "B";
            notesAttempted++;
            Rune_B.GetComponent<RuneOnClick>().activated = false;
        }
        if (Rune_C.GetComponent<RuneOnClick>().activated)
        {
            puzzleOrder += "C";
            notesAttempted++;
            Rune_C.GetComponent<RuneOnClick>().activated = false;
        }
        if (Rune_D.GetComponent<RuneOnClick>().activated)
        {
            puzzleOrder += "D";
            notesAttempted++;
            Rune_D.GetComponent<RuneOnClick>().activated = false;
        }
        if (Rune_E.GetComponent<RuneOnClick>().activated)
        {
            puzzleOrder += "E";
            notesAttempted++;
            Rune_E.GetComponent<RuneOnClick>().activated = false;
        }
        if (Rune_F.GetComponent<RuneOnClick>().activated)
        {
            puzzleOrder += "F";
            notesAttempted++;
            Rune_F.GetComponent<RuneOnClick>().activated = false;
        }
        if (Rune_G.GetComponent<RuneOnClick>().activated)
        {
            puzzleOrder += "G";
            notesAttempted++;
            Rune_G.GetComponent<RuneOnClick>().activated = false;
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
            PuzzleCamera.SetActive(false);
            ChestCamera.SetActive(true);
            interacting = false;
            notesAttempted = 0;
        }
        else
        {
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
        }
    }
}
