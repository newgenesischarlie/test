using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject gameplayContainer; // Parent object containing all gameplay elements
    [SerializeField] private ObjectPooler objectPooler;
    
    // Optional: Specific objects to hide/show if not under gameplayContainer
    [SerializeField] private GameObject[] gameplayObjects;
    #endregion

    #region Public Properties
    public bool IsGameOver { get; private set; }  // Changed to property with public getter
    #endregion

    #region Private Fields
    private bool isGameStarted;
    private List<GameObject> allEnemies = new List<GameObject>();
    #endregion

    private void Start()
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
        // Initialize UI - ensure screens start hidden
        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);
        
        // Ensure gameplay elements are visible
        ShowGameplayElements(true);

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

        isGameStarted = true;
        IsGameOver = false;
        objectPooler.ResumeSpawning(); // Enable spawning when game starts
    }

    private void ShowGameplayElements(bool show)
    {
        // Hide/Show main gameplay container if assigned
        if (gameplayContainer != null)
        {
            gameplayContainer.SetActive(show);
        }

        // Hide/Show individual gameplay objects if assigned
        if (gameplayObjects != null)
        {
            foreach (GameObject obj in gameplayObjects)
            {
                if (obj != null && obj != winScreen && obj != loseScreen)
                {
                    obj.SetActive(show);
                }
            }
        }
    }

    void Update()
    {
        if (IsGameOver) return; // If the game is over, do nothing

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

    private void EndGame()
    {
        IsGameOver = true;
        ShowGameplayElements(false);
        
        // Stop the object pooler
        if (objectPooler != null)
        {
            objectPooler.StopSpawning();
        }
    }

    public void HandleEndReached(Enemy enemy)
    {
        if (IsGameOver) return;
        
        Debug.Log("Enemy reached end point - Game Over!");
        EndGame();
        
        // Show lose screen
        if (loseScreen != null)
        {
            loseScreen.SetActive(true);
        }
    }

    public void HandleEnemyDefeated(Enemy enemy)
    {
        if (IsGameOver) return;
        
        Debug.Log("Enemy defeated - You Win!");
        EndGame();
        
        // Show win screen
        if (winScreen != null)
        {
            winScreen.SetActive(true);
        }
    }

    // Optional: Method to restart the game
    public void RestartGame()
    {
        // Hide end screens
        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);

        // Show gameplay elements
        ShowGameplayElements(true);

        // Reset game state
        IsGameOver = false;
        
        // Resume object pooling
        if (objectPooler != null)
        {
            objectPooler.ResumeSpawning();
        }

        // Additional restart logic here
    }
}
