using UnityEngine;

public class PowerUpManger : MonoBehaviour
{
    public float timeDuration = 10;

    [Header("References")]
    public LocalCharacterMover playerMovement;
    public PlayerHealthLocal localHealth;
    public PlayerHealth multiplayerHealth;
    public LocalPlayerMelee playerMelee;
    public LocalBullet bullet;

    // Store the original values for speed and damage (melee and shooting)
    private float originalSpeed;
    private float originalMeleeDamage;
    private float originalShootDamage;

    // Timers for each power up type
    private float speedTimer;
    private float invincibilityTimer;
    private float damageTimer;

    // Checks if a power up is currently active
    private bool isSpeedActive;
    private bool isInvincibilityActive;
    private bool isDamageActive;

    public void Start()
    {
        // Store the original movement speed
        if(playerMovement != null)
        {
            originalSpeed = playerMovement.moveSpeed;
        }

        // Store the original melee damage
        if (playerMelee != null)
        {
            originalMeleeDamage = playerMelee.meleeDamage;
        }

        // Store the original bullet damage
        if (bullet != null)
        {
            originalShootDamage = bullet.bulletDamage;
        }
    }

    void Update()
    {
        // Speed timer
        if (isSpeedActive)
        {
            speedTimer -= Time.deltaTime;

            // If the timer has run out, reset the players speed
            if (speedTimer <= 0)
            {
                playerMovement.moveSpeed = originalSpeed;
                isSpeedActive = false;
            }
        }

        // Invincibility timer
        if (isInvincibilityActive)
        {
            invincibilityTimer -= Time.deltaTime;

            // If the timer has run out, deactivate the invincibility
            if (invincibilityTimer <= 0)
            {
                isInvincibilityActive = false;
            }
        }

        // Damage timer
        if (isDamageActive)
        {
            damageTimer -= Time.deltaTime;

            // If the timer has run out, reset the players damage
            if (damageTimer <= 0)
            {
                playerMelee.meleeDamage = originalMeleeDamage;
                bullet.bulletDamage = originalShootDamage;
                isDamageActive = false;
            }
        }
    }

    public void ActivatePowerUp(PowerUpData data)
    {
        // Make sure the data exists
        if (data == null)
        {
            return;
        }

        // Activate the appropriate power up based on its type
        switch (data.powerUpType)
        {
            case PowerUpType.Speed:
                Speed(data);
                break;
            case PowerUpType.Invincibility:
                Invincibility(data);
                break;
            case PowerUpType.Damage:
                Damage(data);
                break;
        }
    }

    private void Speed(PowerUpData data)
    {
        // If the speed power up is already active, don't reset the original speed
        if (!isSpeedActive)
        {
            originalSpeed = playerMovement.moveSpeed;
        }

        // Increase the players speed by the multiplier
        playerMovement.moveSpeed = originalSpeed * data.speedMultiplier;

        // Set the speed timer
        speedTimer = data.durationOfPowerUp;
        // Mark the speed power up as active
        isSpeedActive = true;
        Debug.Log("Speed power-up activated for " + data.durationOfPowerUp + " seconds!");
    }

    private void Invincibility(PowerUpData data)
    {
        if (localHealth != null)
        {
            // Activate invincibility for the local player and set the timer
            localHealth.Invincibility(data.durationOfPowerUp);
        }
            
        if (multiplayerHealth != null)
        {
            // Activate invincibility for the multiplayer player and set the timer
            multiplayerHealth.Invincibility(data.durationOfPowerUp);
        }
            
        Debug.Log("Invincibility power-up activated for " + data.durationOfPowerUp + " seconds!");
    }

    private void Damage(PowerUpData data)
    {
        // If the damage power up is already active, don't reset the original damage values
        if (!isDamageActive)
        {
            originalMeleeDamage = playerMelee.meleeDamage;
            originalShootDamage = bullet.bulletDamage;
        }

        // Increase the players melee and bullet damage by the damage increase value
        playerMelee.meleeDamage = originalMeleeDamage + data.damageIncrease;
        bullet.bulletDamage = originalShootDamage + data.damageIncrease;

        // Set the damage timer
        damageTimer = data.durationOfPowerUp;
        // Mark the damage power up as active
        isDamageActive = true;
        Debug.Log("Damage power-up activated for " + data.durationOfPowerUp + " seconds!");
    }
}