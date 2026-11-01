using TMPro;
using UnityEngine;
using Unity.Netcode;

public class CountDownTimer : MonoBehaviour
{
    [Header("Countdown UI")]
    [SerializeField] private TMP_Text countdownText;

    [Header("Game Managers")]
    [SerializeField] private LocalGameManager localGameManager;
    [SerializeField] private GameManager multiplayerGameManager;

    private void Update()
    {
        if (countdownText == null)
            return;

        if (localGameManager != null)
        {
            if (localGameManager.State == LocalGameManager.GameState.Starting)
            {
                ShowCountdown(localGameManager.StartTimeRemaining);
            }
            else
            {
                countdownText.gameObject.SetActive(false);
            }

            return;
        }

        if (multiplayerGameManager != null)
        {
            if (multiplayerGameManager.State.Value == GameManager.GameState.Starting)
            {
                ShowCountdown(multiplayerGameManager.StartTimeRemaining.Value);
            }
            else
            {
                countdownText.gameObject.SetActive(false);
            }
        }
    }

    private void ShowCountdown(float timeRemaining)
    {
        countdownText.gameObject.SetActive(true);
        countdownText.text = Mathf.CeilToInt(timeRemaining).ToString();
    }
}