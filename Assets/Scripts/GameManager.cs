using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject victoryPanel;  // Reference to the Victory UI panel
    public GameObject gameOverPanel; // Reference to the Game Over UI panel

    private int totalEnemies;       // Total number of enemies spawned
    private int remainingEnemies;   // Remaining number of enemies
    private bool gameOver = false;  // To track if the game is over

    public static GameManager Instance; // Singleton reference

    private void Awake()
    {
        Instance = this; // Set the singleton reference
    }

    void Start()
    {
        // Initially, set both panels to inactive
        victoryPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // Assume you have a way to track total enemies; for now, let's say it's set manually.
        // Set the total number of enemies (this can be dynamically assigned in your game)
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

// Static method to handle an enemy reaching the end (e.g., decrement lives)
public static void EnemyReachedEnd(Enemy enemy)
{
    // Handle logic for when an enemy reaches the end (e.g., decrement lives)
    if (Instance != null)
    {
        Debug.Log("Enemy reached the end!");
        Instance.GameOver();  // Optionally, call game over here if needed
    }
}

    private void GameOver()
    {
        throw new NotImplementedException();
    }

    // Static method for when an enemy is destroyed
    public static void EnemyDestroyed(Enemy enemy)
{
    // Handle logic when an enemy is destroyed
    if (Instance != null)
    {
        Debug.Log("Enemy destroyed!");
        Instance.EnemyDefeated();  // Call to decrease remaining enemies
    }
}
}