using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputsManager : MonoBehaviour
{
    public Vector2 move; // Needed for x-axis, z-axis
    public Vector2 look; // Needed for camera
    public bool sprint; // Needed to determine if player is currently sprinting
    public bool aim; // Needed to determine if player is aiming
    public bool interact; // Needed to determine if player is interacting

    // Gets player input based on their "Move" action (WASD Keys)
    void OnMove(InputValue value)
    {
        // Get user input for x/z movement
        move = value.Get<Vector2>();
    }

    // Gets player input based on their "Look" action (Mouse Movement)
    void OnLook(InputValue value)
    {
        // Get user input based for camera movement
        look = value.Get<Vector2>();
    }

    // Gets player input based on their "Sprint" action (Left Shift)
    void OnSprint(InputValue value)
    {
        sprint = value.isPressed;
    }

    // Gets player input based on their "Aim" action (Right Mouse Click)
    void OnAim(InputValue value)
    {
        aim = value.isPressed;
    }

    // Gets player input based on their "Interact" action (E Key)
    void OnInteract(InputValue value)
    {
        interact = value.isPressed;
    }
}
