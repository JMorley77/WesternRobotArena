using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using System.Linq;

public class EnemySpawner : NetworkBehaviour
{
    [Header("Enemy Types")]
    [SerializeField] private List<EnemyStats> enemyStats = new();

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    private float[] timers;

    private void Start()
    {
        timers = new float[enemyStats.Count];
    }
    private void Update()
    {
        if (!IsServer)
            return;

        for (int i = 0; i < enemyStats.Count; i++)
        {
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
        if (NetworkManager.Singleton.ConnectedClientsIds.Count < 1)
            return;

        foreach (ulong playerId in  NetworkManager.Singleton.ConnectedClientsIds)
        {
            SpawnEnemies(enemy, playerId);
        
        }
    }

    public void SpawnEnemies(EnemyStats enemy, ulong targetPlayerId)
    {
        for(int i = 0; i < enemy.numEnemiesPerWave; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];


            NetworkObject enemyObject = Instantiate(enemy.prefab, spawnPoint.position, spawnPoint.rotation);

            enemyObject.Spawn();

            EnemyController enemyController = enemyObject.GetComponent<EnemyController>();

            //enemyController.SetTarget(targetPlayerId);
        }
    }
}
