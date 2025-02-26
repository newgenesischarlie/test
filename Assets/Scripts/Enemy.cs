using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Make this event static so it can be subscribed to without an instance
    public static event Action<Enemy> OnEndReached;

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

        // Set the sprite based on the wave index
        if (EnemySprites.Count > 0 && CurrentWaveIndex < EnemySprites.Count)
        {
            _spriteRenderer.sprite = EnemySprites[CurrentWaveIndex];
        }

        // Initialize health and waypoints
        _enemyHealth.ResetHealth();
    }

    internal void Initialize(Enemy enemy)
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
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

    private void EndPointReached()
    {
        // When the enemy reaches the end point or is defeated, remove from the list
        WaveManager.Instance.RemoveEnemyFromList(this);
        OnEndReached?.Invoke(this);
        _enemyHealth.ResetHealth();
        ObjectPooler.ReturnToPool(gameObject);
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
}
