using UnityEngine;

public enum PowerUpType
{
    Speed,
    Invincibility
}

[CreateAssetMenu(fileName = "NewPowerUp", menuName = "PowerUps/PowerUp")]
public class PowerUpData : ScriptableObject
{
    public string powerUpName;
    public PowerUpType powerUpType;
    public float duration = 10;
    public float multiplier = 2;
}
