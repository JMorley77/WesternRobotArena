using UnityEngine;
using TMPro;
using Unity.Netcode;

public class LobbyManager : NetworkBehaviour
{
    [SerializeField] private TMP_Text playerListText;

    private void Update()
    {
        var nm = NetworkManager.Singleton;

        if (nm == null || !nm.IsListening)
        {
            playerListText.text = "Not connected";
            return;
        }

        playerListText.text =
            "Players connected: " + nm.ConnectedClientsIds.Count;
    }
}