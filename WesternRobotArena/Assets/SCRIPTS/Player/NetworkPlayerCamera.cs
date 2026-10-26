using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerCamera : NetworkBehaviour
{
    private ArenaCamera arenaCamera;

    public override void OnNetworkSpawn()
    {
        arenaCamera = Camera.main.GetComponent<ArenaCamera>();

        if (arenaCamera != null)
            arenaCamera.AddPlayer(transform);
    }

    public override void OnNetworkDespawn()
    {
        if (arenaCamera != null)
            arenaCamera.RemovePlayer(transform);
    }
}