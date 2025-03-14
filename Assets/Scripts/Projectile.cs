using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectile : MonoBehaviour
{
    public static Action<Enemy, float> OnEnemyHit;
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] private float minDistanceToDealDamage = 0.1f;

    public TurretProjectile TurretOwner { get; set; }

    public float Damage { get; set; }

    protected Enemy _enemyTarget;

    private ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    protected virtual void Update()
    {
        if (_enemyTarget != null)
        {
            MoveProjectile();
            RotateProjectile();
        }
    }

    public void ResetProjectile()
    {
        // Reset any relevant properties of the projectile when it is reused
        _enemyTarget = null;  // Reset the enemy target
        // Optionally reset other properties like position, speed, etc.
        transform.localPosition = Vector3.zero;  // Example reset for position
    }

    protected virtual void MoveProjectile()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            _enemyTarget.transform.position, moveSpeed * Time.deltaTime);
        float distanceToTarget = (_enemyTarget.transform.position - transform.position).magnitude;

        if (distanceToTarget < minDistanceToDealDamage)
        {
            OnEnemyHit?.Invoke(_enemyTarget, Damage);
      //      if (_enemyTarget != null && _enemyTarget.GetEnemyHealth() != null)
            {
           //     _enemyTarget.GetEnemyHealth().DealDamage(Damage); // Use the GetEnemyHealth() method
            }

            TurretOwner.ResetTurretProjectile();
            ObjectPooler.ReturnToPool(gameObject);
        }
    }

    private void RotateProjectile()
    {
        Vector3 enemyPos = _enemyTarget.transform.position - transform.position;
        float angle = Vector3.SignedAngle(transform.up, enemyPos, transform.forward);
        transform.Rotate(0f, 0f, angle);
    }

    public void SetEnemy(Enemy enemy)
    {
        _enemyTarget = enemy;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ... your collision logic ...

        ObjectPooler.ReturnToPool(gameObject);
    }
}
