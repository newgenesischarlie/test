using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;  // This will show in the Inspector
    [SerializeField] private float targetingRange = 5f;      // This will show in the Inspector
    [SerializeField] private LayerMask enemyMask;            // This will show in the Inspector
    [SerializeField] private float rotationSpeed = 5f;       // This will show in the Inspector
    [SerializeField] private bool debugMode = true;

    private Transform target;

    private void Update()
    {
        if (target == null)
        {
            Debug.Log("No target found. Searching...");
            FindTarget();
            return;
        }

        Debug.Log("Target acquired: " + target.name);
        RotateTowardsTarget();

        // If the target is out of range, set it to null
        if (!CheckTargetIsInRange())
        {
            Debug.Log("Target out of range, searching for new target.");
            target = null;
        }
    }

    private void FindTarget()
    {
        float debugRange = debugMode ? targetingRange * 2 : targetingRange;
        // Debug layer mask
        Debug.Log("Enemy Layer Mask: " + enemyMask.value);
        
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(enemyMask);
        filter.useTriggers = true; // This is important if enemy colliders are triggers

        Collider2D[] colliders = new Collider2D[20]; // Adjust array size as needed
        int count = Physics2D.OverlapCircle(transform.position, debugRange, filter, colliders);

        // Only process valid colliders
        Debug.Log("Valid enemies detected: " + count);
        for (int i = 0; i < count; i++)
        {
            Debug.Log("Detected enemy at position: " + colliders[i].transform.position + 
                      " with layer: " + LayerMask.LayerToName(colliders[i].gameObject.layer));
            // Process each collider as before...
        }

        CheckEnemyColliders();
    }

    private bool CheckTargetIsInRange()
    {
        // Check if the target is within the targeting range
        return target != null && Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        if (target == null)
            return;

        // Calculate the direction to the target
        Vector2 direction = target.position - transform.position;

        // Calculate the angle from the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Create a quaternion for the desired rotation
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        // Smoothly rotate the turret towards the target
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        Debug.Log("Rotating towards target: " + target.name);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the targeting range for visualization in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }

    private void CheckEnemyColliders()
    {
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in allEnemies)
        {
            Collider2D collider = enemy.GetComponent<Collider2D>();
            if (collider == null)
            {
                Debug.LogError("Enemy " + enemy.name + " has NO Collider2D component!");
            }
            else
            {
                Debug.Log("Enemy " + enemy.name + " has Collider2D: " + 
                          collider.GetType().Name + 
                          ", IsTrigger: " + collider.isTrigger +
                          ", Enabled: " + collider.enabled);
            }
        }
    }
}
