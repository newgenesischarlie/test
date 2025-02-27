using Unity;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    private Vector3 spawnPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Ensure it persists across scenes
        }
        else
        {
            Destroy(gameObject);  // Prevent multiple instances
        }
    }

    // Spawning new enemies in a wave from the pool
    private void SpawnNextWave(List<Enemy> waveEnemies)
    //  {
    //   foreach (var enemy in waveEnemies)
    //  {
    // Corrected to use GetInstanceFromPool() instead of GetPooledFromObject()
    //  GameObject enemyObject = ObjectPooler.Instance.GetInstanceFromPool();
    //   if (enemyObject != null)
    //  {
    //       enemyObject.transform.position = spawnPosition;  // Set spawn position (customize as needed)
    //      enemyObject.GetComponent<Enemy>().Initialize(enemy);  // Initialize the enemy if necessary
    //  }
    //   else
    {
        Debug.LogError("No inactive enemies available in the pool!");
    }

    // Removes an enemy from the list of active enemies (called when the enemy dies or is finished)
    public void RemoveEnemyFromList(Enemy enemy)
    {
        // Ensure to remove only from the list if it's active
        //  if (ObjectPooler.Instance.GetActiveEnemies().Contains(enemy.gameObject))
        {
            //      ObjectPooler.Instance.GetActiveEnemies().Remove(enemy.gameObject);
        }
    }
}