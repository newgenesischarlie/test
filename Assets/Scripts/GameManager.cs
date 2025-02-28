using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isGameOver = false;

    [SerializeField] private GameObject winScreen; // Reference to the win screen UI panel
    [SerializeField] private GameObject loseScreen; // Reference to the lose screen UI panel

    [SerializeField] private List<GameObject> allEnemies = new List<GameObject>(); // Store all active enemies

    public ObjectPooler objectPooler; // Reference to the ObjectPooler

    private List<GameObject> allGameObjectsInScene = new List<GameObject>(); // Store all game objects in the scene
    private Camera mainCamera; // Reference to the main camera

    void Start()
    {
        InitializeGame();
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        Enemy.OnEndReached += HandleEndReached;
        Enemy.OnEnemyDefeated += HandleEnemyDefeated;
    }

    private void UnsubscribeFromEvents()
    {
        Enemy.OnEndReached -= HandleEndReached;
        Enemy.OnEnemyDefeated -= HandleEnemyDefeated;
    }

    private void InitializeGame()
    {
        // Initialize object pooler
        if (objectPooler == null)
        {
            objectPooler = ObjectPooler.Instance;
            if (objectPooler == null)
            {
                Debug.LogError("ObjectPooler is not assigned or found in the scene.");
                return;
            }
        }

        // Initialize UI - ensure screens start hidden
        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);

        // Find all game objects in the scene, but exclude the camera, UI, and win/lose screens
        allGameObjectsInScene.Clear();
        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            if (obj != winScreen && obj != loseScreen && obj.GetComponent<Camera>() == null)
            {
                allGameObjectsInScene.Add(obj);
            }
        }

        // Get the reference to the main camera
        mainCamera = Camera.main;

        // Start game
        isGameOver = false;
    }

    void Update()
    {
        if (isGameOver) return; // If the game is over, do nothing

        // Add active enemies to the list
        AddActiveEnemiesToList();

        // Debugging: Press 'E' to simulate the game over scenario manually
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Simulating game over manually via 'E' key");

            if (allEnemies.Count > 0)
            {
                foreach (var enemy in allEnemies)
                {
                    if (enemy != null && enemy.activeInHierarchy)
                    {
                        HandleEndReached(enemy.GetComponent<Enemy>());
                        break; // Only trigger the event for the first active enemy
                    }
                }
            }
            else
            {
                Debug.LogWarning("No enemies available for simulation.");
            }
        }
    }

    void AddActiveEnemiesToList()
    {
        allEnemies.Clear(); // Clear the list first

        // Assuming ObjectPooler has a method to return all active enemies
        List<GameObject> activeEnemies = objectPooler.GetAllActiveEnemies();

        if (activeEnemies.Count == 0)
        {
            Debug.LogWarning("No active enemies found in the pool.");
        }

        foreach (GameObject enemy in activeEnemies)
        {
            if (!allEnemies.Contains(enemy))
            {
                allEnemies.Add(enemy);
                Debug.Log("Added enemy to allEnemies: " + enemy.name); // Debug log to ensure enemies are being added
            }
        }
    }

    public void HandleEndReached(Enemy enemy)
    {
        if (isGameOver) return;

        Debug.Log("Enemy reached end point - Game Over!");
        isGameOver = true;

        // Show lose screen when enemy reaches final waypoint
        if (loseScreen != null)
        {
            loseScreen.SetActive(true);
        }

        // Deactivate all game objects in the scene except for the UI and camera
        DeactivateAllGameObjects();

        // Stop Object Pooling
        objectPooler.StopPooling();
    }

    public void HandleEnemyDefeated(Enemy enemy)
    {
        if (isGameOver) return;

        Debug.Log("Enemy defeated - You Win!");
        isGameOver = true;

        // Show win screen when enemy is defeated
        if (winScreen != null)
        {
            winScreen.SetActive(true);
        }

        // Deactivate all game objects in the scene except for the UI and camera
        DeactivateAllGameObjects();

        // Stop Object Pooling
        objectPooler.StopPooling();
    }

    // Deactivate all game objects in the scene except for the win/lose screens, camera, and UI
    private void DeactivateAllGameObjects()
    {
        foreach (var obj in allGameObjectsInScene)
        {
            // Exclude the main camera and UI screens
            if (obj != null && obj != mainCamera.gameObject && obj != winScreen && obj != loseScreen)
            {
                obj.SetActive(false);
            }
        }
    }

    // Reactivate all game objects in the scene after the game is over
    public void ReactivateAllGameObjects()
    {
        foreach (var obj in allGameObjectsInScene)
        {
            // Exclude the main camera and UI screens
            if (obj != null && obj != mainCamera.gameObject && obj != winScreen && obj != loseScreen)
            {
                obj.SetActive(true);
            }
        }
    }
}
