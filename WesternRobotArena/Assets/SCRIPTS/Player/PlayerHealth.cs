using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    public NetworkVariable<float> CurrentHealth = new NetworkVariable<float>();

    [Header("Invincibility")]
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;

    public void Update()
    {
        if (!IsServer || !isInvincible)
        {
            return;
        }

        invincibilityTimer -= Time.deltaTime;

        if (invincibilityTimer <= 0)
        {
            isInvincible = false;
            Debug.Log(gameObject.name + " is no longer invincible!");
        }
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;

        CurrentHealth.Value = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!IsServer || isInvincible || damage <= 0f)
            return;

        CurrentHealth.Value -= damage;

        Debug.Log("Player Health: " + CurrentHealth.Value);

        if (CurrentHealth.Value <= 0f)
        {
            Die();
        }
    }

    public void Invincibility()
    {
        if (!IsServer)
            return;
        isInvincible = true;
        invincibilityTimer = 10;
        Debug.Log(gameObject.name + " is now invincible for 10 seconds!");
    }

    public void ResetHealth()
    {
        if (!IsServer)
            return;

        CurrentHealth.Value = maxHealth;
    }
    public void Die()
    {
        if (!IsServer)
            return;

        CurrentHealth.Value = 0f;

        Debug.Log("Player died");

        NetworkObject.Despawn();
    }
}