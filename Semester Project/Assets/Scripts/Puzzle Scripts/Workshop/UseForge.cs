using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseForge : MonoBehaviour
{
    public GameObject SwordPieces;
    public GameObject Magistrate;
    public GameObject Player;
    public GameObject PlayerHair;
    public GameObject PlayerMesh;
    public CharacterController PlayerController;
    //public GameObject SwordInHand;
    public GameObject SwordRebuilt;
    [SerializeField] GameObject PlayerCamera;
    [SerializeField] GameObject ForgeCamera;

    public bool interacting;
    public bool reforge;
    public int collected;

    // Start is called before the first frame update
    void Start()
    {
        SwordPieces = GameObject.Find("SwordPieces");
        Magistrate = GameObject.Find("Magistrate");
        Player = Magistrate.transform.GetChild(0).gameObject;
        PlayerHair = Player.transform.GetChild(1).gameObject;
        PlayerMesh = Player.transform.GetChild(0).gameObject;
        PlayerController = Player.GetComponent<CharacterController>();
        PlayerCamera = Magistrate.transform.Find("Normal VCamera").gameObject;
        ForgeCamera = transform.GetChild(4).gameObject;
        SwordRebuilt = transform.GetChild(3).gameObject;
        //SwordInHand = Player.transform.Find("Sword_Stormbringer").gameObject;
        interacting = false;
        reforge = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interacting)
        {
            Player.GetComponent<PlayerUI>().DisableInteractUI();
            // check if we can reforge the sword
            if (!reforge)
            {
                CheckPiecesCollected();
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            interacting = true;
            Player.GetComponent<PlayerUI>().DisplayInteractUI("Use Forge");
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

    private void CheckPiecesCollected()
    {
        collected = SwordPieces.GetComponent<TrackPieces>().swordPieces;
        reforge = SwordPieces.GetComponent<TrackPieces>().complete;

        if (collected == 0)
        {
            Player.GetComponent<PlayerUI>().DisplayHintUI("I have nothing to Forge...");
        }
        if (collected > 0 && !reforge)
        {
            Player.GetComponent<PlayerUI>().DisplayHintUI("I don't have all the pieces\n" +
                "of the sword yet...");
        }
        if (reforge)
        {
            Player.GetComponent<PlayerStats>().canMove = false;
            PlayerCamera.SetActive(false);
            Debug.Log("Player camera turned off");
            Debug.Log("Sword camera turned on");
            PlayerMesh.SetActive(false);
            PlayerHair.SetActive(false);
            Player.GetComponent<CharacterController>().enabled = false;
            PlayerController.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerCamera.gameObject.SetActive(false);
            ForgeCamera.gameObject.SetActive(true);
            SwordRebuilt.gameObject.SetActive(true);
            Invoke(nameof(PlayerHasSwordNow), 2f);
        }
    }

    private void PlayerHasSwordNow()
    {
        ForgeCamera.gameObject.SetActive(false);
        PlayerMesh.SetActive(true);
        PlayerHair.SetActive(true);
        Player.GetComponent<CharacterController>().enabled = true;
        PlayerController.enabled = true;
        PlayerCamera.SetActive(true);
        Player.GetComponent<PlayerStats>().canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
