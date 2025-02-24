using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject victoryPanel;  // Reference to the Victory UI panel
    public GameObject gameOverPanel; // Reference to the Game Over UI panel

    private int totalEnemies;       // Total number of enemies spawned
    private int remainingEnemies;   // Remaining number of enemies
    private bool gameOver = false;  // To track if the game is over

    void Start()
    {
        // Initially, set both panels to inactive
        victoryPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // Assume you have a way to track total enemies; for now, let's say it's set manually.
        totalEnemies = 10;  // Example number of enemies
        remainingEnemies = totalEnemies;
    }

    // Call this method when an enemy is destroyed
    public void EnemyDefeated()
    {
        remainingEnemies--;

        // If there are no enemies left, player wins
        if (remainingEnemies <= 0 && !gameOver)
        {
            ShowVictory();
        }
    }

    // Call this method when an enemy reaches the final waypoint
    public void EnemyReachedEnd()
    {
        if (!gameOver)
        {
            ShowGameOver();
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