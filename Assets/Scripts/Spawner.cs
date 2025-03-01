using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
     public static Action onDestroy;  // Event to be invoked when an enemy is destroyed
    [Header("Settings")]
    [SerializeField] private int enemyCount = 10;
    [SerializeField] private GameObject testGO;

    [Header("Fixed Delay")]
    [SerializeField] private float delayBtwSpawns;

    private float _spawnTimer;
    private int _enemiesSpawned;

    private ObjectPooler _pooler;

    // Define the WaveCompleted event
    //  public static event Actioßn OnWaveCompleted;

    private void Start()
    {
        _pooler = GetComponent<ObjectPooler>();
    }

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer < 0)
        {
            _spawnTimer = delayBtwSpawns;
            if (_enemiesSpawned < enemyCount)
            {
                _enemiesSpawned++;
                SpawnEnemy();
            }
            else
            {
                // Trigger the event when all enemies are spawned
          //      WaveCompleted();
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = _pooler.GetInstanceFromPool("Enemy");
        enemy.SetActive(true);
    }

    //  private void WaveCompleted()
    //  {
    //     // Notify listeners (e.g., LevelManager) that the wave is complete
    //      Debug.Log("Wave " + (enemyCount / _enemiesSpawned) + " completed!");
    //     OnWaveCompleted?.Invoke(); // Trigger the WaveCompleted event

    // Reset the spawn count for the next wave
    //      _enemiesSpawned = 0;
    //  }
}