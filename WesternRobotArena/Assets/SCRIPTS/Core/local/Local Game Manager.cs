using UnityEngine;
using UnityEngine.InputSystem;

public class LocalGameManager : MonoBehaviour
{
    public enum GameState
    {
        WaitingForPlayers,
        Starting,
        InGame,
        Finished
    }

    [Header("Game Settings")]
    [SerializeField] private float gameDuration = 480f;
    [SerializeField] private float startCountdownDuration = 5f;
    [SerializeField] private int requiredPlayers = 2;

    [Header("Player Settings")]
    [SerializeField] private GameObject SheriffPrefab;
    [SerializeField] private GameObject CowboyPrefab;

    [SerializeField] private Transform[] spawnPoints;

    [Header("Camera Settings")]
    [SerializeField] private Camera mainCamera;

    [Header("Game State")]
    [SerializeField]
    private GameState state = GameState.WaitingForPlayers;

    private float timeRemaining;
    private float startTimeRemaining;

    private GameObject[] spawnedPlayers;

    public GameState State => state;
    public float TimeRemaining => timeRemaining;
    public float StartTimeRemaining => startTimeRemaining;

    private void Start()
    {
        spawnedPlayers = new GameObject[requiredPlayers];

        timeRemaining = gameDuration;
        startTimeRemaining = startCountdownDuration;

        SpawnLocalPlayers();
    }

    private void SpawnLocalPlayers()
    {
        if (spawnPoints == null || spawnPoints.Length < requiredPlayers)
        {
            Debug.LogError("Not enough spawn points assigned!");
            return;
        }

        if (mainCamera == null)
        {
            Debug.LogError("Main camera has not been assigned!");
            return;
        }

        for (int i = 0; i < requiredPlayers; i++)
        {
            GameObject selectedPrefab = (i == 0)? SheriffPrefab: CowboyPrefab;

            if (selectedPrefab == null)
            {
                Debug.LogError($"Player {i + 1} prefab is missing!");
                return;
            }

            Transform spawnPoint = spawnPoints[i];

            GameObject player = Instantiate(selectedPrefab,spawnPoint.position,spawnPoint.rotation);

            spawnedPlayers[i] = player;

            player.tag = (i == 0) ? "TeamRed" : "TeamBlue";

            Debug.Log($"Local Player {i + 1} spawned!");
        }

        state = GameState.Starting;
        startTimeRemaining = startCountdownDuration;
    }

    private void Update()
    {
        if (state == GameState.Starting)
        {
            startTimeRemaining -= Time.deltaTime;

            if (startTimeRemaining <= 0f)
            {
                startTimeRemaining = 0f;
                state = GameState.InGame;

                Debug.Log("Local match started!");
            }
        }
        else if (state == GameState.InGame)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                EndMatch();
            }
        }
    }

    public void EndMatch()
    {
        if (state == GameState.Finished)
            return;

        state = GameState.Finished;

        Debug.Log("Local match ended!");
    }

    public GameObject GetPlayer(int playerIndex)
    {
        if (spawnedPlayers == null ||playerIndex < 0 ||playerIndex >= spawnedPlayers.Length)
        {
            return null;
        }

        return spawnedPlayers[playerIndex];
    }
}