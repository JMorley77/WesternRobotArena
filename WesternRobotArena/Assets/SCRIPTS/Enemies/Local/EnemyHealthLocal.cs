using UnityEngine;

public class EnemyHealthLocal : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private EnemyStats enemyStats;

    [Header("Game Manager")]
    [SerializeField] private LocalGameManager gameManager;
    [SerializeField] private ScoreManagerLocal  scoreManager;
    private float currentHealth;
    private bool isDead;
    private int targetPlayer;
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

        gameManager = FindFirstObjectByType<LocalGameManager>();
        scoreManager = FindFirstObjectByType<ScoreManagerLocal>();

        currentHealth = enemyStats.maxHealth;
    }
    public void SetTargetPlayer(int player)
    {
        targetPlayer = player;
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

        if (targetPlayer == 0)
            scoreManager.RedPointIncrease(enemyStats.rewardValue);
        else
            scoreManager.BluePointIncrease(enemyStats.rewardValue);

        Destroy(gameObject);
    }
}
