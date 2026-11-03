using UnityEngine;

public class PowerUpManger : MonoBehaviour
{
    public float timeDuration = 10;

    [Header("References")]
    //public CharacterMover playerMovement;
    public JackTestDontUse playerMovement;
    public PlayerHealthLocal localHealth;
    public PlayerHealth multiplayerHealth;
    
    private float originalSpeed;

    private float speedTimer;
    private float invincibilityTimer;

    private bool isSpeedActive;
    private bool isInvincibilityActive;

    public void Start()
    {
        if(playerMovement != null)
        {
            originalSpeed = playerMovement.speed;
        }
    }

    void Update()
    {
        // Speed timer
        if (isSpeedActive)
        {
            speedTimer -= Time.deltaTime;

            if (speedTimer <= 0)
            {
                playerMovement.speed = originalSpeed;
                isSpeedActive = false;
            }
        }

        // Invincibility timer
        if (isInvincibilityActive)
        {
            invincibilityTimer -= Time.deltaTime;

            if (invincibilityTimer <= 0)
            {
                isInvincibilityActive = false;
            }
        }
    }

    public void ActivatePowerUp(PowerUpData data)
    {
        if (data == null)
            return;

        switch (data.powerUpType)
        {
            case PowerUpType.Speed:
                Speed(data);
                break;
            case PowerUpType.Invincibility:
                Invincibility(data);
                break;
        }
    }

    private void Speed(PowerUpData data)
    {
        if (!isSpeedActive)
        {
            originalSpeed = playerMovement.speed;
        }

        playerMovement.speed = originalSpeed * data.multiplier;

        speedTimer = data.duration;
        isSpeedActive = true;
        Debug.Log("Speed power-up activated for " + data.duration + " seconds!");
    }

    private void Invincibility(PowerUpData data)
    {
        if (localHealth != null)
            localHealth.Invincibility(data.duration);

        if (multiplayerHealth != null)
            multiplayerHealth.Invincibility(data.duration);

        Debug.Log("Invincibility power-up activated for " + data.duration + " seconds!");
    }
}