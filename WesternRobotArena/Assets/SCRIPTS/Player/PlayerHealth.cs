using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    public NetworkVariable<float> CurrentHealth =
        new NetworkVariable<float>();

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;

        CurrentHealth.Value = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!IsServer)
            return;

        CurrentHealth.Value -= damage;

        Debug.Log("Player Health: " + CurrentHealth.Value);

        if (CurrentHealth.Value <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        CurrentHealth.Value = 0f;

        Debug.Log("Player died");
    }
}