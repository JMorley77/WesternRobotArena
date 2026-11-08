using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManagerLocal : MonoBehaviour
{
    [Header("Game Manager")]
    [SerializeField] private LocalGameManager gameManager;

    [Header("Team Scores")]
    [SerializeField] private int redPoints = 0;
    [SerializeField] private int bluePoints = 0;

    [SerializeField] private TextMeshProUGUI redScoreText;
    [SerializeField] private TextMeshProUGUI blueScoreText;

    [Header("Game Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private int maxPoints = 10;

    [Header("Win Scenes")]
    [SerializeField] private string redWinScene;
    [SerializeField] private string blueWinScene;

    private bool gameEnded = false;

    public int RedPoints => redPoints;
    public int BluePoints => bluePoints;
    public bool GameEnded => gameEnded;

    private void Start()
    {
        if (gameManager == null)
        {
            Debug.LogError("LocalGameManager is missing!");
            return;
        }

        UpdateScoreUI();
        UpdateTimerUI();
    }

    #region Point System

    public void RedPointIncrease(int points)
    {
        if (gameEnded)
            return;

        redPoints += points;

        UpdateScoreUI();
        CheckEndGame();
    }

    public void BluePointIncrease(int points)
    {
        if (gameEnded)
            return;

        bluePoints += points;

        UpdateScoreUI();
        CheckEndGame();
    }

    private void UpdateScoreUI()
    {
        if (redScoreText != null)
            redScoreText.text = "RED: " + redPoints;

        if (blueScoreText != null)
            blueScoreText.text = "BLUE: " + bluePoints;
    }

    #endregion

    #region Game Timer

    private void Update()
    {
        if (gameManager == null || gameEnded)
            return;

        UpdateTimerUI();

        if (gameManager.State == LocalGameManager.GameState.Finished)
        {
            EndGame();
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText == null || gameManager == null)
            return;

        float remainingTime = gameManager.TimeRemaining;

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);

        timerText.text = string.Format("{0:00}:{1:00}",minutes,seconds);
    }

    #endregion

    #region End Game

    private void CheckEndGame()
    {
        if (redPoints >= maxPoints || bluePoints >= maxPoints)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        if (redPoints == bluePoints)
        {
            Debug.Log("It's a tie!");
            // Add Draw Screen
        }
        else if (redPoints > bluePoints)
        {
            Debug.Log("Team Red wins!");
            SceneManager.LoadScene(redWinScene);
        }
        else
        {
            Debug.Log("Team Blue wins!");
            SceneManager.LoadScene(blueWinScene);
        }

        if (gameManager != null)
        {
            gameManager.EndMatch();
        }
    }

    #endregion
}