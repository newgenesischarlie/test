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
    public List<Pool> pools = new List<Pool>();
    private Dictionary<string, List<GameObject>> poolDictionary;
    private bool isSpawningEnabled = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        poolDictionary = new Dictionary<string, List<GameObject>>();
        InitializePools();
    }

    private void InitializePools()
    {
        foreach (Pool pool in pools)
        {
            if (pool.prefab == null)
            {
                Debug.LogError($"Prefab for pool {pool.tag} is not assigned!");
                continue;
            }

            List<GameObject> objectPool = new List<GameObject>();
            GameObject poolParent = new GameObject($"Pool-{pool.tag}");
            poolParent.transform.parent = transform;

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, poolParent.transform);
                obj.SetActive(false);
                objectPool.Add(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
            Debug.Log($"Created pool: {pool.tag} with {pool.size} objects");
        }

        isSpawningEnabled = true;
    }

    public GameObject GetInstanceFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!isSpawningEnabled)
        {
            return null;
        }

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError($"Pool with tag {tag} doesn't exist. Make sure to set up the pool in the Inspector!");
            return null;
        }

        foreach (GameObject obj in poolDictionary[tag])
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }
        }

        return null;
    }

    public void StopSpawning()
    {
        isSpawningEnabled = false;
        DeactivateAllObjects();
    }

    public void ResumeSpawning()
    {
        isSpawningEnabled = true;
    }

    private void DeactivateAllObjects()
    {
        if (poolDictionary == null) return;

        foreach (var pool in poolDictionary.Values)
        {
            foreach (GameObject obj in pool)
            {
                if (obj != null && obj.activeInHierarchy)
                {
                    obj.SetActive(false);
                }
            }
        }
    }

    public List<GameObject> GetAllActiveEnemies()
    {
        List<GameObject> activeEnemies = new List<GameObject>();
        
        if (poolDictionary != null && poolDictionary.ContainsKey("Enemy"))
        {
            foreach (GameObject enemy in poolDictionary["Enemy"])
            {
                if (enemy != null && enemy.activeInHierarchy)
                {
                    activeEnemies.Add(enemy);
                }
            }
        }
        
        return activeEnemies;
    }

    public static void ReturnToPool(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }

    public void RemoveFromActiveEnemies(GameObject enemy)
    {
        if (enemy != null)
        {
            ReturnToPool(enemy);
        }
    }
}
