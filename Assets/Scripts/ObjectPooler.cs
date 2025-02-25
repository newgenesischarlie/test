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

    // To keep track of active enemies
    private int _activeEnemies = 0;

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

    public GameObject GetInstanceFromPool()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].activeInHierarchy)
            {
                _pool[i].SetActive(true);
                _activeEnemies++; // Increment active enemies when an enemy is spawned
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
            GameManager.Instance.EnemyDestroyed(); // Notify the GameManager when an enemy is returned
        }
    }

    public static IEnumerator ReturnToPoolWithDelay(GameObject instance, float delay)
    {
        yield return new WaitForSeconds(delay);
        instance.SetActive(false);
        if (instance.GetComponent<Enemy>() != null)
        {
            GameManager.Instance.EnemyDestroyed(); // Notify GameManager
        }
    }

    // New method to get the count of active enemies
    public int GetActiveEnemyCount()
    {
        return _activeEnemies;
    }

    // Method to decrease the count when an enemy is destroyed
    public void DecrementActiveEnemyCount()
    {
        _activeEnemies--;
    }
}
