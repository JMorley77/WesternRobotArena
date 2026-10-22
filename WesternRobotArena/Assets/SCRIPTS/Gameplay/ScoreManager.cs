using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public GameManager gameManager;
    // Points for each team
    public NetworkVariable<int> RedPoints = new NetworkVariable<int>(0);
    public NetworkVariable<int> BluePoints = new NetworkVariable<int>(0);

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
    public override void OnNetworkSpawn()
    {
        RedPoints.OnValueChanged += (o, n) => redScoreText.text = "RED: " + n;
        BluePoints.OnValueChanged += (o, n) => blueScoreText.text = "BLUE: " + n;
        redScoreText.text = "RED: " + RedPoints.Value;
        blueScoreText.text = "BLUE: " + BluePoints.Value;
    }

    #region Point System
    public void RedPointIncrease()
    {
        if (!IsServer) return;
        RedPoints.Value++;
    }

    public void BluePointIncrease()
    {
        if (!IsServer) return;
        BluePoints.Value++;
    }

    void EndGame()
    {
        if (RedPoints.Value == BluePoints.Value)
        {
            Debug.Log("It's a tie!");
            // Add Draw Screen
        }
        else if (RedPoints.Value >= BluePoints.Value)
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
                RedPoints.Value >= maxPoints ||
                BluePoints.Value >= maxPoints)
            {
                EndGame();
            }
        }
    }
    #endregion
}
