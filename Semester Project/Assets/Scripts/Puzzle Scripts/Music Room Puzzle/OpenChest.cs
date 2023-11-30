using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{
    GameObject MusicRoomPuzzle;
    GameObject SittingRoomKey;
    GameObject Player;
    GameObject Magistrate;
    GameObject PlayerMesh;
    GameObject PlayerHair;

    [SerializeField] GameObject PlayerCamera;
    [SerializeField] GameObject ChestCamera;
    [SerializeField] GameObject KeyCamera;
    CharacterController PlayerController;

    float slerpDuration;
    public bool opening;
    public bool interacting;
    public bool showClue;      // true if UI should display clue about door, false if showing
    public bool showAction;    // true if UI should display action that can be taken on door
    public bool opened;


    // Start is called before the first frame update
    void Start()
    {
        MusicRoomPuzzle = GameObject.Find("Music Room Puzzle");
        SittingRoomKey = GameObject.Find("SittingRoomKey");
        Magistrate = GameObject.Find("Magistrate");
        Player = Magistrate.transform.GetChild(0).gameObject;
        PlayerHair = Player.transform.GetChild(1).gameObject;
        PlayerMesh = Player.transform.GetChild(0).gameObject;
        PlayerController = Player.GetComponent<CharacterController>();
        PlayerCamera = Magistrate.transform.gameObject.transform.Find("Normal VCamera").gameObject;
        ChestCamera = MusicRoomPuzzle.transform.gameObject.transform.Find("Chest VCamera").gameObject;
        KeyCamera = MusicRoomPuzzle.transform.gameObject.transform.Find("Key VCamera").gameObject;

        slerpDuration = 2f;
        opening = false;
        opened = false;
        showClue = true;
        showAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interacting && !opened)
        {

            Player.GetComponent<PlayerUI>().DisableInteractUI(); // action taken stop display
            if (showClue)
            {
                Player.GetComponent<PlayerUI>().DisplayHintUI("Locked, the mechanism seems connected\n" +
                    "to this pipe in the wall...");
                showClue = false;
            }

        }
        if (MusicRoomPuzzle.GetComponent<MusicRoomPuzzle>().puzzleSolved)
        {
            opening = true;
        }
        if (opening)
        {
            Debug.Log("opening chest");
            // disable lock
            this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            SittingRoomKey.SetActive(true);
            Invoke(nameof(OpeningSequence), 3f);
            StartCoroutine(Rotate90());
            
        }
        if (opened) // get camera back to player
        {
            this.gameObject.GetComponent<SphereCollider>().enabled = false;

        }
    }
        

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            interacting = true;
            Debug.Log("Magistrate entered collider");
            //if puzzle is not solved then tell that it is locked
            if (showAction)
            {
                Player.GetComponent<PlayerUI>().DisplayInteractUI("Open Chest");
                showAction = false;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Magistrate exited collider");
            interacting = false;
            showClue = true;
            showAction = true;
            Player.GetComponent<PlayerUI>().DisableHintUI();
            Player.GetComponent<PlayerUI>().DisableInteractUI();
        }
    }

    private void OpeningSequence()
    {
        ChestCamera.SetActive(false);
        KeyCamera.SetActive(true);
        Invoke(nameof(ReturnCamera), 2f);
    }

    private void ReturnCamera()
    {
        KeyCamera.SetActive(false);
        PlayerMesh.SetActive(true);
        PlayerHair.SetActive(true);
        Player.GetComponent<CharacterController>().enabled = true;
        PlayerController.enabled = true;
        PlayerCamera.SetActive(true);
        Player.GetComponent<PlayerStats>().canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    IEnumerator Rotate90()
    {
        float timeElapsed = 0;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(-90, 0, 0);
        while (timeElapsed < slerpDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / slerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
        Debug.Log("Cell door opened");
        opening = false;
        opened = true;
    }

}
