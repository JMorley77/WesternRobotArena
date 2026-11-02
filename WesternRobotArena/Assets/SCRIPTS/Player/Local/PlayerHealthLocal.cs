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

    [Header("Invincibility")]
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;

            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
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

    public void Invincibility()
    {
        isInvincible = true;
        invincibilityTimer = 10;
        Debug.Log(gameObject.name + " is now invincible for 10 seconds!");
    }

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;
        currentHealth = 0f;

        Debug.Log(gameObject.name + " died!");

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
}
