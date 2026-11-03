using System.Collections.Generic;
using UnityEngine;

public class HillZoneLocal : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private ScoreManagerLocal scoreManager;
    [SerializeField] private LocalGameManager gameManager;

    [Header("Zone Colours")]
    public GameObject blue;
    public GameObject red;
    public GameObject yellow;
    public GameObject white;

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
        PlayerHealthLocal player = other.GetComponent<PlayerHealthLocal>();

        if (player == null || player.IsDead)
        {
            return;
        }

        player.OnPlayerDied += OnPlayerDeath;

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
        PlayerHealthLocal player = other.GetComponent<PlayerHealthLocal>();

        if (player != null)
        {
            player.OnPlayerDied -= OnPlayerDeath;
        }

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
            yellow.SetActive(true);
            red.SetActive(false);
            blue.SetActive(false);
            white.SetActive(false);

        }
        else if (redTeam)
        {
            Debug.Log("Team Red is controlling the hill!");
            scoreManager.RedPointIncrease();
            yellow.SetActive(false);
            red.SetActive(true);
            blue.SetActive(false);
            white.SetActive(false);
        }
        else if (blueTeam)
        {
            Debug.Log("Team Blue is controlling the hill!");
            scoreManager.BluePointIncrease();
            yellow.SetActive(false);
            red.SetActive(false);
            blue.SetActive(true);
            white.SetActive(false);
        }
        else if (!blueTeam && !redTeam)
        {
            yellow.SetActive(false);
            red.SetActive(false);
            blue.SetActive(false);
            white.SetActive(true);
        }
    }

    #endregion

    #region Death

    private void OnPlayerDeath(PlayerHealthLocal player)
    {
        RemovePlayerFromZone(player);
    }

    private void RemovePlayerFromZone(PlayerHealthLocal player)
    {
        Collider playerCollider = player.GetComponent<Collider>();

        if (playerCollider == null)
        {
            return;
        }

        bluePlayersInZone.Remove(playerCollider);
        redPlayersInZone.Remove(playerCollider);
    }

    #endregion

    private void OnDisable()
    {
        redPlayersInZone.Clear();
        bluePlayersInZone.Clear();
    }
}
