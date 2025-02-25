public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEndReached;

    public float MoveSpeed = 3f;
    private int _currentWaypointIndex = 0;
    private Vector3 _lastPointPosition;
    private SpriteRenderer _spriteRenderer;
    private EnemyHealth _enemyHealth; // Reference to the EnemyHealth script
    public delegate void EndReachedHandler(Enemy enemy);

    public List<Vector3> Waypoints;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyHealth = GetComponent<EnemyHealth>(); // Initialize the EnemyHealth script
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
        transform.position = Vector3.MoveTowards(transform.position, CurrentPointPosition, MoveSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
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
            return transform.position;
        }
    }

    private bool CurrentPointPositionReached()
    {
        float distanceToNextPointPosition = (transform.position - CurrentPointPosition).magnitude;
        if (distanceToNextPointPosition < 0.1f)
        {
            _lastPointPosition = transform.position;
            return true;
        }
        return false;
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
        OnEndReached?.Invoke(this); // Notify GameManager that the enemy has reached the end
        GameManager.Instance.EnemyReachedEnd(this); // Calls a method in GameManager to handle game logic
        DestroyEnemy(); // Destroy the enemy after reaching the end
    }

    private void DestroyEnemy()
    {
        _enemyHealth.ResetHealth(); // Reset health when endpoint is reached
        ObjectPooler.ReturnToPool(gameObject); // Return the enemy to the object pool or destroy it
    }

    // New method to return the EnemyHealth component
    public EnemyHealth GetEnemyHealth()
    {
        return _enemyHealth;
    }

    private void OnDestroy()
    {
        // Notify GameManager if the enemy was destroyed or defeated
        GameManager.Instance.EnemyDestroyed(this); // This should notify the GameManager if needed
    }
    public void EnemyDestroyed(Enemy enemy);
    {
        // Implement the method logic here
        // For example:
        Debug.Log("Enemy destroyed: " + enemy.name);
        // You can access enemy properties here, for example:
        // enemy.GetEnemyHealth().ResetHealth();
   }
}
