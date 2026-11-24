using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    private EnemyHealthLocal enemyHealth;
    //private EnemyStats enemyStats;

    private void Awake()
    {
        enemyHealth = GetComponentInParent<EnemyHealthLocal>();
    }

    private void Start()
    {
        UpdateHealthBar();
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float healthPercentage = enemyHealth.CurrentHealth / enemyHealth.MaxHealth;

        healthFill.fillAmount = Mathf.Clamp01(healthPercentage);
    }
}