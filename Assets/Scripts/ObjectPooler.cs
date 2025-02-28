using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag = "Enemy";
        public GameObject prefab;
        public int size = 10;
    }

    public static ObjectPooler Instance;
    
    [Header("Pool Configuration")]
    [SerializeField] private GameObject enemyPrefab; // Direct reference to enemy prefab
    [SerializeField] private int poolSize = 10;      // Default pool size

    private Dictionary<string, List<GameObject>> poolDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePool()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy Prefab not assigned to ObjectPooler!");
            return;
        }

        poolDictionary = new Dictionary<string, List<GameObject>>();
        
        // Create enemy pool
        List<GameObject> enemyPool = new List<GameObject>();
        GameObject poolParent = new GameObject("Pool-Enemy");
        poolParent.transform.parent = transform;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab, poolParent.transform);
            obj.SetActive(false);
            enemyPool.Add(obj);
        }

        poolDictionary.Add("Enemy", enemyPool);
        Debug.Log($"Created Enemy pool with {poolSize} objects");
    }

    public GameObject GetInstanceFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError($"Pool with tag {tag} doesn't exist!");
            return null;
        }

        List<GameObject> pool = poolDictionary[tag];
        
        // Find inactive object in pool
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }
        }

        // If no inactive object found, create new one
        GameObject newObj = Instantiate(enemyPrefab, position, rotation);
        pool.Add(newObj);
        return newObj;
    }

    public List<GameObject> GetAllActiveEnemies()
    {
        List<GameObject> activeEnemies = new List<GameObject>();
        
        if (poolDictionary.ContainsKey("Enemy"))
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
}
