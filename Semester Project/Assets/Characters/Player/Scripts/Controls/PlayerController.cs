using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 2f; // Player movement speed;
    [SerializeField] float sprintSpeed = 7f;

    [SerializeField] Transform cameraFollowTarget; // Transform of object the camera will rotate around

    private InputsManager input; // Reference to InputsManager for controls
    private CharacterController controller; // Reference to CharacterController of character
    private Animator animator; // Reference to Animator component

    private float xRotation; // x-axis Camera movement
    private float yRotation; // y-axis Camera movement

    [SerializeField] GameObject mainCam; // For camera movement of main camera
    [SerializeField] GameObject normalCam; // Virtual Normal Camera
    [SerializeField] GameObject aimCam; // Virtual Aim Camera

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
        // Speed is temporarily 0 as the player is idle
        float speed = 0;
        // Prevents controls being restricted to 2D plane
        Vector3 inputDirection = new Vector3(input.move.x, 0, input.move.y);
        // For character rotation
        float targetRotation = 0;
        // If player does "Move" action
        if (input.move != Vector2.zero)
        {
            if (input.sprint)
            {
                speed = sprintSpeed;
            }
            else
            {
                speed = walkSpeed;
            }
            targetRotation = Quaternion.LookRotation(inputDirection).eulerAngles.y + mainCam.transform.rotation.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 20 * Time.deltaTime);

            // 2 For Sprint animation, 1 for Walk animation
            animator.SetFloat("speed", input.sprint ? 2 : input.move.magnitude);
        }
        else
        {
            // 0 For Idle animation
            animator.SetFloat("speed", 0);
        }
        Vector3 targetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;
        // Moves character towards a direction
        controller.Move(targetDirection * speed * Time.deltaTime);
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

}