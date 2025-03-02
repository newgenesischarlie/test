using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f; // Speed at which turret rotates
    [SerializeField] private float attackRange = 5f; // Detection range for enemies
    [SerializeField] private Transform rotationPoint; // The point from which the turret rotates
    [SerializeField] private float attackRate = 1f; // How fast the turret shoots (attacks per second)
    [SerializeField] private GameObject projectilePrefab; // The projectile the turret fires
    [SerializeField] private Transform projectileSpawnPoint; // Where the projectile is spawned

    private Transform currentTarget;
    private float nextAttackTime = 0f;

    private void Start()
    {
        // Ensure the weapon starts inactive and does not update while inactive
        gameObject.SetActive(false); // We ensure the weapon is initially inactive
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy) return; // Skip update if the weapon is inactive

        DetectEnemies();

        if (currentTarget != null && Time.time >= nextAttackTime)
        {
            ShootProjectile();
            nextAttackTime = Time.time + attackRate;
        }

        RotateTowardsTarget();
    }

    // Detect enemies within range
    private void DetectEnemies()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (var enemy in enemiesInRange)
        {
            if (enemy.CompareTag("Enemy"))
            {
                currentTarget = enemy.transform;
                return;
            }
        }

        currentTarget = null; // No enemies in range
    }

    // Rotate the turret to face the enemy
    private void RotateTowardsTarget()
    {
        if (currentTarget != null)
        {
            Vector3 direction = currentTarget.position - rotationPoint.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rotationPoint.rotation = Quaternion.RotateTowards(rotationPoint.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);
        }
    }

    // Shoot a projectile at the target
    private void ShootProjectile()
    {
        if (currentTarget != null)
        {
            GameObject projectile = ObjectPooler.Instance.GetInstanceFromPool();
            projectile.transform.position = projectileSpawnPoint.position;
            projectile.GetComponent<Projectile>().SetEnemy(currentTarget.GetComponent<Enemy>());
            projectile.SetActive(true);
        }
    }

    // Draw a circle in the editor to visualize attack range (for debugging)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
