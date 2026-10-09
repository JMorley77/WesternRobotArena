using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkBehaviour
{
    [Header("Lobby Settings")]
    [SerializeField] private int requiredPlayers = 2;
    [SerializeField] private string mainGameScene = "Main";

    [Header("Lobby UI")]
    [SerializeField] private TMP_Text playerListText;


    private bool sceneLoading;

    private void Update()
    {
        NetworkManager nm = NetworkManager.Singleton;

        if (nm == null || !nm.IsListening)
        {
            if (playerListText != null)
                playerListText.text = "Not connected";

            return;
        }

        int connectedPlayers = nm.ConnectedClientsIds.Count;

        if (playerListText != null)
        {
            playerListText.text =
                $"Players connected: {connectedPlayers}/{requiredPlayers}";
        }

        if (!nm.IsServer)
            return;

        if (sceneLoading)
            return;

        if (connectedPlayers < requiredPlayers)
            return;

        sceneLoading = true;

        Debug.Log("Required players connected. Loading Main scene.");

        var status = nm.SceneManager.LoadScene(
            mainGameScene,
            LoadSceneMode.Single
        );
    }
}