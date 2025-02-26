using System.Collections;
using System.Collections.Generic;
using Unity;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 10;
    private List<GameObject> _pool;
    private GameObject _poolContainer;

    // List to keep track of active enemies (for the WaveManager)
    private List<GameObject> _activeEnemies = new List<GameObject>();

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
        _pool = new List<GameObject>();
        CreatePooler();
    }

    // Correct method to get an inactive instance from the pool
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

    // Method to return an object to the pool and remove it from the active list
    public static void ReturnToPool(GameObject instance)
    {
        instance.SetActive(false);
        if (instance.GetComponent<Enemy>() != null)
        {
            WaveManager.Instance.RemoveEnemyFromList(instance.GetComponent<Enemy>());  // Remove from the wave manager's list of active enemies
        }
    }

    // Coroutine to return the object to the pool with a delay
    public static IEnumerator ReturnToPoolWithDelay(GameObject instance, float delay)
    {
        yield return new WaitForSeconds(delay);
        instance.SetActive(false);
        if (instance.GetComponent<Enemy>() != null)
        {
            WaveManager.Instance.RemoveEnemyFromList(instance.GetComponent<Enemy>());
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
}
