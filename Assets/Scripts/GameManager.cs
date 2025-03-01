using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject gameplayContainer;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text currencyText;

    [Header("Game Settings")]
    [SerializeField] private int startingCurrency = 100;

    private List<Enemy> activeEnemies = new List<Enemy>();
    public bool IsGameOver { get; private set; }
    private int score = 0;
    private int _currency;

    // Property for currency with getter and setter
    public int Currency
    {
        get { return _currency; }
        set
        {
            _currency = value;
            UpdateCurrencyUI();
        }
    }

    private void Awake()
    {
        Enemy.OnEndReached += HandleEndReached;
    }

    private void OnDestroy()
    {
        Enemy.OnEndReached -= HandleEndReached;
    }

    private void Start()
    {
        InitializeGame();
        Currency = startingCurrency;
        UpdateScoreUI();
    }

    private void Update()
    {
        if (!IsGameOver)
        {
            UpdateActiveEnemies();
        }
    }

    private void UpdateActiveEnemies()
    {
        // Clear the list and rebuild it with currently active enemies
        activeEnemies.Clear();
        
        // Find all active enemies in the scene
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy)
            {
                activeEnemies.Add(enemy);
            }
        }
    }

    private void InitializeGame()
    {
        IsGameOver = false;
        activeEnemies.Clear();

        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(false);
        }
        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);
        if (gameplayContainer != null) gameplayContainer.SetActive(true);
    }

    private void HandleEndReached(Enemy enemy)
    {
        if (IsGameOver) return;
        ShowGameOver(false);
    }

    private void ShowGameOver(bool isWin)
    {
        IsGameOver = true;

        // Stop all spawning
        if (ObjectPooler.Instance != null)
        {
            ObjectPooler.Instance.StopSpawning();
        }
        
        // Disable any spawners
        var spawners = FindObjectsOfType<MonoBehaviour>();
        foreach (var spawner in spawners)
        {
            if (spawner.GetType().Name.Contains("Spawner"))
            {
                spawner.enabled = false;
            }
        }

        // Show UI
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

        DeactivateAllEnemies();
    }

    private void DeactivateAllEnemies()
    {
        foreach (Enemy enemy in new List<Enemy>(activeEnemies))
        {
            if (enemy != null)
            {
                enemy.Die();
            }
        }
        activeEnemies.Clear();
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
        if (gameplayContainer != null) gameplayContainer.SetActive(true);

        // Reset game state
        InitializeGame();

        // Resume spawning
        if (ObjectPooler.Instance != null)
        {
            ObjectPooler.Instance.ResumeSpawning();
        }

        // Re-enable spawners
        var spawners = FindObjectsOfType<MonoBehaviour>();
        foreach (var spawner in spawners)
        {
            if (spawner.GetType().Name.Contains("Spawner"))
            {
                spawner.enabled = true;
            }
        }

        // Reset score and currency
        score = 0;
        Currency = startingCurrency;
        UpdateScoreUI();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    public void AddCurrency(int amount)
    {
        Currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (Currency >= amount)
        {
            Currency -= amount;
            return true;
        }
        return false;
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void UpdateCurrencyUI()
    {
        if (currencyText != null)
        {
            currencyText.text = "Currency: " + Currency;
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        // Pause the game or stop spawning
        Time.timeScale = 0f;
    }
}
