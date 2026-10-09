using Unity.Netcode;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{

    public enum GameState
    {
        WaitingForPlayers,
        InGame,
        Finished
    }

    [Header("Game Settings")]
    [SerializeField] private int requiredPlayers = 2;
    [SerializeField] private float gameDuration = 480; // 8 minutes
    [SerializeField] private string mainGameScene = "Main";

    public NetworkVariable<GameState> State = new NetworkVariable<GameState>(GameState.WaitingForPlayers);

    public NetworkVariable<float> TimeRemaining = new NetworkVariable<float>(0f);

    private bool sceneLoading;
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;
        State.Value = GameState.WaitingForPlayers;
        TimeRemaining.Value = gameDuration;
    }

    private void Update()
    {
        if(!IsServer)
            return;

        if(State.Value == GameState.WaitingForPlayers)
        {
            if(NetworkManager.Singleton.ConnectedClientsIds.Count>=requiredPlayers)
            {
                StartMatch();
            }
        }

        if(State.Value !=GameState.InGame)
            return;

        TimeRemaining.Value = Mathf.Max(0f,TimeRemaining.Value - Time.deltaTime);

        if(TimeRemaining.Value <= 0f)
        {
            EndMatch();
        }
    }

    private void StartMatch()
    {
        if (sceneLoading) return;

        sceneLoading = true;
        State.Value = GameState.InGame;
        TimeRemaining.Value = gameDuration;

        Debug.Log("Players connected. Loading game scene.");

        NetworkManager.Singleton.SceneManager.LoadScene(
            mainGameScene,
            LoadSceneMode.Single
        );
    }


    private void EndMatch()
    {
        if(!IsServer)
            return;
        if(State.Value == GameState.Finished)
            return;

        State.Value = GameState.Finished;

        Debug.Log("Match Ended");
    }

}
