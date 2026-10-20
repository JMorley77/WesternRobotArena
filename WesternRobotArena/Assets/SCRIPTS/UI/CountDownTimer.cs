using TMPro;
using Unity.Netcode;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TMP_Text countdownText;

    private void Update()
    {
        if (gameManager == null || countdownText == null)
            return;

        if (gameManager.State.Value == GameManager.GameState.Starting)
        {
            countdownText.gameObject.SetActive(true);

            countdownText.text = Mathf.CeilToInt(gameManager.StartTimeRemaining.Value).ToString();
        }
        else
        {
            countdownText.gameObject.SetActive(false);
        }
    }
}