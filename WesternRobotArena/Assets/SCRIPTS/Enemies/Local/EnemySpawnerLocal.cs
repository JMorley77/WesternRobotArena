using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerLocal : MonoBehaviour
{
    [Header("Enemy Types")]
    [SerializeField] private List<EnemyStats> enemyStats = new();

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Local Game Manager")]
    [SerializeField] private LocalGameManager gameManager;

    private float[] timers;

    private void Start()
    {
        timers = new float[enemyStats.Count];

        if (gameManager == null)
        {
            Debug.LogError("Local Game Manager is missing!");
            enabled = false;
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No enemy spawn points assigned!");
            enabled = false;
        }
    }

    private void Update()
    {
        if (gameManager.State != LocalGameManager.GameState.InGame)
            return;

        if (gameManager.GetPlayer(0) == null || gameManager.GetPlayer(1) == null)
            return;

        for (int i = 0; i < enemyStats.Count; i++)
        {
            if (enemyStats[i] == null)
                continue;

            timers[i] += Time.deltaTime;

            if (timers[i] >= enemyStats[i].spawnRate)
            {
                timers[i] = 0f;
                SpawnWave(enemyStats[i]);
            }
        }
    }

    public void SpawnWave(EnemyStats enemy)
    {
        if (enemy == null)
            return;

        for (int i = 0; i < 2; i++)
        {
            GameObject player = gameManager.GetPlayer(i);

            if (player != null)
            {
                SpawnEnemies(enemy, player.transform);
            }
        }
    }

    public void SpawnEnemies(EnemyStats enemy, Transform targetPlayer)
    {
        if (enemy == null || enemy.prefab == null)
            return;

        if (targetPlayer == null)
            return;

        if (spawnPoints == null || spawnPoints.Length == 0)
            return;

        for (int i = 0; i < enemy.numEnemiesPerWave; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject enemyObject = Instantiate(enemy.prefab,spawnPoint.position,spawnPoint.rotation);

            EnemyControllerLocal enemyController =enemyObject.GetComponent<EnemyControllerLocal>();

            if (enemyController != null)
            {
                enemyController.SetTarget(targetPlayer);
            }
            else
            {
                Debug.LogError("Enemy prefab is missing EnemyControllerLocal!");
            }
        }
    }
}