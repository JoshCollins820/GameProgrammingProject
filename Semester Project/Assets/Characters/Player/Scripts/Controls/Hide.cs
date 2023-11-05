using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Hide : MonoBehaviour
{
    private GameObject player; // Needed to grab InputsManager script

    private InputsManager input; // Reference to InputsManager for controls
    private PlayerController controller; // Get instance of the PlayerController class

    [SerializeField] GameObject normalCam; // Virtual Normal Camera
    [SerializeField] GameObject hidingCam; // Virtual Hiding Camera

    private bool collisionEntered = false; // Flag to see if the player is in range to hide

    [SerializeField]
    public float sensitivity = 2; // Virtual hiding camera settings
    public float smoothing = 1.5f;
    Vector2 velocity;
    Vector2 frameVelocity;


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
    }

    // Update is called once per frame
    void Update()
    {
        // If player presses the "E" key when inside the collision trigger
        // NOTE: Had to use "Input.GetKeyDown" rather than "input.interact" since the latter would trigger multiple times

        // Enter hiding spot
        if(collisionEntered && Input.GetKeyDown(KeyCode.E) && normalCam.activeInHierarchy)
        {
            normalCam.SetActive(false);
            hidingCam.SetActive(true);
            player.SetActive(false);
        }
        // Leave hiding spot
        else if (collisionEntered && Input.GetKeyDown(KeyCode.E) && !normalCam.activeInHierarchy)
        {
            normalCam.SetActive(true);
            hidingCam.SetActive(false);
            player.SetActive(true);
        }
        // While hiding, enable camera movement
        if (collisionEntered && !normalCam.activeInHierarchy)
        {
            CameraRotation();
        }
    }

    // When player enters trigger zone
    void OnTriggerEnter(Collider other)
    {
        collisionEntered = true;
    }

    // When player exits trigger zone
    private void OnTriggerExit(Collider other)
    {
        collisionEntered = false;
    }

    // Controls camera rotation while hiding
    void CameraRotation()
    {
        // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        //velocity.y = Mathf.Clamp(velocity.y, -45, 45); //-90, 90

        // Rotate camera up-down and controller left-right from velocity.
        //hidingCam.transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        hidingCam.transform.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }
    


}
