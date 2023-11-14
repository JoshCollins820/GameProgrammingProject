using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;


public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2f; // Player movement speed;
    public float sprintSpeed = 7f;

    [SerializeField] Transform cameraFollowTarget; // Transform of object the camera will rotate around

    private InputsManager input; // Reference to InputsManager for controls
    private CharacterController controller; // Reference to CharacterController of character
    private Animator animator; // Reference to Animator component

    private float xRotation; // x-axis Camera movement
    private float yRotation; // y-axis Camera movement

    [SerializeField] GameObject mainCam; // For camera movement of main camera
    [SerializeField] GameObject normalCam; // Virtual Normal Camera
    [SerializeField] GameObject aimCam; // Virtual Aim Camera

    public bool hiding = false; // For enemy detection

    Vector3 moveVector; // For moving



    // Start is called before the first frame update
    void Start()
    {
        // Lock mouse onto screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get the InputsManager script
        input = GetComponent<InputsManager>();
        // Get the CharacterController component
        controller = GetComponent<CharacterController>();
        // Get the Animator component
        animator = GetComponent<Animator>();

        // Get the CameraFollowTarget object's transform
        cameraFollowTarget = GameObject.Find("Player/CameraFollowTarget").GetComponent<Transform>();
        // Get the MainCamera object
        mainCam = GameObject.Find("Main Camera"); // Main Camera
        // Get the "Normal VCamera" object
        normalCam = GameObject.Find("Normal VCamera");
        // Get the "Aim VCamera" object
        aimCam = GameObject.Find("Aim VCamera");
        aimCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ApplyMovement();
        ApplyGravity();
    }

    // Used for smoothness
    private void LateUpdate()
    {
        // Apply camera movement
        CameraRotation();
        
        // Switch between default camera/aim camera
        if(input.aim && !aimCam.activeInHierarchy)
        {
            aimCam.SetActive(true);
            normalCam.SetActive(false);
        }
        else if(!input.aim && !normalCam.activeInHierarchy)
        {
            aimCam.SetActive(false);
            normalCam.SetActive(true);
        }
    }

    // Controls camera rotation
    void CameraRotation()
    {
        xRotation += input.look.y/5;
        yRotation += input.look.x/5;
        xRotation = Mathf.Clamp(xRotation, -30, 35); // -30, 70

        Quaternion rotation = Quaternion.Euler(-xRotation, yRotation, 0);
        cameraFollowTarget.rotation = rotation;
    }

    // Controls player movement
    void ApplyMovement()
    {
        // Speed is temporarily 0 as the player is idle
        float speed = 0;
        // Prevents controls being restricted to 2D plane
        Vector3 inputDirection = new Vector3(input.move.x, 0, input.move.y);
        // For character rotation
        float targetRotation = 0;
        // If player does "Move" action
        if (input.move != Vector2.zero)
        {
            // If player sprints
            if(input.sprint && !GetComponent<PlayerStats>().isExausted)
            {
                animator.SetFloat("speed", 2); // Sprint animation
                speed = sprintSpeed; // Change speed to sprint speed
                GetComponent<PlayerStats>().useStamina(); // Use stamina
            }
            // If player walks
            else
            {
                animator.SetFloat("speed", 1); // Walk animation
                speed = walkSpeed; // Change speed to walk speed
                GetComponent<PlayerStats>().isRunning = false; // Let stamina controller know player is not running
            }
            targetRotation = Quaternion.LookRotation(inputDirection).eulerAngles.y + mainCam.transform.rotation.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 20 * Time.deltaTime);
        }
        // If the player is standing still
        else
        {
            // 0 For Idle animation
            animator.SetFloat("speed", 0);
            GetComponent<PlayerStats>().isRunning = false; // Let stamina controller know player is not running
        }

        // Move player
        Vector3 targetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;
        controller.Move(speed * Time.deltaTime * targetDirection);
    }

    // Applies gravity to player
    void ApplyGravity()
    {
        // Adds gravity to character
        moveVector = Vector3.zero;
        if(!controller.isGrounded)
        {
            moveVector += Physics.gravity;
        }
        controller.Move(moveVector * Time.deltaTime);
    }


}