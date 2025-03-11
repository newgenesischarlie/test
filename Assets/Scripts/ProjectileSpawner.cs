using System.Collections;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private Transform spawnPoint;  // The point where the projectile will spawn
    [SerializeField] private float spawnRate = 1f;   // Rate at which the projectile spawns
    [SerializeField] private float projectileSpeed = 10f;  // Speed at which the projectile travels

    private ObjectPooler _objectPooler;
    
    void Start()
    {
        // Get the ObjectPooler instance
        _objectPooler = ObjectPooler.Instance;

        // Start spawning projectiles at regular intervals
        StartCoroutine(SpawnProjectiles());
    }

    // Coroutine to spawn projectiles at a set interval
    private IEnumerator SpawnProjectiles()
    {
        while (true)
        {
            // Get a projectile from the pool
            GameObject projectile = _objectPooler.GetInstanceFromPool();

            if (projectile != null)
            {
                // Position the projectile at the spawn point
                projectile.transform.position = spawnPoint.position;
                projectile.transform.rotation = spawnPoint.rotation;

                // Activate and move the projectile (if necessary, you can add behavior for the projectile like moving forward)
                projectile.SetActive(true);

                // Add a Rigidbody or Rigidbody2D for movement (if you're doing 3D or 2D projectiles)
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Add a force to make the projectile move in the forward direction
                    rb.velocity = spawnPoint.forward * projectileSpeed;
                }

                // Optionally, you can add logic here to destroy the projectile after it leaves the screen or after a certain time
                StartCoroutine(ReturnProjectileToPoolAfterDelay(projectile, 5f)); // Example: return projectile after 5 seconds
            }

            // Wait for the next spawn
            yield return new WaitForSeconds(spawnRate);
        }
    }

    // Coroutine to return the projectile to the pool after a delay
    private IEnumerator ReturnProjectileToPoolAfterDelay(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Return the projectile to the pool
        ObjectPooler.ReturnToPool(projectile);
    }
}
