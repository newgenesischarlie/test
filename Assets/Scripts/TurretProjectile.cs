using UnityEngine;

// Rename to avoid conflict with Projectile class
public class TurretProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private string targetTag = "Enemy";

    private Transform target;
    private Vector3 direction;
    private float timer;

    private void OnEnable()
    {
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            ReturnToPool();
            return;
        }

        if (target != null && target.gameObject.activeInHierarchy)
        {
            // Update direction to follow the target
            direction = (target.position - transform.position).normalized;
        }

        // Move in the current direction
        transform.position += direction * speed * Time.deltaTime;
    }

    public void Initialize(Transform target)
    {
        this.target = target;
        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
        }
        else
        {
            direction = transform.forward;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
}
