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
    private ObjectPooler objectPooler;
    #endregion

    #region State
    private int currentWaypointIndex;
    public int CurrentWaveIndex { get; set; }
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

    private void Update()
    {
        if (waypoints.Count == 0) return;

        Move();
        Rotate();
        
        if (Vector3.Distance(transform.position, CurrentWaypoint) < 0.1f)
        {
            UpdateWaypoint();
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
        if (gameManager.IsGameOver) return;
        
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
            if (!gameManager.IsGameOver)
            {
                OnEndReached?.Invoke(this);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[Enemy] Error during endpoint handling: {e.Message}");
        }
    }

    public void ResetEnemy()
    {
        currentWaypointIndex = 0;
        if (enemyHealth != null)
        {
            enemyHealth.ResetHealth();
        }
        
        // Set initial sprite if needed
        if (spriteRenderer != null && enemySprites != null && enemySprites.Count > CurrentWaveIndex)
        {
            spriteRenderer.sprite = enemySprites[CurrentWaveIndex];
        }
    }

    public void Die()
    {
        OnEnemyDefeated?.Invoke(this);
        ObjectPooler.ReturnToPool(gameObject);
    }
}