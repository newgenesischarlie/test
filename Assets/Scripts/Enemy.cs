using System;
using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    #region Events
    public static event Action<Enemy> OnEndReached;
    #endregion

    #region Serialized Fields
    [Header("Setup")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> enemySprites = new List<Sprite>();
    [SerializeField] private int maxHealth = 100;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private List<Vector3> waypoints = new List<Vector3>();
    #endregion

    #region Component References
    private GameManager gameManager;
    private EnemyHealth enemyHealth;
    #endregion

    #region State
    private int currentHealth;
    private int currentWaypointIndex;
    private bool isDead = false;
    public int CurrentWaveIndex { get; set; }
    #endregion

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        ResetEnemy();
    }

    private void OnEnable()
    {
        isDead = false;
        ResetEnemy();
    }

    private void OnDisable()
    {
        if (gameManager != null)
        {
            UnsubscribeFromEvents();
        }
    }

    private void Update()
    {
        if (isDead || gameManager == null) return;
        if (gameManager.IsGameOver) return;
        if (waypoints == null || waypoints.Count == 0) return;
        MoveAlongPath();
    }

    private bool InitializeComponents()
    {
        try
        {
            enemyHealth = GetComponent<EnemyHealth>();
            gameManager = FindObjectOfType<GameManager>();

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

    public void ResetEnemy()
    {
        currentWaypointIndex = 0;
        currentHealth = maxHealth;
        SetupSprite();
        
        if (waypoints != null && waypoints.Count > 0)
        {
            transform.position = waypoints[0];
        }
    }

    private void SetupSprite()
    {
        if (spriteRenderer != null && enemySprites.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, enemySprites.Count);
            spriteRenderer.sprite = enemySprites[randomIndex];
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
        if (!gameManager.IsGameOver)
        {
            isDead = true;
            OnEndReached?.Invoke(this);
            gameManager.HandleEnemyDefeated(this);
            ObjectPooler.ReturnToPool(gameObject);
        }
    }

    private void MoveAlongPath()
    {
        if (waypoints == null || waypoints.Count == 0 || currentWaypointIndex >= waypoints.Count)
            return;

        Vector3 targetPosition = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentWaypointIndex++;
            
            if (currentWaypointIndex >= waypoints.Count)
            {
                OnEndReached?.Invoke(this);
                gameObject.SetActive(false);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        
        if (OnEndReached != null)
        {
            OnEndReached(this);
        }
        
        gameObject.SetActive(false);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    public void SetWaypoints(List<Vector3> newWaypoints)
    {
        waypoints = new List<Vector3>(newWaypoints);
        currentWaypointIndex = 0;
        
        if (waypoints.Count > 0)
        {
            transform.position = waypoints[0];
        }
    }
}

