using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Make this event static so it can be subscribed to without an instance
    public static event Action<Enemy> OnEndReached;

    // Reference to the GameManager
    public GameManager gameManager;

    // EnemyHealth script reference
    public EnemyHealth enemyHealth;

    // Movement speed
    public float MoveSpeed = 3f;

    // Waypoint system
    private int _currentWaypointIndex = 0;
    private Vector3 _lastPointPosition;

    // Sprite handling
    private SpriteRenderer _spriteRenderer;
    private EnemyHealth _enemyHealth;

    // List of waypoints
    public List<Vector3> Waypoints;

    // Reference to the different sprites for different enemy waves
    public List<Sprite> EnemySprites; // List of sprites to be used for different waves

    // Variable to track the current wave
    public int CurrentWaveIndex = 0;

    private void Start()
    {
        // Initialize components
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyHealth = GetComponent<EnemyHealth>();

        // Check if the enemyHealth component exists
        if (_enemyHealth == null)
        {
            Debug.LogError("EnemyHealth component is missing on this enemy!");
            return;
        }

        // Set the sprite based on the wave index
        if (EnemySprites.Count > 0 && CurrentWaveIndex < EnemySprites.Count)
        {
            _spriteRenderer.sprite = EnemySprites[CurrentWaveIndex];
        }

        // Initialize health and waypoints
        _enemyHealth.ResetHealth();

        // Check if the gameManager is assigned and find it if not
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        if (gameManager == null)
        {
            Debug.LogError("GameManager reference is missing! Make sure it is assigned.");
        }
    }

    internal void Initialize(Enemy enemy)
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        if (gameManager == null || _enemyHealth == null)
        {
            // If gameManager or enemyHealth is null, we should stop updating and return.
            return;
        }

        Move();
        Rotate();
        if (CurrentPointPositionReached())
        {
            UpdateCurrentPointIndex();
        }
    }

    private void Move()
    {
        // Move the enemy towards the current waypoint
        transform.position = Vector3.MoveTowards(transform.position, CurrentPointPosition, MoveSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        // Flip sprite based on movement direction
        if (transform.position.x < CurrentPointPosition.x)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }
    }

    private Vector3 CurrentPointPosition
    {
        get
        {
            if (Waypoints.Count > 0)
            {
                return Waypoints[_currentWaypointIndex];
            }
            return transform.position; // Default to current position if no waypoints
        }
    }

    private bool CurrentPointPositionReached()
    {
        float distanceToNextPointPosition = (transform.position - CurrentPointPosition).magnitude;
        return distanceToNextPointPosition < 0.1f;
    }

    private void UpdateCurrentPointIndex()
    {
        int lastWaypointIndex = Waypoints.Count - 1;
        if (_currentWaypointIndex < lastWaypointIndex)
        {
            _currentWaypointIndex++;
        }
        else
        {
            EndPointReached();
        }
    }
    public void EndPointReached()
    {
        // Check if the GameManager is assigned
        if (gameManager != null)
        {
            Debug.Log("GameManager found. Proceeding with EndPointReached.");

            // Check if the OnEndReached event is subscribed to
            if (OnEndReached != null)
            {
                Debug.Log("Invoking OnEndReached event.");
                OnEndReached.Invoke(this); // This will call HandleEndReached in GameManager
            }
            else
            {
                Debug.LogWarning("OnEndReached event is not subscribed to.");
            }

            // Reset health after reaching the endpoint
            _enemyHealth.ResetHealth();

            // Return the enemy to the pool (if ObjectPooler is available)
            ObjectPooler.ReturnToPool(gameObject);
        }
        else
        {
            Debug.LogError("GameManager is null when reaching endpoint!");
        }

        if (OnEndReached != null)
        {
            OnEndReached.Invoke(this);  // 'this' refers to the current instance of the Enemy
        }
        else
        {
            Debug.LogWarning("OnEndReached event is not subscribed to.");
        }

    }


    // Public method to get the EnemyHealth component
    public EnemyHealth GetEnemyHealth()
    {
        return _enemyHealth;
    }

    // New method to manage spawning waves
    public static void StartNextWave(List<Enemy> enemies)
    {
        // Ensure the wave spawns only after all enemies from the current wave are gone
        if (enemies.Count == 0)
        {
            // Spawn the next wave of enemies here
            // Example: SpawnWave(NextWaveEnemiesList);
        }
    }

    // You can call this method from your Object Pooler or another manager when all enemies in a wave are dead
    public void CheckIfWaveComplete(List<Enemy> allEnemies)
    {
        bool waveComplete = true;

        foreach (var enemy in allEnemies)
        {
            if (enemy.gameObject.activeSelf)  // If any enemy is still active, wave is not complete
            {
                waveComplete = false;
                break;
            }
        }

        if (waveComplete)
        {
            StartNextWave(allEnemies);  // Start the next wave after the current wave is finished
        }
    }

    // Example in the enemy script when the enemy is killed
    public void KillEnemy()
    {
        // Notify the GameManager that this enemy is killed
        if (gameManager != null)
        {
            gameManager.OnEnemyKilled(gameObject);
        }

        // Optionally, you can handle other death-related logic (animations, effects, etc.)
        gameObject.SetActive(false); // Deactivate the enemy
    }
}


