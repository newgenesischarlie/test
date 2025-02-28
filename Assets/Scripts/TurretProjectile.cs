using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [SerializeField] protected Transform projectileSpawnPosition;
    [SerializeField] protected float delayBtwAttacks = 2f;
    [SerializeField] protected float damage = 2f;

    public float Damage { get; set; }
    public float DelayPerShot { get; set; }
    protected float _nextAttackTime;
    protected ObjectPooler _pooler;
    protected Turret _turret;
    protected Projectile _currentProjectileLoaded;

    private void Start()
    {
        _turret = GetComponent<Turret>();
        _pooler = GetComponent<ObjectPooler>();

        Damage = damage;
        DelayPerShot = delayBtwAttacks;
        LoadProjectile();
    }

    protected virtual void Update()
    {
        // Reload projectile if needed
        if (IsTurretEmpty())
        {
            LoadProjectile();
        }

        // Check if it's time to shoot
        if (Time.time > _nextAttackTime)
        {
            // Ensure there's a valid enemy target and the projectile is ready
        //    if (_turret.CurrentEnemyTarget != null && _currentProjectileLoaded != null &&
          //      _turret.CurrentEnemyTarget.GetEnemyHealth().CurrentHealth > 0f)  // Fixed to use GetEnemyHealth()
            {
                // Detach the projectile and set its enemy target
                _currentProjectileLoaded.transform.parent = null;
                _currentProjectileLoaded.SetEnemy(_turret.CurrentEnemyTarget);

                // Update the attack time to prevent multiple shots in quick succession
                _nextAttackTime = Time.time + DelayPerShot;
            }
        }
    }

    internal void ResetTurretProjectile()
    {
        // Reset the projectile logic (if needed)
        _currentProjectileLoaded.ResetProjectile();
    }

    protected virtual void LoadProjectile()
    {
        // Get a new projectile instance from the pool
        GameObject newInstance = _pooler.GetInstanceFromPool();
        newInstance.transform.localPosition = projectileSpawnPosition.position;
        newInstance.transform.SetParent(projectileSpawnPosition);

        // Set up the projectile and assign damage
        _currentProjectileLoaded = newInstance.GetComponent<Projectile>();
        _currentProjectileLoaded.TurretOwner = this;
        _currentProjectileLoaded.ResetProjectile();
        _currentProjectileLoaded.Damage = Damage;
        newInstance.SetActive(true);
    }

    private bool IsTurretEmpty()
    {
        // Return true if the turret is empty and needs to reload (this is just an example, adjust as needed)
        return false;  // Replace with actual condition for turret reloading
    }
}
