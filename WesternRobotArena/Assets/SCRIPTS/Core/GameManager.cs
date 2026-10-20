using Unity.Netcode;
using UnityEngine;
public class GameManager : NetworkBehaviour
{
    public enum GameState
    {
        WaitingForPlayers,
        InGame,
        Finished
    }

    [Header("Game Settings")]
    [SerializeField] private float gameDuration = 480f; // 8 minutes

    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;

    public NetworkVariable<GameState> State =
        new NetworkVariable<GameState>(GameState.WaitingForPlayers);

    public NetworkVariable<float> TimeRemaining =
        new NetworkVariable<float>(0f);

    private int nextSpawnIndex = 0;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;

        NetworkManager.OnClientConnectedCallback += OnClientConnected;

        foreach (ulong clientId in NetworkManager.ConnectedClientsIds)
        {
            SpawnPlayer(clientId);
        }

        State.Value = GameState.InGame;
        TimeRemaining.Value = gameDuration;

        Debug.Log("Match started!");
    }

    private void OnClientConnected(ulong clientId)
    {
        if (!IsServer)
            return;

        SpawnPlayer(clientId);
    }

    private void SpawnPlayer(ulong clientId)
    {
        if (NetworkManager.ConnectedClients[clientId].PlayerObject != null)
            return;
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        Transform spawnPoint = spawnPoints[nextSpawnIndex % spawnPoints.Length];
        nextSpawnIndex++;

        GameObject player = Instantiate(
            playerPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        player.GetComponent<NetworkObject>()
              .SpawnAsPlayerObject(clientId);

        Debug.Log($"Client {clientId} spawned at {spawnPoint.position}");
    }


    private void Update()
    {
        if (!IsServer)
            return;

        if (State.Value != GameState.InGame)
            return;

        TimeRemaining.Value = Mathf.Max(
            0f,
            TimeRemaining.Value - Time.deltaTime
        );

        if (TimeRemaining.Value <= 0f)
        {
            EndMatch();
        }
    }

    private void EndMatch()
    {
        if (State.Value == GameState.Finished)
            return;

        State.Value = GameState.Finished;

        Debug.Log("Match ended!");
    }
}
