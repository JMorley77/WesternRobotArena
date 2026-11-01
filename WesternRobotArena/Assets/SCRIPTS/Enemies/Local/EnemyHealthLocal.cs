using UnityEngine;

public class EnemyHealthLocal : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private EnemyStats enemyStats;

    private float currentHealth;
    private bool isDead;

    public float CurrentHealth => currentHealth;
    public bool IsDead => isDead;

    private void Start()
    {
        if (enemyStats == null)
        {
            Debug.LogError("Enemy Stats is missing!");
            enabled = false;
            return;
        }

        currentHealth = enemyStats.maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead || damage <= 0f)
            return;

        currentHealth = Mathf.Max(currentHealth - damage, 0f);

        Debug.Log("Enemy Health: " + currentHealth);

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log(enemyStats.AIName + " Enemy Died");

        Destroy(gameObject);
    }
}
