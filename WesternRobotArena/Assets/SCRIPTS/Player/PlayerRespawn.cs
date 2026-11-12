using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private PlayerHealthLocal health;
    private Transform spawnPoint;

    private void Awake()
    {
        health = GetComponent<PlayerHealthLocal>();
    }

    private void OnEnable()
    {
        health.OnPlayerDied += Respawn;
    }

    private void OnDisable()
    {
        health.OnPlayerDied -= Respawn;
    }

    public void SetSpawnPoint(Transform point)
    {
        spawnPoint = point;
    }

    public void Respawn(PlayerHealthLocal h)
    {


        transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

        health.ResetHealth();
    }
}
