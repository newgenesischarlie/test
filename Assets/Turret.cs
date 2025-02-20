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

    public object CurrentEnemyTarget { get; internal set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
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
        if (other.CompareTag("Enemy"))
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

        // For simplicity, set the first enemy in the list as the target
        // Here you could add logic to target the closest enemy if needed
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
        float rotationSpeed = 5f; // You can adjust this value
        float step = rotationSpeed * Time.deltaTime;
        float angleToRotate = Mathf.MoveTowardsAngle(transform.eulerAngles.z, angle, step);

        // Rotate the turret
        transform.rotation = Quaternion.Euler(0f, 0f, angleToRotate);
    }
}
