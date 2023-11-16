using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BasementPuzzle : MonoBehaviour
{
    GameObject Player;
    GameObject StoneRed;
    GameObject StonePurple;
    GameObject StoneBlue;
    GameObject StoneCyan;
    GameObject StoneGreen;
    GameObject StoneYellow;
    [SerializeField] GameObject PuzzleCamera;
    [SerializeField] GameObject PlayerCamera;
    [SerializeField] GameObject SecretDoorCamera;
    GameObject SecretDoor;
    public AudioSource doorOpen;
    public GameObject Priest;

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


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Magistrate");
        Priest = GameObject.Find("PriestTest");
        StoneRed = GameObject.Find("Stone_Red");
        StonePurple = GameObject.Find("Stone_Purple");
        StoneBlue = GameObject.Find("Stone_Blue");
        StoneCyan = GameObject.Find("Stone_Cyan");
        StoneGreen = GameObject.Find("Stone_Green");
        StoneYellow = GameObject.Find("Stone_Yellow");
        PuzzleCamera = this.transform.gameObject.transform.Find("Puzzle Camera").gameObject;
        PlayerCamera = Player.transform.gameObject.transform.Find("Normal VCamera").gameObject;
        SecretDoorCamera = this.transform.gameObject.transform.Find("SecretDoorCamera").gameObject;
        SecretDoor = GameObject.Find("SecretDoor");

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
        if (Input.GetKeyDown(KeyCode.E) && interacting)
        {
            PlayerCamera.SetActive(false);
            PuzzleCamera.SetActive(true);
            Player.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            puzzleStarted = true;
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
            Debug.Log("Give camera back.");
            SecretDoorCamera.SetActive(false);
            PlayerCamera.SetActive(true);
            Player.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            changeCamera = false;
        }
    }

    // Check order when finished and give control back to player
    private void checkOrder()
    {
        if (orderActive == "rygcbp")
        {
            //Priest.GetComponent<NavMeshAgent>().enabled = true;
            //Priest.GetComponent<Animator>().enabled = true;
            Priest.GetComponent<EnemyAIFSM>().SetStateToPatrol();
            Debug.Log("Puzzle Solved");
            opening = true;
            PuzzleCamera.SetActive(false);
            SecretDoorCamera.SetActive(true);
            interacting = false;
            activeStones = 0;
            doorOpen.Play();
        }
        else
        {
            Debug.Log("Puzzle Failed");
            puzzleFailed = true;
            resetPuzzle();
            PuzzleCamera.SetActive(false);
            Player.SetActive(true);
            PlayerCamera.SetActive(true);
            interacting = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Puzzle zone entered.");
            interacting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            interacting = false;
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
        activeStones = 0;
        orderActive = "";
    }

    IEnumerator OpenDoor()
    {
        Debug.Log("Opening door!");
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
