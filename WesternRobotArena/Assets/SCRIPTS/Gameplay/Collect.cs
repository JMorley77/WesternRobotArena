using UnityEngine;
using UnityEngine.InputSystem;

public class Collect : MonoBehaviour
{
    public PowerUpManger powerUpManager;
    private GameObject powerUp;
    private bool canPickUp;

    void Start()
    {
        canPickUp = false;
    }

    void Update()
    {
        if (canPickUp && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
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

        PowerUp pickup = powerUp.GetComponent<PowerUp>();

        powerUpManager.ActivatePowerUp(pickup.powerUpData);

        Destroy(powerUp);

        powerUp = null;
        canPickUp = false;
    }
}
