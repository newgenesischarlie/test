using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float maxLifetime = 5f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Vector3 respawnPosition = Vector3.zero; // Optional: Set this as the respawn point

    private int damage;
    private Transform target;
    private Vector3 direction;
    private float lifetimeTimer;
    public bool hasHitTarget = false;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(int damageAmount, Transform enemyTarget)
    {
        damage = damageAmount;
        target = enemyTarget;
        lifetimeTimer = 0f;
        hasHitTarget = false;

        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
        }
        else
        {
            direction = transform.right;
        }

        // Show projectile
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
    }

    private void Update()
    {
        if (hasHitTarget) return;

        // If target is gone, keep moving in last direction
        if (target != null && target.gameObject.activeInHierarchy)
        {
            direction = (target.position - transform.position).normalized;
        }

        // Move projectile
        transform.Translate(direction * speed * Time.deltaTime);

        // Rotate to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Check if projectile is out of screen bounds
        if (IsOutOfBounds())
        {
            RespawnProjectile(); // Respawn the projectile
        }
    }

    private bool IsOutOfBounds()
    {
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        return screenPos.x < -0.1f || screenPos.x > 1.1f || screenPos.y < -0.1f || screenPos.y > 1.1f;
    }

    private void RespawnProjectile()
    {
        // Reset position to the spawn point
        transform.position = respawnPosition;

        // Optionally, you could add a delay before respawning the projectile
        hasHitTarget = false; // Reset hit target flag
        lifetimeTimer = 0f; // Reset lifetime timer
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHitTarget) return;

        if (other.CompareTag("Enemy"))
        {
            // Get the Enemy component from the collider
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                // Call the TakeDamage method from the Enemy class to deal damage
                enemy.TakeDamage(damage);

                // Optionally trigger the enemy's FX (like damage effects)
                EnemyFX enemyFX = other.GetComponent<EnemyFX>();
                if (enemyFX != null)
                {
                    enemyFX.OnEnemyHit(damage);
                }
            }

            hasHitTarget = true;
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject); // Destroy the projectile
    }
}
