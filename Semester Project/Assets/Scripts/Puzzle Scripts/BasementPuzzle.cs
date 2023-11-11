using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasementPuzzle : MonoBehaviour
{
    GameObject Player;
    GameObject StoneRed;
    GameObject StonePurple;
    GameObject StoneBlue;
    GameObject StoneCyan;
    GameObject StoneGreen;
    GameObject StoneYellow;
    Camera PuzzleCamera;
    Camera PlayerCamera;
    Camera SecretDoorCamera;
    GameObject SecretDoor;

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


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        StoneRed = GameObject.Find("Stone_Red");
        StonePurple = GameObject.Find("Stone_Purple");
        StoneBlue = GameObject.Find("Stone_Blue");
        StoneCyan = GameObject.Find("Stone_Cyan");
        StoneGreen = GameObject.Find("Stone_Green");
        StoneYellow = GameObject.Find("Stone_Yellow");
        PuzzleCamera = GameObject.Find("Puzzle Camera").GetComponent<Camera>();
        PlayerCamera = Player.GetComponentInChildren<Camera>();
        SecretDoorCamera = GameObject.Find("SecretDoorCamera").GetComponent<Camera>();
        SecretDoor = GameObject.Find("SecretDoor");

        closedPos = SecretDoor.transform.position;
        openPos = new Vector3(SecretDoor.transform.position.x, SecretDoor.transform.position.y, -29.248f);
        lerpDuration = 1.5f;
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
        if (puzzleSolved)
        {
            opening = true;
            // puzzle solved open doors
            StartCoroutine(OpenDoor());
            // wait until door opens before giving camera back to player
            float waitTime = 0f ;
            while (waitTime < 1.5f)
            {
                waitTime += Time.deltaTime;
            }
            SecretDoorCamera.gameObject.SetActive(false);
            PlayerCamera.gameObject.SetActive(true);

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            // TODO: get player canvas and show "E to interact"
            if (Input.GetKeyDown("Interact") && !interacting)
            {
                interacting = true;
                PlayerCamera.gameObject.SetActive(false);
                PuzzleCamera.gameObject.SetActive(true);
                puzzleStarted = true;
            }
        }
    }

    // Check order when finished and give control back to player
    private void checkOrder()
    {
        if (orderActive == "rpbcgy")
        {
            Debug.Log("Puzzle Solved");
            puzzleSolved = true;
            PuzzleCamera.gameObject.SetActive(false);
            SecretDoorCamera.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Puzzle Failed");
            puzzleFailed = true;
            resetPuzzle();
            PuzzleCamera.gameObject.SetActive(false);
            PlayerCamera.gameObject.SetActive(true);
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
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            SecretDoor.transform.position = Vector3.Lerp(closedPos, openPos, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        SecretDoor.transform.position = openPos;
        opening = false;
    }
}
