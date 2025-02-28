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

    // Call this method when an enemy is killed
    public void OnEnemyKilled(GameObject enemy)
    {
        if (allEnemies.Contains(enemy))
        {
            allEnemies.Remove(enemy);
        }
    }

    void SubscribeToEnemyEndEvent()
    {
        // Subscribe to the OnEndReached event for a specific enemy
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.OnEndReached += HandleEndReached; // Corrected to subscribe for individual enemies
        }
    }

    void HandleEndReached(Enemy enemy)
    {
        // Check if enemy is null
        if (enemy == null)
        {
            Debug.LogError("Enemy is null in HandleEndReached!");
            return;
        }

        // Proceed with the logic if enemy is valid
        Debug.Log("Enemy reached the end: " + enemy.gameObject.name);
        LoseGame();  // Assuming you want to call LoseGame when an enemy reaches the end
    }



    void OnDestroy()
    {
        // Unsubscribe from all enemies in the pool
        foreach (GameObject enemy in objectPooler.GetAllActiveEnemies())
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                // Unsubscribe from the OnEndReached event
                enemyScript.OnEndReached -= HandleEndReached;
            }
        }
    }

    // Method to simulate game over and trigger the lose screen for testing
    void TriggerGameOver()
    {
        Debug.Log("Game Over triggered!");
        loseScreen.SetActive(true); // Show the lose screen
        Time.timeScale = 0f; // Stop the game time (freeze the game)
        isGameOver = true;
    }
    public void SpawnEnemy(GameObject enemyPrefab)
    {
        // Instantiate the enemy from the pool or prefab
        GameObject newEnemy = Instantiate(enemyPrefab);

        // Subscribe to the OnEndReached event after instantiating the enemy
        SubscribeToEnemyEndEvent(newEnemy); // Ensure the event handler is added here

        // Optionally, add the enemy to the list if needed
        allEnemies.Add(newEnemy);
    }
}
