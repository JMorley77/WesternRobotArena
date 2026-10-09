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

    public NetworkVariable<GameState> State =
        new NetworkVariable<GameState>(GameState.WaitingForPlayers);

    public NetworkVariable<float> TimeRemaining =
        new NetworkVariable<float>(0f);

    public override void OnNetworkSpawn()
    {

        if (!IsServer)
            return;

        State.Value = GameState.InGame;
        TimeRemaining.Value = gameDuration;

        Debug.Log("Match started!");
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
