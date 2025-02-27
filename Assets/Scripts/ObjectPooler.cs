using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 10;
    private List<GameObject> _pool;
    private GameObject _poolContainer;

    // List to keep track of active enemies
    private List<GameObject> _activeEnemies = new List<GameObject>();

    private GameManager gameManager;

    // Singleton instance
    public static ObjectPooler Instance { get; private set; }

    private GameObject CreateInstance()
    {
        GameObject newInstance = Instantiate(prefab);
        newInstance.transform.SetParent(_poolContainer.transform);
        newInstance.SetActive(false);
        return newInstance;
    }

    private void CreatePooler()
    {
        _poolContainer = new GameObject($"_pool_{prefab.name}");
        for (int i = 0; i < poolSize; i++)
        {
            _pool.Add(CreateInstance());
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }

        _pool = new List<GameObject>();
        CreatePooler();

        // Get a reference to the GameManager
        gameManager = FindObjectOfType<GameManager>();
    }

    public GameObject GetInstanceFromPool()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].activeInHierarchy)
            {
                _pool[i].SetActive(true);  // Activate the object
                _activeEnemies.Add(_pool[i]); // Add to active enemies list
                return _pool[i];
            }
        }
        return null;
    }

    public static void ReturnToPool(GameObject instance)
    {
        instance.SetActive(false);
        if (instance.GetComponent<Enemy>() != null)
        {
            instance.GetComponent<Enemy>().NotifyEnemyDestroyed();
        }
    }

    // Coroutine to return to pool with a delay
    public static IEnumerator ReturnToPoolWithDelay(GameObject instance, float delay)
    {
        yield return new WaitForSeconds(delay);
        instance.SetActive(false);
        if (instance.GetComponent<Enemy>() != null)
        {
            instance.GetComponent<Enemy>().NotifyEnemyDestroyed();
        }
    }

    // Get all active enemies
    public List<GameObject> GetAllActiveEnemies()
    {
        return _activeEnemies; // Return the list of active enemies
    }

    public int GetTotalEnemiesInScene()
    {
        return _activeEnemies.Count; // Total count of active enemies
    }

    public void ResetPool()
    {
        _activeEnemies.Clear();
    }
}
