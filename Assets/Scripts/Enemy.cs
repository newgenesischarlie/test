using UnityEngine;
using System;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    #region Events
    public static event Action<Enemy> OnEndReached;
    public static event Action<Enemy> OnEnemyDefeated;
    #endregion

    #region Serialized Fields
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private List<Sprite> enemySprites = new List<Sprite>();
    [SerializeField] private List<Vector3> waypoints = new List<Vector3>();
    #endregion

    #region Component References
    private GameManager gameManager;
    private EnemyHealth enemyHealth;
    private SpriteRenderer spriteRenderer;
    #endregion

    #region State
    private int currentWaypointIndex;
    private bool isInitialized;
    public int CurrentWaveIndex { get; set; }
    #endregion

    private void Start()
    {
        isInitialized = InitializeComponents();
        if (isInitialized)
        {
            SetupEnemy();
            SubscribeToEvents();
        }
    }

    private void OnEnable()
    {
        if (gameManager != null)
        {
            OnEndReached += gameManager.HandleEndReached;
        }
    }

    private void OnDisable()
    {
        if (gameManager != null)
        {
            OnEndReached -= gameManager.HandleEndReached;
            UnsubscribeFromEvents();
        }
    }

    private void Update()
    {
        if (!isInitialized || waypoints.Count == 0) return;

        Move();
        Rotate();
        
        if (Vector3.Distance(transform.position, CurrentWaypoint) < 0.1f)
        {
            UpdateWaypoint();
        }
    }

    private bool InitializeComponents()
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

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"[Enemy] Initialization failed on {gameObject.name}: {e.Message}");
            return false;
        }
    }

    private void SetupEnemy()
    {
        try
        {
            if (enemySprites.Count > CurrentWaveIndex)
            {
                spriteRenderer.sprite = enemySprites[CurrentWaveIndex];
            }
            else
            {
                Debug.LogWarning($"[Enemy] No sprite available for wave {CurrentWaveIndex}");
            }
            
            enemyHealth.ResetHealth();
            currentWaypointIndex = 0;
        }
        catch (Exception e)
        {
            Debug.LogError($"[Enemy] Setup failed on {gameObject.name}: {e.Message}");
            isInitialized = false;
        }
    }

    private void SubscribeToEvents()
    {
        if (enemyHealth != null)
        {
            // Monitor the enemy's health for changes
            enemyHealth.OnDeath += HandleEnemyDefeated;
        }
    }

    private void UnsubscribeFromEvents()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath -= HandleEnemyDefeated;
        }
    }

    private void HandleEnemyDefeated()
    {
        if (!gameManager.isGameOver)
        {
            OnEnemyDefeated?.Invoke(this);
            ObjectPooler.ReturnToPool(gameObject);
        }
    }

    private Vector3 CurrentWaypoint
    {
        get
        {
            if (currentWaypointIndex >= waypoints.Count)
            {
                Debug.LogError($"[Enemy] Waypoint index out of range: {currentWaypointIndex}");
                return transform.position;
            }
            return waypoints[currentWaypointIndex];
        }
    }

    private void Move()
    {
        if (gameManager.isGameOver) return;
        
        transform.position = Vector3.MoveTowards(
            transform.position, 
            CurrentWaypoint, 
            moveSpeed * Time.deltaTime
        );
    }

    private void Rotate()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = transform.position.x > CurrentWaypoint.x;
        }
    }

    private void UpdateWaypoint()
    {
        if (currentWaypointIndex < waypoints.Count - 1)
        {
            currentWaypointIndex++;
            return;
        }
        
        try
        {
            if (!gameManager.isGameOver)
            {
                OnEndReached?.Invoke(this);
            }
            ObjectPooler.ReturnToPool(gameObject);
        }
        catch (Exception e)
        {
            Debug.LogError($"[Enemy] Error during endpoint handling: {e.Message}");
        }
    }
}