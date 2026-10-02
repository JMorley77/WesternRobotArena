using UnityEngine;
using UnityEngine.InputSystem;

public class LocalMultiplayerManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;

    public void OnPlayerJoined(PlayerInput player)
    {
        int playerIndex = player.playerIndex;

        if (playerIndex < spawnPoints.Length)
        {
            player.transform.position = spawnPoints[playerIndex].position;
        }
    }
}