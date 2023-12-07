using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class BasementPuzzle : MonoBehaviour
{
    public UnityEvent SpawnEnemies;

    GameObject Magistrate;
    public GameObject Player;
    GameObject PlayerMesh;
    GameObject PlayerHair;
    GameObject StoneRed;
    GameObject StonePurple;
    GameObject StoneBlue;
    GameObject StoneCyan;
    GameObject StoneGreen;
    GameObject StoneYellow;
    [SerializeField] GameObject PuzzleCamera;
    [SerializeField] GameObject PlayerCamera;
    [SerializeField] GameObject SecretDoorCamera;
    CharacterController PlayerController;
    GameObject SecretDoor;
    public AudioSource doorOpen;
    public GameObject Priest;
    public GameObject breathing;

    public bool puzzleStarted;
    public bool puzzleFailed;
    public bool puzzleSolved;
    public bool interacting;
    public bool redActive;
    public bool purpleActive;
    public bool blueActive;
    public bool cyanActive;
    public bool greenActive;
    public bool yellowActive;
    public int activeStones;
    public string orderActive;

    Vector3 openPos;
    Vector3 closedPos;
    float lerpDuration;
    bool opening;
    bool changeCamera = false;
    public int enter;


    // Start is called before the first frame update
    void Start()
    {
        Magistrate = GameObject.Find("Magistrate");
        Player = Magistrate.transform.GetChild(0).gameObject;
        PlayerHair = Player.transform.GetChild(1).gameObject;
        PlayerMesh = Player.transform.GetChild(0).gameObject;
        PlayerController = Player.GetComponent<CharacterController>();
        Priest = GameObject.Find("Priest");
        StoneRed = GameObject.Find("Stone_Red");
        StonePurple = GameObject.Find("Stone_Purple");
        StoneBlue = GameObject.Find("Stone_Blue");
        StoneCyan = GameObject.Find("Stone_Cyan");
        StoneGreen = GameObject.Find("Stone_Green");
        StoneYellow = GameObject.Find("Stone_Yellow");
        PuzzleCamera = this.transform.Find("Basement Puzzle VCamera").gameObject;
        PlayerCamera = Magistrate.transform.Find("Normal VCamera").gameObject;
        SecretDoorCamera = this.transform.Find("Basement Secret Door VCamera").gameObject;
        SecretDoor = GameObject.Find("SecretDoor");
        doorOpen = SecretDoor.transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        //breathing = Priest.transform.Find("Breathing").gameObject;

        closedPos = SecretDoor.transform.position;
        openPos = new Vector3(SecretDoor.transform.position.x, SecretDoor.transform.position.y, -29.248f);
        lerpDuration = 4f;
        opening = false;

        puzzleStarted = false;
        puzzleFailed = false;
        puzzleSolved = false;
        interacting = false;
        redActive = false;
        purpleActive = false;
        blueActive = false;
        cyanActive = false;
        greenActive = false;
        yellowActive = false;
        activeStones = 0;
        orderActive = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interacting)
            {
                //Debug.Log("Pressed E interacting start puzzle");

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
            //    Debug.Log("Puzzle Started and Pressed E, leave puzzle");
            //    resetPuzzle();
            //    puzzleStarted = false;
            //    PuzzleCamera.SetActive(false);
            //    PlayerMesh.SetActive(true);
            //    PlayerHair.SetActive(true);
            //    Player.GetComponent<CharacterController>().enabled = true;
            //    PlayerController.enabled = true;
            //    PlayerCamera.SetActive(true);
            //    Player.GetComponent<PlayerStats>().canMove = true;
            //    Player.GetComponent<PlayerUI>().DisplayInteractUI("Touch Stones");
            //    Cursor.lockState = CursorLockMode.Locked;
            //    Cursor.visible = false;       
            //}
            
        }

        if (StoneRed.GetComponent<StoneOnClick>().activated && !redActive)
        {
            activeStones++;
            redActive = true;
            orderActive += "r";
        }
        if (StonePurple.GetComponent<StoneOnClick>().activated && !purpleActive)
        {
            activeStones++;
            purpleActive = true;
            orderActive += "p";
        }
        if (StoneBlue.GetComponent<StoneOnClick>().activated && !blueActive)
        {
            activeStones++;
            blueActive = true;
            orderActive += "b";
        }
        if (StoneCyan.GetComponent<StoneOnClick>().activated && !cyanActive)
        {
            activeStones++;
            cyanActive = true;
            orderActive += "c";
        }
        if (StoneGreen.GetComponent<StoneOnClick>().activated && !greenActive)
        {
            activeStones++;
            greenActive = true;
            orderActive += "g";
        }
        if (StoneYellow.GetComponent<StoneOnClick>().activated && !yellowActive)
        {
            activeStones++;
            yellowActive = true;
            orderActive += "y";
        }
        if (activeStones == 6)
        {
            checkOrder();
        }
        if(opening)
        {
            // puzzle solved open doors
            StartCoroutine(OpenDoor());
        }
        if(changeCamera)
        {
            //Debug.Log("Give camera back.");
            SecretDoorCamera.SetActive(false);
            PlayerCamera.SetActive(true);
            PlayerHair.SetActive(true);
            PlayerMesh.SetActive(true);
            Player.GetComponent<CharacterController>().enabled = true;
            PlayerController.enabled = true;

            Player.GetComponent<PlayerUI>().DisplayHintUI("THE PRIEST! I must hide under the bed before he sees me!");

            Player.GetComponent<PlayerStats>().canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            changeCamera = false;
        }
    }
    private void delayDoor()
    {
        opening = true;
        doorOpen.Play();

    }

    private void delayPriest()
    {
        breathing.SetActive(true);
        Priest.GetComponent<EnemyAIFSM>().SetStateToPatrol();
    }

    // Check order when finished and give control back to player
    private void checkOrder()
    {
        if (orderActive == "rygcbp")
        {
            //Priest.GetComponent<NavMeshAgent>().enabled = true;
            //Priest.GetComponent<Animator>().enabled = true;
            Debug.Log("Puzzle Solved");
            PuzzleCamera.SetActive(false);
            SecretDoorCamera.SetActive(true);
            interacting = false;
            activeStones = 0;
            Invoke("delayDoor", 1f);
            //Invoke("delayPriest", 4f);
            SpawnEnemies.Invoke();
        }
        else
        {
            Debug.Log("Puzzle Failed");
            Player.GetComponent<PlayerUI>().DisplayHintUI("Puzzle Failed");
            puzzleFailed = true;
            resetPuzzle();
            PuzzleCamera.SetActive(false);
            PlayerMesh.SetActive(true);
            PlayerHair.SetActive(true);
            Player.GetComponent<CharacterController>().enabled = true;
            PlayerController.enabled = true;
            PlayerCamera.SetActive(true);
            Player.GetComponent<PlayerStats>().canMove = true;
            interacting = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Player.GetComponent<PlayerUI>().DisplayInteractUI("Touch Stones");
            //Debug.Log("Puzzle zone entered.");
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

    private void resetPuzzle()
    {
        puzzleStarted = false;
        puzzleSolved = false;
        redActive = false;
        purpleActive = false;
        blueActive = false;
        cyanActive = false;
        greenActive = false;
        yellowActive = false;
        StoneYellow.GetComponent<StoneOnClick>().resetStone();
        StoneRed.GetComponent<StoneOnClick>().resetStone();
        StoneBlue.GetComponent<StoneOnClick>().resetStone();
        StoneCyan.GetComponent<StoneOnClick>().resetStone();
        StoneGreen.GetComponent<StoneOnClick>().resetStone();
        StonePurple.GetComponent<StoneOnClick>().resetStone();
        activeStones = 0;
        orderActive = "";

    }

    IEnumerator OpenDoor()
    {
        //Debug.Log("Opening door!");
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            SecretDoor.transform.position = Vector3.Lerp(closedPos, openPos, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        SecretDoor.transform.position = openPos;
        opening = false;
        changeCamera = true;
    }
}
