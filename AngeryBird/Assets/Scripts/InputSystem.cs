using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    public static PlayerInput playerInput;

    private InputAction mousePositionAction;
    private InputAction mouseAction;

    public static Vector2 mousePosition;

    public static bool wasLeftButtonWasPressed;
    public static bool wasLeftButtonWasReleased;
    public static bool wasLeftButtonIsPressed;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        mousePositionAction = playerInput.actions["MousePosition"];
        mouseAction = playerInput.actions["Mouse"];

    }

    private void Update()
    {
        mousePosition = mousePositionAction.ReadValue<Vector2>();

        wasLeftButtonWasPressed = mouseAction.WasPressedThisFrame();
        wasLeftButtonWasReleased = mouseAction.WasReleasedThisFrame();
        wasLeftButtonIsPressed = mouseAction.IsPressed();
    }
}
