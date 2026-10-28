using UnityEngine;
using Unity.Netcode;

public class FallRespawn : MonoBehaviour
{
    [SerializeField] private Transform outlawSpawnPoint;
    [SerializeField] private Transform sheriffSpawnPoint;


    public void OnCollisionEnter(Collision collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        Debug.Log("entered");
        playerHealth.Die();
        Respawn(collision.gameObject);

    }

    private void Respawn(GameObject player)
    {
        Transform spawnPoint = null;

        if(player.CompareTag("TeamBlue"))
        {
            spawnPoint = outlawSpawnPoint;
        }
        if (player.CompareTag("TeamRed"))
        {
            spawnPoint = sheriffSpawnPoint;
        }

        player.transform.SetPositionAndRotation(spawnPoint.position,spawnPoint.rotation);


    }
}
