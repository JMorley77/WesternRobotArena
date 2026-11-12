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

        // Check if it's time to spawn a new power-up
        if (timer >= spawnInterval)
        {
            SpawnPowerUp();
            timer = 0;
        }
    }

    void SpawnPowerUp()
    {
        // Check if there are any power-up prefabs and spawn points available
        if (powerUpPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            return;
        }

        // Check if the maximum number of power ups has been reached in the scene
        GameObject[] existing = GameObject.FindGameObjectsWithTag("PowerUp");

        // If the maximum number of power ups has been reached, don't spawn another one
        if (existing.Length >= maxPowerUps)
        {
            return;
        }

        // Randomly select a power-up prefab and a spawn point
        int prefabIndex = Random.Range(0, powerUpPrefabs.Length);
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        
        Transform spawnPoint = spawnPoints[spawnIndex];

        // Check if theres already a power up at the spawn point
        foreach (GameObject powerUp in existing)
        {
            
            if (Vector3.Distance(powerUp.transform.position, spawnPoint.position) < 1f)
            {
                // If theres already a power up at the spawn point, don't spawn another one
                return;
            }
        }

        // Spawn a new power up at a spawn point
        Instantiate(powerUpPrefabs[prefabIndex], spawnPoints[spawnIndex].position, Quaternion.identity);
    }
}
