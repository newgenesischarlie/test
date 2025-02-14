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

    private GameObject CreateInstance()
    {
        GameObject newInstance = Instantiate(prefab);
        newInstance.transform.SetParent(_poolContainer.transform);
        newInstance.SetActive(false);
        return newInstance;
    }

    private void CreatePooler()
    {
        _poolContainer = new GameObject($"_pool_{prefab.name}");  // Fixed name generation
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

    public GameObject GetInstanceFromPool()  // Fixed duplicate method definition
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].activeInHierarchy)
            {
                _pool[i].SetActive(true);  // Ensure the instance is activated before returning
                return _pool[i];
            }
        }
        return null; // Return null if no inactive object is found
    }

    public static void ReturnToPool(GameObject instance)
    {
        instance.SetActive(false);
    }

    public static IEnumerator ReturnToPoolWithDelay(GameObject instance, float delay)
    {
        yield return new WaitForSeconds(delay);
        instance.SetActive(false);
    }

}
