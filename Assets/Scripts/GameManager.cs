using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen; // Reference to your game over screen UI
    public GameObject gameplayUI;     // Reference to gameplay UI (if you have one)

    private void Start()
    {
        // Initially hide the game over screen at the start of the game
        gameOverScreen.SetActive(false);
    }

    public void HandleEndReached(Enemy enemy)
    {
        // Log when the enemy reaches the endpoint
        Debug.Log("Endpoint Reached by: " + enemy.name);

        // Show the game over screen when the enemy reaches the endpoint
        ShowGameOverScreen();
    }

    private void ShowGameOverScreen()
    {
        // Log to check if the function is being called
        Debug.Log("Showing Game Over Screen");

        // Show the game over screen
        gameOverScreen.SetActive(true);

        // Optionally disable gameplay UI if you have one
        if (gameplayUI != null)
        {
            gameplayUI.SetActive(false);
        }

        // Disable other gameplay-related components if necessary
        // Example: Stop gameplay mechanics like player movement, etc.
    }
}
