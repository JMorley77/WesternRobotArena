using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    public NetworkVariable<float> CurrentHealth = new NetworkVariable<float>();

    [Header("Invincibility")]
    private bool isInvincible;
    private float invincibilityTimer;

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

    public void Invincibility(float duration)
    {
        if (!IsServer)
            return;

        isInvincible = true;
        invincibilityTimer = duration;

        Debug.Log(gameObject.name + " is now invincible for " + duration + " seconds!");
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