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

    // Sprite handling
    private SpriteRenderer _spriteRenderer;

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
        enemyHealth = GetComponent<EnemyHealth>();

        // Check if the enemyHealth component exists
        if (enemyHealth == null)
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
        enemyHealth.ResetHealth();

        // Check if the gameManager is assigned and find it if not
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        if (gameManager == null)
        {
            Debug.LogError("GameManager reference is missing! Make sure it is assigned.");
        }

        // Subscribe to the OnEndReached event
        SubscribeToEndEvent();
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event when the enemy is destroyed
        UnsubscribeFromEndEvent();
    }

    private void SubscribeToEndEvent()
    {
        // Subscribe to the static event
        if (OnEndReached != null)
        {
            OnEndReached += gameManager.HandleEndReached;  // Ensure gameManager is listening to this event
        }
    }

    private void UnsubscribeFromEndEvent()
    {
        // Unsubscribe when the enemy is destroyed
        if (OnEndReached != null)
        {
            OnEndReached -= gameManager.HandleEndReached;  // Ensure gameManager is no longer listening
        }
    }

    private void Update()
    {
        if (gameManager == null || enemyHealth == null)
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

            // Correctly invoke the static event using the class name (Enemy)
            Enemy.OnEndReached?.Invoke(this); // Invoke the static event correctly

            // Reset health after reaching the endpoint
            enemyHealth.ResetHealth();

            // Optionally, return the enemy to the pool (if ObjectPooler is available)
            ObjectPooler.ReturnToPool(gameObject);
        }
        else
        {
            Debug.LogError("GameManager is null when reaching endpoint!");
        }
    }
}
