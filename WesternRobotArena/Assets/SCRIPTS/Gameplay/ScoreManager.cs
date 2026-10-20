using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public GameManager gameManager;
    // Points for each team
    [SerializeField] private int teamRedPoints = 0;
    [SerializeField] private int teamBluePoints = 0;
    [SerializeField] TextMeshProUGUI redScoreText;
    [SerializeField] TextMeshProUGUI blueScoreText;

    // Game timer
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float maxPoints;
    private float remainingTime;

    public void Start()
    {
        remainingTime = gameManager.TimeRemaining.Value;
    }


    #region Point System
    public void RedPointIncrease()
    {
        teamRedPoints++;
        redScoreText.text = teamRedPoints.ToString();
        Debug.Log("Team Red scored! Current score: " + teamRedPoints);
    }

    public void BluePointIncrease()
    {
        teamBluePoints++;
        blueScoreText.text = teamBluePoints.ToString();
        Debug.Log("Team Blue scored! Current score: " + teamBluePoints);
    }

    void EndGame()
    {
        if (teamRedPoints == teamBluePoints)
        {
            Debug.Log("It's a tie!");
            // Add Draw Screen
        }
        else if (teamRedPoints >= teamBluePoints)
        {
            Debug.Log("Team Red wins!");
            // Add Red Win Screen
        }
        else
        {
            Debug.Log("Team Blue wins!");
            // Add Blue Win Screen
        }
    }

    #endregion

    #region Game Timer
    private void Update()
    {
        if (gameManager == null || timerText == null)
            return;

        float remainingTime = gameManager.TimeRemaining.Value;

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        timerText.text = string.Format(
            "{0:00}:{1:00}",
            minutes,
            seconds
        );

        if (gameManager.State.Value == GameManager.GameState.InGame)
        {
            if (remainingTime <= 0 ||
                teamRedPoints >= maxPoints ||
                teamBluePoints >= maxPoints)
            {
                EndGame();
            }
        }
    }
    #endregion
}
