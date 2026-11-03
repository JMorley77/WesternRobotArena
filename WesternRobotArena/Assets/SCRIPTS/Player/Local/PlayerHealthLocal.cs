using System;
using UnityEngine;

public class PlayerHealthLocal : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;
    private bool isDead = false;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => isDead;

    public event Action<PlayerHealthLocal> OnPlayerDied;

    [Header("Invincibility")]
    private bool isInvincible;
    private float invincibilityTimer;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isInvincible)
            return;

        invincibilityTimer -= Time.deltaTime;

        if (invincibilityTimer <= 0)
        {
            isInvincible = false;
            Debug.Log(gameObject.name + " is no longer invincible!");
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead || isInvincible || damage <= 0f)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0f);

        Debug.Log(gameObject.name + " Health: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;
        currentHealth = 0f;

        Debug.Log(gameObject.name + " died!");

        OnPlayerDied?.Invoke(this);

        // Disable the player movement and other components.


        Destroy(gameObject);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
    }
    public void Heal(float amount)
    {
        if (isDead || amount <= 0f)
            return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        Debug.Log(gameObject.name + " healed. Health: " + currentHealth);
    }

    public void Invincibility(float duration)
    {
        isInvincible = true;
        invincibilityTimer = duration;

        Debug.Log(gameObject.name + " is now invincible for " + duration + " seconds!");
    }
}
