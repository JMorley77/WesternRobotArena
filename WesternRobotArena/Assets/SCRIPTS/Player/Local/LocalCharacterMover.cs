
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LocalInputHandler))]
public class LocalCharacterMover : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private PlayerAnimation playerAnimation;

    private LocalInputHandler input;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    private Camera camera;

    private void Awake()
    {
        input = GetComponent<LocalInputHandler>();
    }

    private void Update()
    {
        if (camera == null)
            camera = Camera.main;

        if (camera == null)
            return;

        Vector3 targetVector = new Vector3(input.InputVector.x, 0f, input.InputVector.y);

        MoveTowardTarget(targetVector);

        // Animation
        if (targetVector.magnitude < 0.1f)
        {
            playerAnimation.Idle();
        }
        else if (Keyboard.current.leftShiftKey.isPressed)
        {
            playerAnimation.Run();
        }
        else
        {
            playerAnimation.Walk();
        }

        // aiming
        if (input.UsingKeyboardMouse)
        {
            RotateTowardMouse();
        }
        else
        {
            RotateTowardControllerAim();
        }
    }

    private void MoveTowardTarget(Vector3 targetVector)
    {
        var speed = moveSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * targetVector;

        var targetPosition = transform.position + targetVector * speed;

        transform.position = targetPosition;
    }

    private void RotateTowardControllerAim()
    {
        Vector2 aimInput = input.AimVector;

        if (aimInput.sqrMagnitude < 0.01f)
            return;

        Vector3 aimDirection = new Vector3(aimInput.x, 0, aimInput.y);

        aimDirection = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * aimDirection;

        Quaternion targetRotation = Quaternion.LookRotation(aimDirection);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private void RotateTowardMouse()
    {
        Ray ray = camera.ScreenPointToRay(input.MousePosition);

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