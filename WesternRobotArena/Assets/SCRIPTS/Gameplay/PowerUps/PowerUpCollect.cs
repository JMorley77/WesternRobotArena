using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUpCollect : MonoBehaviour
{
    public PowerUpManger powerUpManager;
    private LocalInputHandler inputHandler;
    private GameObject powerUp;
    private bool canPickUp;

    private void Awake()
    {
        // Get the LocalInputHandler component from the player
        inputHandler = GetComponent<LocalInputHandler>();
    }

    private void Update()
    {
        // Check if the player can pick up a power up and if the interact button is pressed
        if (inputHandler != null && inputHandler.InteractPressed && canPickUp)
        {
            PickUp();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has entered the trigger of a power up, allow them to pick it up
        if (other.CompareTag("PowerUp"))
        {
            powerUp = other.gameObject;
            canPickUp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the player exits the trigger of a power up, dont allow them to pick it up anymore
        if (other.gameObject == powerUp)
        {
            powerUp = null;
            canPickUp = false;
        }
    }

    private void PickUp()
    {
        // Check if the power up and power up manager are attached
        if (powerUp == null || powerUpManager == null)
        {
            return;
        }

        // PLay Pickup Animation
        GetComponent<PlayerAnimation>().Pickup();

        // Find the PowerUp component on the power up object to get the power up data
        PowerUp pickup = powerUp.GetComponentInParent<PowerUp>();

        Debug.Log("Power up collected");

        // Activate the power up
        powerUpManager.ActivatePowerUp(pickup.powerUpData);

        // Remove the power up from the scene
        Destroy(powerUp);
        
        powerUp = null;
        canPickUp = false;
    }
}
