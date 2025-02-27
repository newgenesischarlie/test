using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject loseScreen;
    public Vector3 finalWaypointPosition; // Position of the final waypoint
    private int remainingEnemies;
    private ObjectPooler objectPooler;

    void Start()
    {
        objectPooler = ObjectPooler.Instance; // Get reference to the ObjectPooler
        remainingEnemies = objectPooler.GetTotalEnemiesInScene();

        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    void Update()
    {
        CheckGameStatus();
    }

    void CheckGameStatus()
    {
        // Check if all enemies are destroyed
        if (remainingEnemies <= 0)
        {
            WinGame();
            return;
        }

        // Check if any enemy has reached the final waypoint
        foreach (var enemy in objectPooler.GetAllActiveEnemies())
        {
            if (enemy != null && Vector3.Distance(enemy.transform.position, finalWaypointPosition) < 1f)
            {
                LoseGame();
                break;
            }
        }
    }

    public void OnEnemyDestroyed()
    {
        remainingEnemies--;
    }

    void WinGame()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0; // Stop the game time
    }

    void LoseGame()
    {
        loseScreen.SetActive(true);
        Time.timeScale = 0; // Stop the game time
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        Time.timeScale = 1; // Resume game time
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
