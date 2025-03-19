using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private float _range = 5f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private int _damage = 10;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    
    [Header("Rotation Constraints")]
    [SerializeField] private float minRotationAngle = -45f;
    [SerializeField] private float maxRotationAngle = 45f;
    [SerializeField] private Transform pivotPoint; // Optional pivot point for rotation

    [Header("Targeting Waypoints")]
    [SerializeField] private Transform targetWaypoint; // Waypoint to rotate towards

    // Public properties
    public float range { get { return _range; } set { _range = value; } }
    public float rotationSpeed { get { return _rotationSpeed; } set { _rotationSpeed = value; } }
    public float fireRate { get { return _fireRate; } set { _fireRate = value; } }
    public int damage { get { return _damage; } set { _damage = value; } }

    private float nextFireTime;
    private Transform currentTarget;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    
    private void Awake()
    {
        // Start inactive
        gameObject.SetActive(false);  // Start weapon inactive

        if (pivotPoint == null)
            pivotPoint = transform;

        if (targetWaypoint == null)
            Debug.LogWarning("No target waypoint assigned to the weapon!");
    }

    private void Start()
    {
        nextFireTime = Time.time;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Update()
{
    if (!gameObject.activeInHierarchy) return; // Skip if inactive

    // Rotate towards waypoint once the weapon is placed
    if (targetWaypoint != null)
    {
        RotateTowardsWaypoint();  // Rotate toward the waypoint
    }

    // Find and target enemies
    FindAndTargetEnemy();

    if (currentTarget != null)
    {
        TryShoot();  // Try shooting if the target exists
    }

    // Keep weapon at its initial position
    if (transform.position != initialPosition)
    {
        transform.position = initialPosition;
    }
}


    private void FindAndTargetEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float closestDistance = range;
        Transform closestEnemy = null;

        foreach (Enemy enemy in enemies)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= range && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }
        }

        currentTarget = closestEnemy;
    }

    private void RotateTowardsWaypoint()
    {
        if (targetWaypoint == null) return;

        // Calculate direction to waypoint
        Vector3 direction = targetWaypoint.position - transform.position;

        // Calculate rotation angle
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply constraints - convert to local space if needed
        float baseAngle = initialRotation.eulerAngles.z;
        float relativeTurretAngle = Mathf.DeltaAngle(baseAngle, targetAngle);

        // Clamp to min/max range
        float clampedAngle = Mathf.Clamp(relativeTurretAngle, minRotationAngle, maxRotationAngle);
        float finalAngle = baseAngle + clampedAngle;

        // Create target rotation (only rotate around Z axis)
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, finalAngle);

        // Rotate smoothly towards target
        transform.rotation = Quaternion.Lerp(
            transform.rotation, 
            targetRotation, 
            rotationSpeed * Time.deltaTime
        );
    }

    private void TryShoot()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void Shoot()
    {
        if (firePoint == null || currentTarget == null || projectilePrefab == null)
            return;

        // Create a projectile
        GameObject projectileObj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        
        if (projectileObj != null)
        {
            // Initialize the projectile
            Projectile projectile = projectileObj.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.Initialize(damage, currentTarget);
            }
        }
    }

    // Upgrade methods
    public void UpgradeDamage(float multiplier = 1.2f)
    {
        damage = Mathf.RoundToInt(damage * multiplier);
    }

    public void UpgradeRange(float multiplier = 1.15f)
    {
        range *= multiplier;
    }

    public void UpgradeFireRate(float multiplier = 1.1f)
    {
        fireRate *= multiplier;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
        
        // Draw rotation constraints
        if (Application.isPlaying) return;
        
        Vector3 center = pivotPoint != null ? pivotPoint.position : transform.position;
        float radius = 1f; // Visual radius for gizmo
        
        Gizmos.color = Color.yellow;
        Vector3 minDir = Quaternion.Euler(0, 0, minRotationAngle) * Vector3.right;
        Vector3 maxDir = Quaternion.Euler(0, 0, maxRotationAngle) * Vector3.right;
        Gizmos.DrawRay(center, minDir * radius);
        Gizmos.DrawRay(center, maxDir * radius);
    }
}
