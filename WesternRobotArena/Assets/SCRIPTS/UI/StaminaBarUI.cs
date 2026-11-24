using UnityEngine;
using UnityEngine.UI;

public class StaminaBarUI : MonoBehaviour
{
    [SerializeField] private Image staminaFill;
    private LocalCharacterMover player;

    public void SetPlayer(LocalCharacterMover character)
    {
        player = character;
        UpdateHealthBar();
    }

    private void Update()
    {
        if (player != null)
        {
            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        float staminaPercentage = player.CurrentStamina / player.MaxStamina;

        staminaFill.fillAmount = Mathf.Clamp01(staminaPercentage);
    }
}
