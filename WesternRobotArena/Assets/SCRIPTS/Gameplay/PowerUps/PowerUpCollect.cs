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
        inputHandler = GetComponent<LocalInputHandler>();
    }

    private void Update()
    {
        if (inputHandler != null && inputHandler.InteractPressed && canPickUp)
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
        if (powerUp == null || powerUpManager == null)
        {
            return;
        }

        PowerUp pickup = powerUp.GetComponentInParent<PowerUp>();

        Debug.Log("Power up collected");

        powerUpManager.ActivatePowerUp(pickup.powerUpData);

        Destroy(powerUp);

        powerUp = null;
        canPickUp = false;
    }
}
