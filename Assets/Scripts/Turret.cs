using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // List to hold all the enemies in the turret's range
    private List<Enemy> _enemies = new List<Enemy>();

    // Current enemy that the turret is targeting
    private Enemy _currentEnemyTarget;

    // Modifiable attack range
    [SerializeField] private float attackRange = 5f;  // Can be set in the Unity Inspector

    // List of tags to filter which enemies the turret will target
    [SerializeField] private List<string> enemyTags = new List<string> { "Enemy" };

    // The speed at which the turret rotates
    [SerializeField] private float rotationSpeed = 5f;

    public float AttackRange => attackRange;  // To allow other scripts to access the range

       public Enemy CurrentEnemyTarget { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsEnemyInRange(other))
        {
            Enemy newEnemy = other.GetComponent<Enemy>();
            if (newEnemy != null && !_enemies.Contains(newEnemy))
            {
                _enemies.Add(newEnemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsEnemyInRange(other))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && _enemies.Contains(enemy))
            {
                _enemies.Remove(enemy);
            }
        }
    }

    private void Update()
    {
        GetCurrentEnemyTarget();
        RotateTowardsTarget();
    }

    private void GetCurrentEnemyTarget()
    {
        if (_enemies.Count <= 0)
        {
            _currentEnemyTarget = null;
            return;
        }

        // Target the first enemy within range by default
        // You can extend this logic to prioritize enemies by health, distance, etc.
        _currentEnemyTarget = _enemies[0];
    }

    private void RotateTowardsTarget()
    {
        if (_currentEnemyTarget == null)
        {
            return;
        }

        // Get the direction to the target enemy
        Vector3 targetPos = _currentEnemyTarget.transform.position - transform.position;

        // Calculate the angle to rotate
        float angle = Vector3.SignedAngle(transform.up, targetPos, Vector3.forward);

        // Rotate the turret smoothly, adjusting the rotation speed
        float step = rotationSpeed * Time.deltaTime;
        float angleToRotate = Mathf.MoveTowardsAngle(transform.eulerAngles.z, angle, step);

        // Rotate the turret
        transform.rotation = Quaternion.Euler(0f, 0f, angleToRotate);
    }

    // Check if the collider is an enemy within the turret's range and matching the tags
    private bool IsEnemyInRange(Collider2D collider)
    {
        // Check if the collider belongs to an enemy and within the turret's attack range
        if (collider.CompareTag("Enemy"))
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
            {
                return true;
            }
        }
        return false;
    }

    // Draw the attack range for visualization in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
