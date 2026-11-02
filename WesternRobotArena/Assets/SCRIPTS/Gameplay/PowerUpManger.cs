using UnityEngine;

public class PowerUpManger : MonoBehaviour
{
    public float timeDuration = 10;

    [Header("Speed")]
    //public CharacterMover playerMovement;
    public JackTestDontUse playerMovement;
    public float speedMultiplier = 2;

    private float originalSpeed;
    private float timer;
    private bool isSpeedActive;

    [Header("Invincibility")]
    public PlayerHealthLocal localHealth;
    public PlayerHealth multiplayerHealth;

    void Update()
    {
        if (!isSpeedActive)
        {
            return;
        }
        
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            playerMovement.speed = originalSpeed;
            isSpeedActive = false;
        }
    }

    public void Speed()
    {
        if (!isSpeedActive)
        {
            originalSpeed = playerMovement.speed;
            playerMovement.speed *= speedMultiplier;
            isSpeedActive = true;
        }

        timer = timeDuration;
    }
}