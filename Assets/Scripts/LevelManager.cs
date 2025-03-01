using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
}

public class LevelManager : Singleton<LevelManager>
{
    // Expose these fields in the Inspector
  //  [SerializeField] private int instanceID;
   // [SerializeField] private string localIdentifierInFile;
    [SerializeField] private int lives = 10;
    [SerializeField] private int totalLives;
    [SerializeField] private int currentWave;

    public int TotalLives
    {
        get { return totalLives; }
        set { totalLives = value; }
    }

    public int CurrentWave
    {
        get { return currentWave; }
        set { currentWave = value; }
    }

   // public int InstanceID
  //  {
    //    get { return instanceID; }
    //    set { instanceID = value; }
   // }

  //  public string LocalIdentifierInFile
  //  {
  //      get { return localIdentifierInFile; }
   //     set { localIdentifierInFile = value; }
  //  }

    private void Start()
    {
        // Initialize with default or serialized values
        totalLives = lives;
        currentWave = 1;
  //      instanceID = System.Guid.NewGuid().GetHashCode();  // Generate a unique instance ID (for example)
 //       localIdentifierInFile = "level_" + instanceID; // Just an example, can be customized
    }

    private void ReduceLives(Enemy enemy)
    {
        totalLives--;
        if (totalLives <= 0)
        {
            totalLives = 0;
            GameOver();
        }
    }

    private void GameOver()
    {
        // Handle Game Over logic here (e.g., show game over screen)
        Debug.Log("Game Over! You lost all your lives.");

        // Example: Pause the game, show a game over UI, etc.
        Time.timeScale = 0f; // Freeze the game (optional)
    }

  //  private void WaveCompleted()
  //  {
        // Handle wave completion logic here (e.g., increase wave count, spawn new enemies)
  //      Debug.Log("Wave " + currentWave + " completed!");

        // Example: Increase the wave count and start the next wave
    //    currentWave++;
        // Optionally, you can spawn more enemies or adjust difficulty, etc.
   // }

    private void Awake()
    {
        Enemy.OnEndReached += HandleEnemyReachedEnd;
    }
    
    private void OnDestroy()
    {
        Enemy.OnEndReached -= HandleEnemyReachedEnd;
    }
    
    private void HandleEnemyReachedEnd(Enemy enemy)
    {
        // Your level manager specific handling
    }

    public string sceneName;

    public void changeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
