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
    public List<GameObject> _activeEnemies = new List<GameObject>();

    public static ObjectPooler Instance;

    private void Awake()
    {
        // Ensure that there's only one instance of ObjectPooler
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy the duplicate ObjectPooler
        }

        _pool = new List<GameObject>();
        CreatePooler();
    }

    // Method to create an instance of the prefab
    public GameObject CreateInstance()
    {
        GameObject newInstance = Instantiate(prefab);
        newInstance.transform.SetParent(_poolContainer.transform);
        newInstance.SetActive(false);
        return newInstance;
    }

    // Method to create the object pool
    private void CreatePooler()
    {
        _poolContainer = new GameObject($"_pool_{prefab.name}");
        for (int i = 0; i < poolSize; i++)
        {
            _pool.Add(CreateInstance());
        }
    }

    // Get an inactive instance from the pool
    public GameObject GetInstanceFromPool()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].activeInHierarchy)
            {
                _pool[i].SetActive(true);  // Ensure the instance is activated before returning
                _activeEnemies.Add(_pool[i]); // Add it to the active enemies list
                return _pool[i];
            }
        }
        return null; // Return null if no inactive object is found
    }

    // Return an instance to the pool
    public static void ReturnToPool(GameObject instance)
    {
        if (instance == null)
        {
            Debug.LogError("Attempted to return a null instance to the pool!");
            return;
        }

        // Deactivate the object and remove from active enemies list
        instance.SetActive(false);

        // Get the ObjectPooler instance and remove from active enemies
        if (Instance != null)
        {
            Instance.RemoveFromActiveEnemies(instance);
        }
        else
        {
            Debug.LogError("ObjectPooler instance is missing! Could not remove enemy from the list.");
        }
    }

    // Coroutine to return the object to the pool with a delay
    public static IEnumerator ReturnToPoolWithDelay(GameObject instance, float delay)
    {
        yield return new WaitForSeconds(delay);
        instance.SetActive(false);
        if (Instance != null)
        {
            Instance.RemoveFromActiveEnemies(instance);
        }
        else
        {
            Debug.LogError("ObjectPooler instance is missing! Could not remove enemy from the list.");
        }
    }

    // Helper method to retrieve active enemies
    public List<GameObject> GetActiveEnemies()
    {
        return _activeEnemies; // Return the list of currently active enemies
    }

    // Optional: Reset the pool and active enemies list (useful for wave resets or scene changes)
    public void ResetPool()
    {
        _activeEnemies.Clear();
    }

    // Get all active enemies in the pool
    public List<GameObject> GetAllActiveEnemies()
    {
        List<GameObject> activeEnemies = new List<GameObject>();
        foreach (GameObject enemy in _activeEnemies)
        {
            if (enemy.activeInHierarchy)
            {
                activeEnemies.Add(enemy);
            }
        }
        return activeEnemies;
    }

    // Remove an enemy from the list of active enemies
    public void RemoveFromActiveEnemies(GameObject enemy)
    {
        if (_activeEnemies.Contains(enemy))
        {
            _activeEnemies.Remove(enemy);
        }
    }

    // Add an enemy to the list of active enemies
    public void AddToActiveEnemies(GameObject enemy)
    {
        if (!_activeEnemies.Contains(enemy))
        {
            _activeEnemies.Add(enemy);
        }
    }

    // You can also implement this method to handle reactivation of enemies
    public void ActivateEnemy(GameObject enemy)
    {
        if (enemy != null)
        {
            enemy.SetActive(true);
            AddToActiveEnemies(enemy);
        }
    }
}
