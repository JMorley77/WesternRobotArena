using UnityEngine;
using Unity.Netcode;
using UnityEditor.Networking.PlayerConnection;

public class ServerManager : NetworkBehaviour
{

    public void HostGame()
    {
       NetworkManager.Singleton.StartHost();
    }

    public void JoinGame()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void Disconnect()
    {
        NetworkManager.Singleton.Shutdown();
        // go to main menu scene
    }
}
