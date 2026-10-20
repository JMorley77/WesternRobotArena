using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 InputVector { get; private set; }

    public Vector2 AimVector { get; private set; }

    public Vector2 MousePosition { get; private set; }

    public bool ShootHeld { get; private set; }

    public bool UsingKeyboardMouse { get; private set; }

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction aimAction;
    private InputAction pointAction;

    private InputAction shootAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        aimAction = playerInput.actions["Aim"];
        pointAction = playerInput.actions["Point (Mouse Aim)"];
        shootAction = playerInput.actions["Shoot"];
    }

    private void Update()
    {
        InputVector = moveAction.ReadValue<Vector2>();

        AimVector = aimAction.ReadValue<Vector2>();

        MousePosition = pointAction.ReadValue<Vector2>();

        ShootHeld = shootAction.IsPressed();

        UsingKeyboardMouse = playerInput.currentControlScheme == "Keyboard&Mouse";
    }
}