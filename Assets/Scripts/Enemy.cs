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
    }

    private void Update()
    {
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