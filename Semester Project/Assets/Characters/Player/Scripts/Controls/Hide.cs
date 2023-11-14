using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Hide : MonoBehaviour
{
    private GameObject player; // Needed to grab InputsManager script

    private InputsManager input; // Reference to InputsManager for controls
    //private PlayerController controller; // Get instance of the PlayerController class

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
        // Get the player so we can grab the InputsManager script
        player = GameObject.Find("Player");
        // Get the InputsManager script
        input = player.GetComponent<InputsManager>();
        // Get the "Normal VCamera" object
        normalCam = GameObject.Find("Normal VCamera");
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
            //Reset player movement
            player.GetComponent<InputsManager>().move = Vector2.zero;
            player.GetComponent<InputsManager>().look = Vector2.zero;

            normalCam.SetActive(false);
            hidingCam.SetActive(true);
            player.SetActive(false);
            player.GetComponent<PlayerController>().hiding = true;

            // If the hiding spot is a Wardrobe
            if (this.transform.parent.gameObject.name.Contains("(Hinge)")) //== "Hideable Wardrobe")
            {
                // Open the door immediately
                door.SetPositionAndRotation(openPos.position, openPos.rotation);

                // Open the door
                GetComponent<HideWardrobe>().open = true;
            }
        }
        // Leave hiding spot
        else if(collisionEntered && Input.GetKeyDown(KeyCode.E) && !normalCam.activeInHierarchy)
        {
            normalCam.SetActive(true);
            hidingCam.SetActive(false);
            player.SetActive(true);
            player.GetComponent<PlayerController>().hiding = false;

            // If the hiding spot is a Wardrobe
            if (this.transform.parent.gameObject.name.Contains("(Hinge)"))
            {
                // Close the door
                GetComponent<HideWardrobe>().open = false;
            }
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
        }
    }

    // When player exits trigger zone
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            collisionEntered = false;
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


}
