
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LocalInputHandler))]
public class LocalCharacterMover : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private PlayerAnimation playerAnimation;

    private LocalInputHandler input;

    [SerializeField] public float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField, Range(0.1f, 1f)] private float walkRatio = 0.5f;
    [SerializeField] private float clipNaturalSpeed = 4.5f;



    private Camera cam;

    private void Awake()
    {
        input = GetComponent<LocalInputHandler>();
    }

    private void Update()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        Vector2 raw = input.InputVector;
        Vector3 targetVector = Vector3.ClampMagnitude(new Vector3(raw.x, 0f, raw.y), 1f);
        if (targetVector.magnitude < 0.1f) 
            targetVector = Vector3.zero;

        Vector3 worldMovement = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0) * targetVector;


        bool running =
            (Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed) ||
            (Gamepad.current != null && Gamepad.current.leftStickButton.isPressed);

        float speedRatio = running ? 1f : walkRatio;


        transform.position += worldMovement * (moveSpeed * speedRatio * Time.deltaTime);

        float actualSpeed = moveSpeed * speedRatio * worldMovement.magnitude;


        float animationMultiplier = actualSpeed / clipNaturalSpeed;

        if (actualSpeed < 0.1f)
            animationMultiplier = 1f;
        else
            animationMultiplier = Mathf.Clamp(animationMultiplier, 0.1f, 2f);

        playerAnimation.SetLocomotionSpeed(animationMultiplier);


        Vector3 localMovement = transform.InverseTransformDirection(worldMovement);

        float direction = localMovement.x * speedRatio;
        float forward = localMovement.z * speedRatio;

        float movementAmount = localMovement.magnitude * speedRatio;

        playerAnimation.SetMovement(direction, forward, movementAmount);

        // Aiming
        if (input.UsingKeyboardMouse) RotateTowardMouse();
        else RotateTowardControllerAim();
    }

    private void MoveTowardTarget(Vector3 targetVector)
    {
        var speed = moveSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0) * targetVector;

        var targetPosition = transform.position + targetVector * speed;

        transform.position = targetPosition;
    }

    private void RotateTowardControllerAim()
    {
        Vector2 aimInput = input.AimVector;

        if (aimInput.sqrMagnitude < 0.01f)
            return;

        Vector3 aimDirection = new Vector3(aimInput.x, 0, aimInput.y);

        aimDirection = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0) * aimDirection;

        Quaternion targetRotation = Quaternion.LookRotation(aimDirection);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private void RotateTowardMouse()
    {
        Ray ray = cam.ScreenPointToRay(input.MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 300f))
        {
            Vector3 target = hitInfo.point;

            target.y = transform.position.y;

            Vector3 direction = target - transform.position;

            if (direction.sqrMagnitude < 0.01f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);



            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
}