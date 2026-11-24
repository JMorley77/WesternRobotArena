using UnityEngine;

public class EnemyHealthLocal : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private EnemyStats enemyStats;

    [Header("Game Manager")]
    [SerializeField] private LocalGameManager gameManager;
    [SerializeField] private ScoreManagerLocal  scoreManager;

    [Header("Audio")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;
    private AudioSource audioSource;
    private EnemyControllerLocal enemyController;
    public float currentHealth;
    private bool isDead;
    private int targetPlayer;
    private float maxHealth;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => isDead;

    private void Start()
    {
        if (enemyStats == null)
        {
            Debug.LogError("Enemy Stats is missing!");
            enabled = false;
            return;
        }
        maxHealth = enemyStats.maxHealth;
        gameManager = FindFirstObjectByType<LocalGameManager>();
        scoreManager = FindFirstObjectByType<ScoreManagerLocal>();
        audioSource = GetComponent<AudioSource>();
        enemyController = GetComponent<EnemyControllerLocal>();
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

        // Play hit sound
        audioSource.PlayOneShot(hitSound);

        Debug.Log("Enemy Health: " + currentHealth);

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        if (isDead)
            return;

        // Play death sound
        audioSource.PlayOneShot(deathSound);

        isDead = true;

        // Stop the enemys movement
        enemyController.Die();

        Debug.Log(enemyStats.AIName + " Enemy Died");

        if (targetPlayer == 0)
            scoreManager.BluePointIncrease(enemyStats.rewardValue);
        else
            scoreManager.RedPointIncrease(enemyStats.rewardValue);

        Destroy(gameObject, deathSound.length);
    }
}
