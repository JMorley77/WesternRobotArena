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

    private void Respawn(PlayerHealthLocal h)
    {
        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

        if (cc != null) cc.enabled = true;

        health.ResetHealth();
    }
}
