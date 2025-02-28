using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Serialized Fields
    [Header("UI References")]
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject gameplayContainer; // Container for all gameplay objects

    [Header("UI Settings")]
    [SerializeField] private float fadeSpeed = 0.5f;

    [SerializeField] private Camera uiCamera;
    [SerializeField] private GameObject[] gameplayObjects;
    #endregion

    #region Public Properties
    public bool IsGameOver { get; private set; }
    #endregion

    #region Private Fields
    private List<GameObject> allEnemies = new List<GameObject>();
    #endregion

    private void Start()
    {
        InitializeGame();
        SubscribeToEvents();
        InitializeUI();
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
        // Set up UI
        if (gameOverCanvas != null)
        {
            gameOverCanvas.sortingOrder = 999; // Ensure it's on top
            gameOverCanvas.gameObject.SetActive(false);
        }
        
        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);
        
        IsGameOver = false;
    }

    private void InitializeUI()
    {
        if (gameOverCanvas != null && uiCamera != null)
        {
            // Set up UI camera
            uiCamera.clearFlags = CameraClearFlags.Depth;
            uiCamera.depth = 999;
            
            // Set up canvas
            gameOverCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            gameOverCanvas.worldCamera = uiCamera;
            gameOverCanvas.planeDistance = 1f;
        }

        // Hide screens initially
        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);
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
        List<GameObject> activeEnemies = ObjectPooler.Instance.GetAllActiveEnemies();

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
        if (IsGameOver) return;
        
        ShowGameOver(false); // Show lose screen
    }

    public void HandleEnemyDefeated(Enemy enemy)
    {
        if (IsGameOver) return;
        
        ShowGameOver(true); // Show win screen
    }

    private void ShowGameOver(bool isWin)
    {
        IsGameOver = true;

        // Disable gameplay elements
        if (gameplayContainer != null)
        {
            gameplayContainer.SetActive(false);
        }

        // Enable canvas and appropriate screen
        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(true);
            
            if (isWin && winScreen != null)
            {
                winScreen.SetActive(true);
                StartCoroutine(FadeInUI(winScreen.GetComponent<CanvasGroup>()));
            }
            else if (!isWin && loseScreen != null)
            {
                loseScreen.SetActive(true);
                StartCoroutine(FadeInUI(loseScreen.GetComponent<CanvasGroup>()));
            }
        }

        // Stop spawning
        ObjectPooler.Instance.StopSpawning();
    }

    private IEnumerator FadeInUI(CanvasGroup canvasGroup)
    {
        if (canvasGroup == null) yield break;

        canvasGroup.alpha = 0f;
        
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime / fadeSpeed;
            yield return null;
        }
        
        canvasGroup.alpha = 1f;
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
        ObjectPooler.Instance.ResumeSpawning();

        // Additional restart logic here
    }
}
