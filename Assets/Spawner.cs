using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private int instanceID;  // Instance ID input
    [SerializeField] private string localIdentifierInFile;  // Local identifier in file
    [SerializeField] private bool spawnMode = true;  // Spawn mode (true = regular, false = custom/random)
    [SerializeField] private float delayBtwSpawns = 2f;  // Delay between each spawn
    [SerializeField] private float minRandomDelay = 1f;  // Minimum random delay
    [SerializeField] private float maxRandomDelay = 3f;  // Maximum random delay

    [Header("Spawned Enemies")]
    [SerializeField] private int enemyCount = 10;  // Total number of enemies to spawn
    [SerializeField] private GameObject testGO;  // Prefab for testing enemy spawn
    [SerializeField] private Transform waypoint;  // Waypoint where enemies should spawn

    // These are outputs in the Inspector
    [Header("Spawn Stats")]
    [SerializeField] private float spawnTimer;  // Current spawn timer value
    [SerializeField] private int enemiesSpawned = 0;  // Count of enemies spawned
    [SerializeField] private int enemiesRemaining;  // Count of remaining enemies to spawn
    [SerializeField] private GameObject spawner;  // Reference to the spawner object
    [SerializeField] private Transform waypointOutput;  // Output the waypoint's position

    private float _randomDelay;  // Variable for random delay
    private ObjectPooler _pooler;  // Object pooler reference

    // Event to notify when a wave is completed
    public static event Action OnWaveCompleted;

    private void Start()
    {
        _pooler = GetComponent<ObjectPooler>();
        enemiesRemaining = enemyCount;  // Set remaining enemies to the total count initially
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;  // Update spawn timer

        // If spawn timer is below 0, spawn an enemy
        if (spawnTimer <= 0)
        {
            // Reset timer with either fixed delay or random delay
            spawnTimer = spawnMode ? delayBtwSpawns : UnityEngine.Random.Range(minRandomDelay, maxRandomDelay);

            if (enemiesSpawned < enemyCount)
            {
                enemiesSpawned++;
                enemiesRemaining--;  // Decrease remaining enemies
                SpawnEnemy();
            }
            else
            {
                // Notify wave completion
                WaveCompleted();
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject newInstance = _pooler.GetInstanceFromPool();
        newInstance.SetActive(true);
        newInstance.transform.position = waypoint.position;  // Spawn enemy at the waypoint position
    }

    private void WaveCompleted()
    {
        // Notify listeners (e.g., LevelManager) that the wave is complete
        Debug.Log("Wave completed with " + enemiesSpawned + " enemies spawned.");
        OnWaveCompleted?.Invoke();  // Trigger the WaveCompleted event

        // Reset the spawn count for the next wave
        enemiesSpawned = 0;
        enemiesRemaining = enemyCount;  // Reset remaining enemies
    }
}
