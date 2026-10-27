using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class InputHandler : NetworkBehaviour
{
    public Vector2 InputVector { get; private set; }

    public Vector2 AimVector { get; private set; }

    public Vector2 MousePosition { get; private set; }

    public bool ShootHeld { get; private set; }
    public bool ShootPressed { get; private set; }
    public bool SwitchWeaponPressed { get; private set; }

    public bool UsingKeyboardMouse { get; private set; }

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction aimAction;
    private InputAction pointAction;

    private InputAction shootAction;

    private InputAction switchWeaponAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        aimAction = playerInput.actions["Aim"];
        pointAction = playerInput.actions["Point (Mouse Aim)"];
        shootAction = playerInput.actions["Shoot"];
        switchWeaponAction = playerInput.actions["SwitchWeapon"];

        playerInput.enabled = false;
    }

    public override void OnNetworkSpawn()
    {
        playerInput.enabled = IsOwner;
    }

    private void Update()
    {
        if (!IsOwner)
            return;

        InputVector = moveAction.ReadValue<Vector2>();
        AimVector = aimAction.ReadValue<Vector2>();
        MousePosition = pointAction.ReadValue<Vector2>();

        ShootHeld = shootAction.IsPressed();
        ShootPressed = shootAction.WasPressedThisFrame();

        SwitchWeaponPressed = switchWeaponAction.WasPressedThisFrame();

        UsingKeyboardMouse = playerInput.currentControlScheme == "Keyboard&Mouse";
    }
}