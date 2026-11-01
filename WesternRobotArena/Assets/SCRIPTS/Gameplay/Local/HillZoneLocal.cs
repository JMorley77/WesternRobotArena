using System.Collections.Generic;
using UnityEngine;

public class HillZoneLocal : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private ScoreManagerLocal scoreManager;
    [SerializeField] private LocalGameManager gameManager;

    // Track players inside the hill
    private HashSet<Collider> redPlayersInZone = new HashSet<Collider>();
    private HashSet<Collider> bluePlayersInZone = new HashSet<Collider>();

    private float scoreTimer = 0f;

    private void Update()
    {
        if (scoreManager == null || gameManager == null)
            return;

        if (scoreManager.GameEnded)
            return;

        if (gameManager.State != LocalGameManager.GameState.InGame)
            return;

        scoreTimer += Time.deltaTime;

        if (scoreTimer >= 1f)
        {
            scoreTimer -= 1f;
            AreaCheck();
        }
    }

    #region Trigger Detection

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TeamRed"))
        {
            Debug.Log("Team Red is in the hill zone");
            redPlayersInZone.Add(other);
        }
        else if (other.CompareTag("TeamBlue"))
        {
            Debug.Log("Team Blue is in the hill zone");
            bluePlayersInZone.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TeamRed"))
        {
            Debug.Log("Team Red has exited the hill zone");
            redPlayersInZone.Remove(other);
        }
        else if (other.CompareTag("TeamBlue"))
        {
            Debug.Log("Team Blue has exited the hill zone");
            bluePlayersInZone.Remove(other);
        }
    }

    #endregion

    #region Hill Control

    private void AreaCheck()
    {
        bool redTeam = redPlayersInZone.Count > 0;
        bool blueTeam = bluePlayersInZone.Count > 0;

        if (redTeam && blueTeam)
        {
            Debug.Log("Both teams are contesting the hill!");
        }
        else if (redTeam)
        {
            Debug.Log("Team Red is controlling the hill!");

            scoreManager.RedPointIncrease();
        }
        else if (blueTeam)
        {
            Debug.Log("Team Blue is controlling the hill!");

            scoreManager.BluePointIncrease();
        }
    }

    #endregion

    private void OnDisable()
    {
        redPlayersInZone.Clear();
        bluePlayersInZone.Clear();
    }
}
