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
    private ObjectPooler objectPooler;
    #endregion

    private void Start()
    {
        InitializeGame();
        objectPooler = ObjectPooler.Instance;
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
        IsGameOver = false;
        
        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(false);
        }
        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);
        if (gameplayContainer != null) gameplayContainer.SetActive(true);
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

    private void Update()
    {
        // Only check for active enemies if the game is running
        if (!IsGameOver && objectPooler != null)
        {
            UpdateActiveEnemies();
        }
    }

    private void UpdateActiveEnemies()
    {
        // Only log if debugging is needed
        //List<GameObject> activeEnemies = objectPooler.GetAllActiveEnemies();
        //if (activeEnemies.Count == 0)
        //{
        //    Debug.LogWarning("No active enemies found in the pool.");
        //}
    }

    public void HandleEndReached(Enemy enemy)
    {
        if (IsGameOver) return;
        ShowGameOver(false);
    }

    public void HandleEnemyDefeated(Enemy enemy)
    {
        if (IsGameOver) return;
        ShowGameOver(true);
    }

    private void ShowGameOver(bool isWin)
    {
        IsGameOver = true;

        // Disable gameplay elements
        if (gameplayContainer != null)
        {
            gameplayContainer.SetActive(false);
        }

        // Show appropriate screen
        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(true);
            if (isWin && winScreen != null)
            {
                winScreen.SetActive(true);
            }
            else if (!isWin && loseScreen != null)
            {
                loseScreen.SetActive(true);
            }
        }

        // Deactivate all enemies
        DeactivateAllEnemies();
    }

    private void DeactivateAllEnemies()
    {
        // Find all active enemies and deactivate them
        Enemy[] activeEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in activeEnemies)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy)
            {
                ObjectPooler.ReturnToPool(enemy.gameObject);
            }
        }

        // Disable spawners
        WaveSpawner[] spawners = FindObjectsOfType<WaveSpawner>();
        foreach (WaveSpawner spawner in spawners)
        {
            spawner.enabled = false;
        }
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

    public void RestartGame()
    {
        // Reset UI
        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(false);
        }
        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);

        // Enable gameplay elements
        if (gameplayContainer != null)
        {
            gameplayContainer.SetActive(true);
        }

        // Re-enable spawners
        WaveSpawner[] spawners = FindObjectsOfType<WaveSpawner>();
        foreach (WaveSpawner spawner in spawners)
        {
            spawner.enabled = true;
        }

        IsGameOver = false;
    }
}
