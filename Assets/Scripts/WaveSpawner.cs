using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public int enemyCount;
        public float spawnDelay;
        public float timeBetweenWaves;
    }

    [Header("Wave Settings")]
    [SerializeField] private Wave[] waves;
    [SerializeField] private Transform spawnPoint;
    
    [Header("Debug")]
    [SerializeField] private bool showSpawnGizmo = true;
    [SerializeField] private Color gizmoColor = Color.red;

    private ObjectPooler objectPooler;
    private int currentWave = 0;
    private bool isSpawning = false;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        if (objectPooler == null)
        {
            Debug.LogError("ObjectPooler not found!");
            enabled = false;
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("Spawn point not set!");
            enabled = false;
            return;
        }

        StartNextWave();
    }

    private void StartNextWave()
    {
        if (currentWave < waves.Length && !isSpawning)
        {
            StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()
    {
        isSpawning = true;
        Wave wave = waves[currentWave];

        Debug.Log($"Starting Wave: {wave.waveName}");

        for (int i = 0; i < wave.enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(wave.spawnDelay);
        }

        yield return new WaitForSeconds(wave.timeBetweenWaves);

        isSpawning = false;
        currentWave++;
        StartNextWave();
    }

    private void SpawnEnemy()
    {
        if (spawnPoint == null) 
        {
            Debug.LogError("Spawn point not set!");
            return;
        }

        GameObject enemy = objectPooler.GetInstanceFromPool("Enemy", spawnPoint.position, Quaternion.identity);
        if (enemy != null)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.ResetEnemy();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!showSpawnGizmo || spawnPoint == null) return;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(spawnPoint.position, 0.5f);
    }

    // Public control methods
    public void PauseSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    public void ResumeSpawning()
    {
        if (!isSpawning)
        {
            StartNextWave();
        }
    }
} 