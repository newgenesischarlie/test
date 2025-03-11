using UnityEngine;

public class PoolHelper : MonoBehaviour
{
    private static PoolHelper _instance;
    public static PoolHelper Instance 
    {
        get 
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("PoolHelper");
                _instance = go.AddComponent<PoolHelper>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
    }

    // Method to get a projectile (using direct instantiation)
    public GameObject GetProjectile(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null) return null;
        
        // Just instantiate directly without using the pool
        GameObject obj = Instantiate(prefab, position, rotation);
        return obj;
    }
} 