using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Reference to the ObjectPooler
    public ObjectPooler objectPooler;

    // Final waypoint coordinates (set these directly in the Inspector)
    public Vector3 finalWaypointCoordinates;

    // UI elements to display win/lose messages
    public GameObject winScreen;
    public GameObject loseScreen;

    private List<GameObject> allEnemies; // List of all spawned enemies
    private bool isGameOver = false;

    void Start()
    {
        // Initialize the enemy list
        allEnemies = new List<GameObject>();

        // Add all currently active enemies to the list
        AddActiveEnemiesToList();

        // Hide win/lose screens at the start
        winScreen.SetActive(false);
        loseScreen.SetActive(false);

        // Subscribe to OnEndReached event for each enemy
        SubscribeToEnemyEndEvent();
    }

    void Update()
    {
        if (isGameOver) return; // If the game is over, do nothing

        // Check if the object (e.g., player or any object) reaches the final waypoint
        if (Vector3.Distance(transform.position, finalWaypointCoordinates) < 1f)
        {
            // Check if all enemies are dead
            if (AreAllEnemiesDead())
            {
                WinGame();
            }
            else
            {
                LoseGame();
            }
        }

        // Debugging: Press 'E' to trigger the game over scenario
        if (Input.GetKeyDown(KeyCode.E))
        {
            // This will simulate an enemy reaching the end point
            Debug.Log("Triggering game over manually via 'E' key");
            HandleEndReached(null);  // You can simulate the event here for testing purposes
        }
    }

    void AddActiveEnemiesToList()
    {
        // Get all active enemies from the object pooler
        foreach (GameObject enemy in objectPooler.GetAllActiveEnemies())
        {
            if (!allEnemies.Contains(enemy))
            {
                allEnemies.Add(enemy);
            }
        }
    }

    bool AreAllEnemiesDead()
    {
        // Check if all enemies are dead by looking through the list of enemies
        foreach (GameObject enemy in allEnemies)
        {
            if (enemy != null && enemy.activeInHierarchy)
            {
                return false; // If any enemy is still alive, return false
            }
        }
        return true; // All enemies are dead
    }

    void WinGame()
    {
        // Display win screen and stop the game
        winScreen.SetActive(true);
        Time.timeScale = 0f; // Freeze the game
        isGameOver = true;
    }

    void LoseGame()
    {
        // Display lose screen and stop the game
        loseScreen.SetActive(true);
        Time.timeScale = 0f; // Freeze the game
        isGameOver = true;
    }

    public void OnEnemyKilled(GameObject enemy)
    {
        if (allEnemies.Contains(enemy))
        {
            allEnemies.Remove(enemy);
        }
    }

    void SubscribeToEnemyEndEvent()
    {
        // Get all active enemies from the object pooler
        foreach (GameObject enemy in objectPooler.GetAllActiveEnemies())
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                // Subscribe to the static event using the class name
                Debug.Log("Subscribing to OnEndReached event for " + enemy.name);
                Enemy.OnEndReached += HandleEndReached;
            }
            else
            {
                Debug.LogWarning("Enemy script not found on " + enemy.name);
            }
        }
    }

    public void HandleEndReached(Enemy enemy)
    {
        // Check if enemy is null
        if (enemy == null)
        {
            Debug.LogError("Enemy is null in HandleEndReached! Please check the event subscription and enemy state.");
            return;
        }

        // If the enemy is valid, log the enemy's name for more clarity
        Debug.Log("Enemy reached the end: " + enemy.gameObject.name);

        // Proceed with the logic (for example, calling LoseGame)
        LoseGame();
    }

    void OnDestroy()
    {
        // Unsubscribe from the event when the object is destroyed
        foreach (GameObject enemy in objectPooler.GetAllActiveEnemies())
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                // Unsubscribe from the static event using the class name
                Debug.Log("Unsubscribing from OnEndReached event for " + enemy.name);
                Enemy.OnEndReached -= HandleEndReached;
            }
            else
            {
                Debug.LogWarning("Enemy script not found on " + enemy.name);
            }
        }
    }
}
