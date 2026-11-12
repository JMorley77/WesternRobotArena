using UnityEngine;

// Enum for different types of power-ups
public enum PowerUpType
{
    Speed,
    Invincibility,
    Damage
}

[CreateAssetMenu(fileName = "NewPowerUp", menuName = "PowerUps/PowerUp")]
public class PowerUpData : ScriptableObject
{
    // public string powerUpName;
    public PowerUpType powerUpType;
    public float durationOfPowerUp = 10;
    public float speedMultiplier = 2;
    public float damageIncrease = 15;
}
