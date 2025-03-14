using System;
using UnityEngine;
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
    [SerializeField] private EnemyFX enemyFX;  // Reference to the EnemyFX component
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

    // TakeDamage function
  public void TakeDamage(int damageAmount)
{
    if (enemyHealth != null)
    {
        // Call ReduceHealth instead of TakeDamage
        enemyHealth.ReduceHealth(damageAmount);  // Calls the ReduceHealth method

        // Notify the EnemyFX component to show the damage effect when the enemy is hit
        if (enemyFX != null)
        {
            enemyFX.OnEnemyHit(damageAmount);
        }

        // Check if the enemy's health has dropped to 0 or below
        if (enemyHealth.CurrentHealth <= 0)
        {
            enemyHealth.Die();  // Call Die when health reaches 0
        }
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
        }
        UnsubscribeFromEvents();
    }

    private void Update()
    {
        if (!isInitialized || waypoints.Count == 0 || gameManager.isGameOver) return;

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

            // Initialize the enemyFX component, make sure it's assigned in the Inspector
            if (enemyFX == null)
            {
                enemyFX = GetComponent<EnemyFX>();
            }

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
        if (gameManager.isGameOver) return;

        // Trigger the win screen if enemy is defeated
        OnEnemyDefeated?.Invoke(this);
        gameManager.HandleEnemyDefeated(this);
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
            ObjectPooler.ReturnToPool(gameObject); // Return the enemy to the pool once it reaches the end
        }
        catch (Exception e)
        {
            Debug.LogError($"[Enemy] Error during endpoint handling: {e.Message}");
        }
    }
}
