using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Make this event static so it can be subscribed to without an instance
    public static event Action<Enemy> OnEndReached;

<<<<<<< HEAD
    #region Serialized Fields
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private List<Sprite> enemySprites = new List<Sprite>();
    [SerializeField] private List<Vector3> waypoints = new List<Vector3>();
    #endregion

    #region Component References
    private GameManager gameManager;
    private EnemyHealth enemyHealth;
    private SpriteRenderer spriteRenderer;
    private ObjectPooler objectPooler;
    #endregion

    #region State
    private int currentWaypointIndex;
    public int CurrentWaveIndex { get; set; }
    private bool isInitialized;
    #endregion

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        InitializeComponents();
        SetupEnemy();
    }

    private void InitializeComponents()
    {
        try
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            enemyHealth = GetComponent<EnemyHealth>();
            gameManager = FindObjectOfType<GameManager>();

            if (spriteRenderer == null)
            {
                throw new MissingComponentException("SpriteRenderer not found on enemy");
            }
            if (enemyHealth == null)
            {
                throw new MissingComponentException("EnemyHealth not found on enemy");
            }
            if (gameManager == null)
            {
                throw new MissingComponentException("GameManager not found in scene");
            }
            if (waypoints.Count == 0)
            {
                throw new InvalidOperationException("No waypoints assigned to enemy");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[Enemy] Initialization failed on {gameObject.name}: {e.Message}");
        }
    }

    public void SetupEnemy()
    {
        try
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (enemySprites != null && enemySprites.Count > CurrentWaveIndex && spriteRenderer != null)
            {
                spriteRenderer.sprite = enemySprites[CurrentWaveIndex];
            }
            else
            {
                Debug.LogWarning($"[Enemy] Missing required components or sprites for wave {CurrentWaveIndex}");
            }
            
            if (enemyHealth != null)
            {
                enemyHealth.ResetHealth();
            }
            
            currentWaypointIndex = 0;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[Enemy] Setup failed: {e.Message}");
        }
    }

    private void OnEnable()
    {
        ResetEnemy();
=======
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
>>>>>>> parent of d0bcb54 (fixedgameover)
    }

    private void Update()
    {
<<<<<<< HEAD
        if (waypoints == null || waypoints.Count == 0) return;
        MoveToNextWaypoint();
    }

    private void MoveToNextWaypoint()
    {
        if (currentWaypointIndex >= waypoints.Count) return;

        Vector3 targetPosition = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentWaypointIndex++;
            
            if (currentWaypointIndex >= waypoints.Count)
            {
                ReachedEndPoint();
            }
        }
    }

    private void ReachedEndPoint()
    {
        OnEndReached?.Invoke(this);
        if (gameManager != null)
        {
            gameManager.HandleEndReached(this);
        }
        gameObject.SetActive(false);
    }

    public void ResetEnemy()
    {
        currentWaypointIndex = 0;
        if (waypoints != null && waypoints.Count > 0)
        {
            transform.position = waypoints[0];
        }
    }

    public void Die()
    {
        OnEnemyDefeated?.Invoke(this);
        ObjectPooler.ReturnToPool(gameObject);
    }

    // Method to set waypoints from outside
    public void SetWaypoints(List<Vector3> newWaypoints)
    {
        waypoints = new List<Vector3>(newWaypoints);
        ResetEnemy();
    }

    // Optional: Visual debug of waypoints
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Count == 0) return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
            Gizmos.DrawWireSphere(waypoints[i], 0.2f);
        }
        Gizmos.DrawWireSphere(waypoints[waypoints.Count - 1], 0.2f);
    }
}
=======
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
>>>>>>> parent of d0bcb54 (fixedgameover)
