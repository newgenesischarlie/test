using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isGameOver = false;
    private bool isGameStarted = false;

    [SerializeField] private GameObject winScreen; // Reference to the win screen UI panel
    [SerializeField] private GameObject loseScreen; // Reference to the lose screen UI panel

    [SerializeField] private List<GameObject> allEnemies = new List<GameObject>(); // Store all active enemies

    public ObjectPooler objectPooler; // Reference to the ObjectPooler

    void Start()
    {
        // Ensure the object pooler is assigned
        if (objectPooler == null)
        {
            objectPooler = ObjectPooler.Instance;

        }

        if (objectPooler == null)
        {
            Debug.LogError("ObjectPooler is not assigned or found in the scene.");
        }

        // Start game simulation if required
        if (!isGameStarted)
        {
            StartGame();
        }

        // Hide the win and lose screens at the start
        if (winScreen != null)
        {
            winScreen.SetActive(false);
        }
        if (loseScreen != null)
        {
            loseScreen.SetActive(false);
        }
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
        // Check if enemy is null
        if (enemy == null)
        {
            Debug.LogError("Enemy is null in HandleEndReached! Please check the event subscription and enemy state.");
            return;
        }

        Debug.Log("Enemy reached the end: " + enemy.gameObject.name);

        // Proceed with the logic (for example, calling LoseGame or WinGame)
        // Example condition: Lose if an enemy reaches the end
        LoseGame();
    }

    void WinGame()
    {
        // Game won logic here
        Debug.Log("You Win!");
        isGameOver = true;

        // Show the win screen (game over screen)
        if (winScreen != null)
        {
            winScreen.SetActive(true); // Activate the win screen UI
        }
    }

    void LoseGame()
    {
        // Game over logic here
        Debug.Log("Game Over!");
        isGameOver = true;

        // Show the lose screen (game over screen)
        if (loseScreen != null)
        {
            loseScreen.SetActive(true); // Activate the lose screen UI
        }
    }

    void StartGame()
    {
        // Game start logic here
        isGameStarted = true;
        Debug.Log("Game Started!");
    }
}
