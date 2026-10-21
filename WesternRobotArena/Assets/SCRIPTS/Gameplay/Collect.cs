using UnityEngine;
using UnityEngine.InputSystem;

public class Collect : MonoBehaviour
{
    public GameObject powerUp;
    private bool canPickUp;

    public CharacterMover playerMovement;
    public float speedMultiplier = 2f;
    public float timeDuration = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canPickUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canPickUp && Keyboard.current[Key.E].wasPressedThisFrame)
        {
            PickUp();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            powerUp = other.gameObject;
            canPickUp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == powerUp)
        {
            powerUp = null;
            canPickUp = false;
        }
    }

    private void PickUp()
    {
        if (powerUp == null)
        {
            return;
        }

        timeDuration -= Time.deltaTime;
        if (timeDuration > 0)
        {
            playerMovement.moveSpeed = playerMovement.moveSpeed * speedMultiplier;
        }

        Debug.Log("Power up collected");
        Destroy(powerUp);
        powerUp = null;
        canPickUp = false;
    }
}
