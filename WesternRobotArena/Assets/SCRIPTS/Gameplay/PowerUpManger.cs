using UnityEngine;

public class PowerUpManger : MonoBehaviour
{
    public float timeDuration = 10;

    [Header("Speed")]
    public CharacterMover playerMovement;
    public float speedMultiplier = 2;
    private float originalSpeed;
    private bool isSpeedActive;
    
    void Update()
    {
        if (!isSpeedActive)
        {
            return;
        }
        
        timeDuration -= Time.deltaTime;

        if (timeDuration <= 0)
        {
            playerMovement.moveSpeed = originalSpeed;
            isSpeedActive = false;
        }
    }

    public void Speed()
    {
        Debug.Log("Speed method called");

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement reference is missing!");
            return;
        }

        Debug.Log("Speed before boost: " + playerMovement.moveSpeed);

        if (isSpeedActive)
        {
            timeDuration = 10;
            return;
        }

        originalSpeed = playerMovement.moveSpeed;
        playerMovement.moveSpeed = originalSpeed * speedMultiplier;

        Debug.Log("Speed after boost: " + playerMovement.moveSpeed);

        timeDuration = 10;
        isSpeedActive = true;
    }
}