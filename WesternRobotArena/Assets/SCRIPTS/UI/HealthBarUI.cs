using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    private PlayerHealthLocal playerHealth;

    public void SetPlayer(PlayerHealthLocal health)
    {
        playerHealth = health;
        UpdateHealthBar();
    }

    private void Update()
    {
        if (playerHealth != null)
        {
            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        float healthPercentage = playerHealth.CurrentHealth / playerHealth.MaxHealth;

        healthFill.fillAmount = Mathf.Clamp01(healthPercentage);
    }
}
