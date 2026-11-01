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

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead || damage <= 0f)
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
