using Unity.Netcode;
using UnityEngine;
public class GameManager : NetworkBehaviour
{
    public enum GameState
    {
        WaitingForPlayers,
        Starting,
        InGame,
        Finished
    }

    [Header("Game Settings")]
    [SerializeField] private float gameDuration = 480f; // 8 minutes
    [SerializeField] private float startCountdownDuration = 5f; // 5 seconds
    [SerializeField] private int requiredPlayers = 2;

    [Header("Player Settings")]
    [SerializeField] private GameObject SheriffPrefab;
    [SerializeField] private GameObject CowboyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private int nextSpawnIndex = 0;

    #region Network Variables
    public NetworkVariable<GameState> State =
        new NetworkVariable<GameState>(GameState.WaitingForPlayers);

    public NetworkVariable<float> TimeRemaining =
        new NetworkVariable<float>(0f);

    public NetworkVariable<float> StartTimeRemaining =
        new NetworkVariable<float>(0f);
    #endregion

    #region Initial Spawning
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;

        NetworkManager.OnClientConnectedCallback += OnClientConnected;

        foreach (ulong clientId in NetworkManager.ConnectedClientsIds)
        {
            SpawnPlayer(clientId);
        }


        State.Value = GameState.WaitingForPlayers;


        TimeRemaining.Value = gameDuration;
        StartTimeRemaining.Value = startCountdownDuration;

        Debug.Log("Waiting for both players...");

        CheckPlayers();
    }
    

    private void OnClientConnected(ulong clientId)
    {
        if (!IsServer)
            return;

        SpawnPlayer(clientId);
        CheckPlayers();
    }
    #endregion

    #region Checking Players
    private void CheckPlayers()
    {
        if (!IsServer)
            return;

        if (State.Value != GameState.WaitingForPlayers)
            return;

        Debug.Log($"Connected players: {NetworkManager.ConnectedClientsIds.Count}/{requiredPlayers}");

        if (NetworkManager.ConnectedClientsIds.Count >= requiredPlayers)
        {
            State.Value = GameState.Starting;
            StartTimeRemaining.Value = startCountdownDuration;

            Debug.Log("Host and client connected! Starting countdown!");
        }
    }
    #endregion
    
    #region Spawning Players
    private void SpawnPlayer(ulong clientId)
    {
        if (NetworkManager.ConnectedClients[clientId].PlayerObject != null)
            return;
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        GameObject selectedPrefab;

        if (clientId == 0)
        {
            selectedPrefab = SheriffPrefab;
        }
        else
        {
            selectedPrefab = CowboyPrefab;
        }


        Transform spawnPoint = spawnPoints[nextSpawnIndex % spawnPoints.Length];
        nextSpawnIndex++;

        GameObject player = Instantiate(
            selectedPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        player.tag = (clientId == 0) ? ("TeamRed") : ("TeamBlue") ;


        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

        Debug.Log($"Client {clientId} spawned at {spawnPoint.position}");
    }

    #endregion

    #region Game Loop
    private void Update()
    {
        if (!IsServer)
            return;

        // Starting countdown
        if (State.Value == GameState.Starting)
        {
            StartTimeRemaining.Value = Mathf.Max(0f,StartTimeRemaining.Value - Time.deltaTime
            );

            if (StartTimeRemaining.Value <= 0f)
            {
                State.Value = GameState.InGame;

                Debug.Log("Match started!");
            }
        }
        // Main game timer
        else if (State.Value == GameState.InGame)
        {
            TimeRemaining.Value = Mathf.Max(0f,TimeRemaining.Value - Time.deltaTime
            );

            if (TimeRemaining.Value <= 0f)
            {
                EndMatch();
            }
        }
    }


    private void EndMatch()
    {
        if (State.Value == GameState.Finished)
            return;

        State.Value = GameState.Finished;

        Debug.Log("Match ended!");
    }
    #endregion

    #region Despawning
    public override void OnNetworkDespawn()
    {
        if (IsServer && NetworkManager != null)
        {
            NetworkManager.OnClientConnectedCallback -= OnClientConnected;
        }
    }
    #endregion
}
