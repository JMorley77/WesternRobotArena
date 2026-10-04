using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Points for each team
    [SerializeField] private int teamRedPoints = 0;
    [SerializeField] private int teamBluePoints = 0;
    [SerializeField] TextMeshProUGUI redScoreText;
    [SerializeField] TextMeshProUGUI blueScoreText;

    // Game timer
    [SerializeField] TextMeshProUGUI timerText;
    // 480 seconds = 8 minutes
    [SerializeField] float remainingTime;

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
        if (remainingTime <= 0 && teamRedPoints >= teamBluePoints)
        {
            Debug.Log("Team Red wins!");
            // Add Red Win Screen
        }
        else if (remainingTime <= 0 && teamBluePoints >= teamRedPoints)
        {
            Debug.Log("Team Blue wins!");
            // Add Blue Win Screen
        }
        else if (remainingTime <= 0 && teamRedPoints == teamBluePoints)
        {
            Debug.Log("It's a tie!");
            // Add Draw Screen
        }
    }

    #endregion

    #region Game Timer
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime <= 0)
        {
            remainingTime = 0;
            EndGame();
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    #endregion
}
