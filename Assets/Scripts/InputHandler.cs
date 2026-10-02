using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 InputVector { get; private set; }

    public Vector2 AimVector { get; private set; }

    public Vector2 MousePosition { get; private set; }

    public bool UsingKeyboardMouse { get; private set; }

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction aimAction;
    private InputAction pointAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        aimAction = playerInput.actions["Aim"];
        pointAction = playerInput.actions["Point (Mouse Aim)"];
    }

    private void Update()
    {
        InputVector = moveAction.ReadValue<Vector2>();

        AimVector = aimAction.ReadValue<Vector2>();

        MousePosition = pointAction.ReadValue<Vector2>();

        UsingKeyboardMouse = playerInput.currentControlScheme == "Keyboard&Mouse";
    }
}