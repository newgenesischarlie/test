using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    // Simple serializable class for pool configuration
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    // Pool configuration
    [SerializeField] private List<Pool> pools = new List<Pool>();
    
    // Internal collections
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private HashSet<GameObject> activeObjects;
    private bool isSpawningEnabled = true;

    // Singleton instance
    public static ObjectPooler Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        Initialize();
    }

    private void Initialize()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        activeObjects = new HashSet<GameObject>();
        
        foreach (Pool pool in pools)
        {
            CreatePool(pool);
        }
    }

    private void CreatePool(Pool pool)
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < pool.size; i++)
        {
            GameObject obj = Instantiate(pool.prefab);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        poolDictionary.Add(pool.tag, objectPool);
    }

    public GameObject GetInstanceFromPool(string tag)
    {
        if (!isSpawningEnabled) return null;

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        Queue<GameObject> pool = poolDictionary[tag];
        
        GameObject objectToSpawn = null;
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy && !activeObjects.Contains(obj))
            {
                objectToSpawn = obj;
                break;
            }
        }

        if (objectToSpawn != null)
        {
            activeObjects.Add(objectToSpawn);
            objectToSpawn.SetActive(true);
            return objectToSpawn;
        }

        Debug.LogWarning($"No available objects in pool with tag {tag}");
        return null;
    }

    public static void ReturnToPool(GameObject obj)
    {
        if (Instance != null && obj != null)
        {
            Instance.activeObjects.Remove(obj);
            obj.SetActive(false);
        }
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
        List<GameObject> objectsToDeactivate = new List<GameObject>(activeObjects);
        foreach (var obj in objectsToDeactivate)
        {
            if (obj != null)
            {
                ReturnToPool(obj);
            }
        }
        activeObjects.Clear();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}