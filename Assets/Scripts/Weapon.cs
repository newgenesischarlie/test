using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private float _range = 5f;
    [SerializeField] private int _damage = 10;
    [SerializeField] private string projectileTag = "Projectile";
    [SerializeField] private Transform firePoint;

    public float fireRate { 
        get { return _fireRate; } 
        set { _fireRate = value; } 
    }
    
    public float range { 
        get { return _range; } 
        set { _range = value; } 
    }
    
    public int damage { 
        get { return _damage; } 
        set { _damage = value; } 
    }

    private float nextFireTime;
    private ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        nextFireTime = Time.time;
    }

    private void Update()
    {
        if (Time.time >= nextFireTime)
        {
            TryFire();
        }
    }

    private void TryFire()
    {
        // Find a target
        Enemy target = FindNearestEnemy();
        if (target != null)
        {
            Fire(target.transform);
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private Enemy FindNearestEnemy()
    {
        // Find all active enemies manually instead of using GetAllActiveEnemies
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Enemy nearestEnemy = null;
        float nearestDistance = range;

        foreach (Enemy enemy in enemies)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= range && distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }

        return nearestEnemy;
    }

    private void Fire(Transform target)
    {
        if (objectPooler == null || firePoint == null) return;

        // Use GetInstanceFromPool instead of GetPooledObject
        GameObject projectileObj = objectPooler.GetInstanceFromPool(projectileTag);
        if (projectileObj != null)
        {
            projectileObj.transform.position = firePoint.position;
            projectileObj.transform.rotation = firePoint.rotation;
            
            // Use BasicProjectile instead of Projectile
            BasicProjectile projectile = projectileObj.GetComponent<BasicProjectile>();
            if (projectile != null)
            {
                Vector3 direction = (target.position - firePoint.position).normalized;
                projectile.SetDirection(direction);
            }
            
            // Or if using TurretProjectile
            TurretProjectile turretProjectile = projectileObj.GetComponent<TurretProjectile>();
            if (turretProjectile != null)
            {
                turretProjectile.Initialize(target);
            }
        }
    }
}
