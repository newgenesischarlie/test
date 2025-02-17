using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find the instance in the scene
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    // If the instance doesn't exist, create a new one
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString() + " (Singleton)";
                }
            }
            return _instance;
        }
    }

[SerializeField] private int lives = 10;
    public int TotalLives { get; set; }
    public int CurrentWave { get; set; }

    private void Start()
    {
        TotalLives = lives;
        CurrentWave = 1;
    }

    private void ReduceLives(Enemy enemy)
    {
        TotalLives--;
        if (TotalLives <= 0)
        {
            TotalLives = 0;
            GameOver();
        }
    }

    private void GameOver()
    {
        // Handle Game Over logic here (e.g., show game over screen)
        Debug.Log("Game Over! You lost all your lives.");

        // Example: Pause the game, show a game over UI, etc.
        Time.timeScale = 0f; // Freeze the game (optional)
        // You can also show a Game Over UI here if you have one.
        // GameUI.Instance.ShowGameOverScreen();
    }

    private void WaveCompleted()
    {
        // Handle wave completion logic here (e.g., increase wave count, spawn new enemies)
        Debug.Log("Wave " + CurrentWave + " completed!");

        // Example: Increase the wave count and start the next wave
        CurrentWave++;
        // Optionally, you can spawn more enemies or adjust difficulty, etc.
        // Spawner.Instance.SpawnNextWave(CurrentWave); // Example of spawning the next wave
    }

    private void OnEnable()
    {
        // Subscribe to the static event from the Enemy class
        Enemy.OnEndReached += ReduceLives;

        // Subscribe to the static event from the Spawner class (if it's set up that way)
        Spawner.OnWaveCompleted += WaveCompleted;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        Enemy.OnEndReached -= ReduceLives;
        Spawner.OnWaveCompleted -= WaveCompleted;
    }
}

