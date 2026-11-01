using UnityEngine;
using Unity.Netcode;

public class FallRespawn : MonoBehaviour
{
    [Header("Respawn Points")]
    [SerializeField] private Transform outlawSpawnPoint;
    [SerializeField] private Transform sheriffSpawnPoint;

    private void OnCollisionEnter(Collision collision)
    {
        // Check for the multiplayer health component 
        PlayerHealth onlineHealth =collision.gameObject.GetComponentInParent<PlayerHealth>();

        if (onlineHealth != null)
        {
            // Only the server handles multiplayer respawning
            if (!onlineHealth.IsSpawned || !onlineHealth.IsServer)
                return;

            Respawn(onlineHealth.gameObject);
            onlineHealth.ResetHealth();
            return;
        }

        //check for the local health component
        PlayerHealthLocal localHealth =collision.gameObject.GetComponentInParent<PlayerHealthLocal>();

        if (localHealth != null)
        {
            Respawn(localHealth.gameObject);
            localHealth.ResetHealth();
        }
    }

    private void Respawn(GameObject player)
    {
        Transform spawnPoint = null;

        if (player.CompareTag("TeamBlue"))
        {
            spawnPoint = outlawSpawnPoint;
        }
        else if (player.CompareTag("TeamRed"))
        {
            spawnPoint = sheriffSpawnPoint;
        }

        if (spawnPoint == null)
        {
            Debug.LogWarning("No respawn point assigned for " + player.name);
            return;
        }

        player.transform.SetPositionAndRotation(spawnPoint.position,spawnPoint.rotation);

        // Stop any remaining movement after respawning.
        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log(player.name + " respawned!");
    }
}
