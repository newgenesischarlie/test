using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Action onDestroy;  // Event to be invoked when an enemy is destroyed
    [Header("Spawning Settings")]
    [SerializeField] private float spawnDelay = 2f; // Time between spawns
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool canSpawn = true;
    [SerializeField] private int maxEnemies = 10; // Optional: Maximum enemies to spawn

    [Header("Debug")]
    [SerializeField] private bool showSpawnGizmo = true;
    [SerializeField] private Color gizmoColor = Color.red;

    private float _spawnTimer;
    private int _enemiesSpawned;

    private ObjectPooler objectPooler;
    private float nextSpawnTime;
    private int enemiesSpawned; // Track how many enemies we've spawned

    // Define the WaveCompleted event
    //  public static event Actioßn OnWaveCompleted;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        if (objectPooler == null)
        {
            Debug.LogError("ObjectPooler not found in scene!");
            enabled = false;
            return;
        }

        // Set initial spawn time
        nextSpawnTime = Time.time + spawnDelay;
        enemiesSpawned = 0;
    }

    private void Update()
    {
        if (!canSpawn || objectPooler == null) return;
        if (maxEnemies > 0 && enemiesSpawned >= maxEnemies) return;

        // Check if it's time to spawn
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            // Set next spawn time
            nextSpawnTime = Time.time + spawnDelay;
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoint == null)
        {
            Debug.LogWarning("No spawn point assigned!");
            return;
        }

        GameObject enemy = objectPooler.GetInstanceFromPool("Enemy", spawnPoint.position, Quaternion.identity);
        if (enemy != null)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.ResetEnemy();
                enemiesSpawned++;
            }
        }
    }

    //  private void WaveCompleted()
    //  {
    //     // Notify listeners (e.g., LevelManager) that the wave is complete
    //      Debug.Log("Wave " + (enemyCount / _enemiesSpawned) + " completed!");
    //     OnWaveCompleted?.Invoke(); // Trigger the WaveCompleted event

    // Reset the spawn count for the next wave
    //      _enemiesSpawned = 0;
    //  }

    // Optional: Visualize spawn point in editor
    private void OnDrawGizmos()
    {
        if (!showSpawnGizmo || spawnPoint == null) return;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(spawnPoint.position, 0.5f);
    }

    // Public methods to control spawning
    public void StartSpawning()
    {
        canSpawn = true;
        nextSpawnTime = Time.time + spawnDelay;
    }

    public void StopSpawning()
    {
        canSpawn = false;
    }

    public void ResetSpawnCount()
    {
        enemiesSpawned = 0;
    }

    // Method to change spawn delay at runtime if needed
    public void SetSpawnDelay(float newDelay)
    {
        if (newDelay >= 0)
        {
            spawnDelay = newDelay;
        }
    }

    // Method to change max enemies at runtime if needed
    public void SetMaxEnemies(int newMax)
    {
        if (newMax >= 0)
        {
            maxEnemies = newMax;
        }
    }
}