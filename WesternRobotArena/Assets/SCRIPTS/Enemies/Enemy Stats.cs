using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Western Robot / Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("ID")]
    public string AIName;

    [Header("Prefab")]
    public NetworkObject prefab;

    [Header("Stats")]
    public float maxHealth;
    public float moveSpeed;
    public float attackDamage;
    public float attackRange; 
    public float attackCooldown;
    public int rewardValue;

    [Header("Spawning")]
    [Tooltip("Time in seconds between each wave of enemies")]
    public float spawnRate;
    public int numEnemiesPerWave;
}
