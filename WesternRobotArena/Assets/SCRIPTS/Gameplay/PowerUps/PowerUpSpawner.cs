using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("PowerUp Prefabs")]
    public GameObject[] powerUpPrefabs;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Spawn Settings")]
    public float spawnInterval = 10;
    public int maxPowerUps = 5;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnPowerUp();
            timer = 0;
        }
    }

    void SpawnPowerUp()
    {
        if (powerUpPrefabs.Length == 0 || spawnPoints.Length == 0)
            return;

        GameObject[] existing = GameObject.FindGameObjectsWithTag("PowerUp");

        if (existing.Length >= maxPowerUps)
            return;

        int prefabIndex = Random.Range(0, powerUpPrefabs.Length);
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        Instantiate(powerUpPrefabs[prefabIndex], spawnPoints[spawnIndex].position, Quaternion.identity);
    }
}
