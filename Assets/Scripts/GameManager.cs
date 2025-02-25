using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject victoryPanel;   // Reference to the Victory UI panel
    public GameObject gameOverPanel;  // Reference to the Game Over UI panel

    private int totalEnemies;         // Total number of enemies spawned
    private bool gameOver = false;    // To track if the game is over

    public static GameManager Instance; // Singleton reference
    private ObjectPooler objectPooler;  // Reference to the ObjectPooler

    private void Awake()
    {
        Instance = this; // Set the singleton reference
        objectPooler = FindObjectOfType<ObjectPooler>(); // Find the ObjectPooler in the scene
    }

    void Start()
    {
        // Initially, set both panels to inactive
        victoryPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // Set the total number of enemies (this can be dynamically assigned in your game)
        totalEnemies = 10;  // Example number of enemies
    }

    void OnEnable()
    {
        // Subscribe to the EnemyReachedEnd event
        Enemy.OnEndReached += EnemyReachedEnd;
    }

    void OnDisable()
    {
        // Unsubscribe from the event
        Enemy.OnEndReached -= EnemyReachedEnd;
    }

    // This method is called when an enemy reaches the end point
    public void EnemyReachedEnd(Enemy enemy)
    {
        if (!gameOver)
        {
            ShowGameOver(); // Trigger game-over screen if an enemy reaches the end
        }
    }

    // Call this method when an enemy is destroyed
    public void EnemyDestroyed()
    {
        // Check if there are no active enemies left
        if (objectPooler.GetActiveEnemyCount() <= 0 && !gameOver)
        {
            ShowVictory(); // Show victory if no enemies are left
        }
    }

    // Show Victory UI
    private void ShowVictory()
    {
        victoryPanel.SetActive(true);
        gameOver = true;  // Prevent further game-over or victory triggers
    }

    // Show Game Over UI
    private void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        gameOver = true;  // Prevent further game-over or victory triggers
    }
}
