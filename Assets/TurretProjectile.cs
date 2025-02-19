using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [SerializedField] protected Transform projectileSpawnPosition;
    [SerializedField] protected float delayBtwAttacks = 2f;
    [SerializedField] protected float damage = 2f;

    public float Damage { get; set; }
    public float DelayPerShot { get; set; }
    protected float _nextAttackTime;
    protected ObjectPooler _pooler;
    protected Turret _turret;
    protected Projectile _currentProjectileLoaded;

    private void start()
    {
        _turret = GetComponent<>();
        _pooler = GetComponent<>();

        Damage = damage;
        DelayPerShot = delayBtwAttacks;
        LoadProjectiles();
    }

    protected virutal void update()
    {
        if (IsTurretEmpty())
        {
            LoadProjectile();
        }
        if (Time.time > _nextAttackTime)
        {
            if (_turret.CurrentEnemyTarget != null && _currentProjectileLoaded != null && _turret.CurrentEnemyTarget.enemyHealh.CurrentHealth > 0f)
            {
                _currentProjectileLoaded.transform.parent = null;
                currentProjectileLoaded.SetEnemy(turret.CurrentEnemyTarget);
            }
            _nextAttackTime = Time.time + DelayPerShot;
        }
    }

    protected virtual void LoadProjectile()
    {
        GameObject newInstance = _pooler.GetInstanceFromPool();
        newInstance.transform.localposition = projectileSpawnPosition.position;
        newInstance.transform.SetParent(projectileSpawnProjectile);

        _currentProjectileLoaded = newInstance.GetComponent<Projectile>();
        _currentProjectileLoaded.TurretOwner = this;
        _currentProjectileLoaded.ResetProjectile();
        _currentProjectileLoaded.Damage;
        newInstance.SetActive(true);
    }
}