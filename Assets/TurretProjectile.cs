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

    // Start is a Unity lifecycle method, so make sure it's capitalized.
    private void Start()
    {
        _turret = GetComponent<Turret>();
        _pooler = GetComponent<ObjectPooler>();

        Damage = damage;
        DelayPerShot = delayBtwAttacks;
        LoadProjectile();
    }

    // Update is a Unity lifecycle method, make sure it's capitalized.
    protected virtual void Update()
    {
        if (IsTurretEmpty())
        {
            LoadProjectile();
        }

        if (Time.time > _nextAttackTime)
        {
            if (_turret.CurrentEnemyTarget != null && _currentProjectileLoaded != null &&
                _turret.CurrentEnemyTarget.enemyHealth.CurrentHealth > 0f)
            {
                _currentProjectileLoaded.transform.parent = null;  // Detach from parent
                _currentProjectileLoaded.SetEnemy(_turret.CurrentEnemyTarget);
            }
            else
            {
            }
            _nextAttackTime = Time.time + DelayPerShot;
        }
    }

    internal void ResetTurretProjectile()
    {
        throw new NotImplementedException();
    }

    protected virtual void LoadProjectile()
    {
        // Getting the instance from the pool
        GameObject newInstance = _pooler.GetInstanceFromPool();
        newInstance.transform.localPosition = projectileSpawnPosition.position;
        newInstance.transform.SetParent(projectileSpawnPosition);  // Corrected variable name

        // Load the current projectile
        _currentProjectileLoaded = newInstance.GetComponent<Projectile>();
        _currentProjectileLoaded.TurretOwner = this;
        _currentProjectileLoaded.ResetProjectile();  // Ensure ResetProjectile() is defined in the Projectile class
        _currentProjectileLoaded.Damage = Damage;
        newInstance.SetActive(true);
    }

    private bool IsTurretEmpty()
    {
        // Assuming there's logic to check if turret is out of ammo or projectiles.
        return false; // Replace this with the actual condition (e.g., if projectiles are available)
    }
}
