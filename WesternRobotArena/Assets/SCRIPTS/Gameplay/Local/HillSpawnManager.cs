using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class HillSpawnManager : MonoBehaviour
{
    [Header("Hill")]
    [SerializeField] private GameObject hillPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Timing")]
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private float activeTime = 30;

    [Header("Movement")]
    [SerializeField] private float undergroundPos = 5;

    private GameObject hill;
    private Transform currentSpawn;
    private float timer;
    private bool isRaising = true;
    private bool isLowering = false;
    private Vector3 targetPos;

    private void Start()
    {
        SpawnHill();
    }

    private void Update()
    {
        // raise or lower the hill
        if (isRaising || isLowering)
        {
            hill.transform.position = Vector3.MoveTowards(hill.transform.position, targetPos, moveSpeed * Time.deltaTime);

            // Check if the hill reached its destination
            if (hill.transform.position == targetPos)
            {
                isRaising = false;

                // Start the 30 second timer after raising
                if (!isLowering)
                {
                    timer = activeTime;
                }
            }

            // Check if the hill has finished lowering
            if (isLowering && hill.transform.position == targetPos)
            {
                Destroy(hill);
                SpawnHill();
            }
        }
        else
        {
            // Count down the 30 seconds
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                // Start lowering the hill
                isLowering = true;

                targetPos = currentSpawn.position -
                                 Vector3.up * undergroundPos;
            }
        }
    }

    void SpawnHill()
    {
        // Pick a random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        currentSpawn = spawnPoints[randomIndex];

        // Calculate underground position
        Vector3 undergroundPosition = currentSpawn.position -
                                      Vector3.up * undergroundPos;

        // Instantiate the hill underground
        hill = Instantiate(
            hillPrefab,
            undergroundPosition,
            currentSpawn.rotation
        );

        // Set the target to ground level
        targetPos = currentSpawn.position;

        // Start raising
        isRaising = true;
        isLowering = false;
    }
}