using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Hide : MonoBehaviour
{
    //private GameObject player;
    GameObject Magistrate;
    GameObject Player;
    GameObject PlayerMesh;
    GameObject PlayerHair;

    private PlayerController controller; // Reference to PlayerController class

    [SerializeField] GameObject normalCam; // Virtual Normal Camera
    [SerializeField] GameObject hidingCam; // Virtual Hiding Camera

    private bool collisionEntered = false; // Flag to see if the player is in range to hide

    [SerializeField]
    public float sensitivity = 2; // Virtual hiding camera settings
    public float smoothing = 1.5f;
    Vector2 velocity;
    Vector2 frameVelocity;

    // Wardrobe positions
    [SerializeField] Transform door;
    [SerializeField] Transform openPos;


    // Start is called before the first frame update
    void Start()
    {
        Magistrate = GameObject.Find("Magistrate");
        Player = Magistrate.transform.GetChild(0).gameObject;
        PlayerHair = Player.transform.GetChild(1).gameObject;
        PlayerMesh = Player.transform.GetChild(0).gameObject;

        // Get the "Normal VCamera" object
        //normalCam = GameObject.Find("Normal VCamera");
        normalCam = Magistrate.transform.gameObject.transform.Find("Normal VCamera").gameObject;
        // Get the "Hiding VCamera" object OF the hiding spot
        hidingCam = this.transform.parent.gameObject.transform.Find("Hide VCamera").gameObject;

        // If the hiding spot is a wardrobe
        if(this.transform.parent.gameObject.name.Contains("(Hinge)"))
        {
            door = this.transform.parent.gameObject.transform.Find("Door").gameObject.transform;
            openPos = this.transform.parent.gameObject.transform.Find("OpenPosition").gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If player presses the "E" key when inside the collision trigger
        // NOTE: Had to use "Input.GetKeyDown" rather than "input.interact" since the latter would trigger multiple times

        // Enter hiding spot
        if(collisionEntered && Input.GetKeyDown(KeyCode.E) && normalCam.activeInHierarchy)
        {
            EnterHidingSpot();
        }
        // Leave hiding spot
        else if(collisionEntered && Input.GetKeyDown(KeyCode.E) && !normalCam.activeInHierarchy)
        {
            LeaveHidingSpot();
        }
        // While hiding, enable camera movement
        if(collisionEntered && !normalCam.activeInHierarchy)
        {
            CameraRotation();
        }
    }

    // When player enters trigger zone
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            collisionEntered = true;
            Player.GetComponent<PlayerUI>().DisplayInteractUI("Hide");
        }
    }

    // When player exits trigger zone
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            collisionEntered = false;
            Player.GetComponent<PlayerUI>().DisableInteractUI();
        }
    }

    // Controls camera rotation while hiding
    void CameraRotation()
    {
        // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;

        // Rotate camera left-right
        hidingCam.transform.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }
    
    // Enters a hiding spot
    void EnterHidingSpot()
    {
        // Reset player movement
        Player.GetComponent<InputsManager>().move = Vector2.zero;
        Player.GetComponent<InputsManager>().look = Vector2.zero;

        Player.GetComponent<Animator>().SetFloat("speed", 0);
        Player.GetComponent<PlayerController>().hiding = true;
        normalCam.SetActive(false);
        hidingCam.SetActive(true);
        //player.SetActive(false);
        //Player.SetActive(false);

        Player.GetComponent<PlayerController>().enabled = false;
        PlayerMesh.SetActive(false);
        PlayerHair.SetActive(false);
        //controller.enabled = false;

        // If the hiding spot is a Wardrobe
        if (this.transform.parent.gameObject.name.Contains("(Hinge)")) //== "Hideable Wardrobe")
        {
            // Open the door immediately
            door.SetPositionAndRotation(openPos.position, openPos.rotation);

            // Open the door
            GetComponent<HideWardrobe>().open = true;
        }
        Player.GetComponent<PlayerUI>().DisableHintUI(); // hide hint ui
        Player.GetComponent<PlayerUI>().DisplayInteractUI("Exit"); // display exit text
    }

    // Leave hiding spot
    void LeaveHidingSpot()
    {
        Player.GetComponent<PlayerController>().hiding = false;
        normalCam.SetActive(true);
        hidingCam.SetActive(false);
        //player.SetActive(true);
        //Player.SetActive(true);

        Player.GetComponent<PlayerController>().enabled = true;
        PlayerMesh.SetActive(true);
        PlayerHair.SetActive(true);
        //controller.enabled = true;

        // If the hiding spot is a Wardrobe
        if (this.transform.parent.gameObject.name.Contains("(Hinge)"))
        {
            // Close the door
            GetComponent<HideWardrobe>().open = false;
        }
        Player.GetComponent<PlayerUI>().DisplayInteractUI("Hide"); // display hide text
    }


}
