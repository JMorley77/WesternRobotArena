using Unity.Netcode;
using UnityEngine;

public class EnemyHealth : NetworkBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private EnemyStats enemyStats;

    public NetworkVariable<float> CurrentHealth = new NetworkVariable<float>();

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;
        CurrentHealth.Value = enemyStats.maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!IsServer)
            return;

        CurrentHealth.Value -= damage;

        Debug.Log("Enemy Health: " + CurrentHealth.Value);

        if (CurrentHealth.Value <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle enemy death logic here
        Debug.Log(enemyStats.AIName + " Enemy Died");

        NetworkObject.Despawn();
    }
}
