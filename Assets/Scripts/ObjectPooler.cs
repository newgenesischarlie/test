using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler Instance;
    public List<Pool> pools;
    private Dictionary<string, List<GameObject>> poolDictionary;
    private bool isSpawningEnabled = true;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        poolDictionary = new Dictionary<string, List<GameObject>>();

        foreach (Pool pool in pools)
        {
            List<GameObject> objectPool = new List<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Add(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!isSpawningEnabled)
        {
            return null;
        }

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        // Get an inactive object from the pool
        GameObject objectToSpawn = null;
        foreach (GameObject obj in poolDictionary[tag])
        {
            if (!obj.activeInHierarchy)
            {
                objectToSpawn = obj;
                break;
            }
        }

        // If no inactive object was found
        if (objectToSpawn == null)
        {
            Debug.LogWarning($"No inactive objects available in pool {tag}");
            return null;
        }

        // Set up the object
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // Reset enemy component if it exists
        Enemy enemy = objectToSpawn.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.ResetEnemy();
        }

        return objectToSpawn;
    }

    public void StopSpawning()
    {
        isSpawningEnabled = false;
        // Deactivate all currently active objects
        List<GameObject> activeEnemies = GetAllActiveEnemies();
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null && enemy.activeInHierarchy)
            {
                ReturnToPool(enemy);
            }
        }
    }

    public void ResumeSpawning()
    {
        isSpawningEnabled = true;
    }

    public static void ReturnToPool(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }

    public List<GameObject> GetAllActiveEnemies()
    {
        List<GameObject> activeEnemies = new List<GameObject>();
        
        if (poolDictionary.ContainsKey("Enemy"))
        {
            foreach (GameObject enemy in poolDictionary["Enemy"])
            {
                if (enemy.activeInHierarchy)
                {
                    activeEnemies.Add(enemy);
                }
            }
        }
        
        return activeEnemies;
    }
}
