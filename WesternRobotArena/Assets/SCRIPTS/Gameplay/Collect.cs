using UnityEngine;
using UnityEngine.InputSystem;

public class Collect : MonoBehaviour
{
    public PowerUpManger powerUpManager;
    public GameObject powerUp;
    private bool canPickUp;

    void Start()
    {
        canPickUp = false;
    }

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

        Debug.Log("Power up collected");
        powerUpManager.Speed();
        Destroy(powerUp);

        powerUp = null;
        canPickUp = false;
    }
}
