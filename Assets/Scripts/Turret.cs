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

    [Header("Weapon Settings")]
    [SerializeField] private GameObject weaponObject;  // Reference to the weapon GameObject (hidden initially)
    
    private Transform target;

    private void Start()
    {
        if (weaponObject != null)
        {
            weaponObject.SetActive(false); // Hide weapon at the start
            DebugWeaponVisibility(weaponObject);
        }
    }

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

        if (!CheckTargetIsInRange())
        {
            Debug.Log("Target out of range, searching for new target.");
            target = null;
            HideWeapon();  // Hide weapon if target is lost
        }
    }

    private void FindTarget()
    {
        float debugRange = debugMode ? targetingRange * 2 : targetingRange;
        
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(enemyMask);
        filter.useTriggers = true;

        Collider2D[] colliders = new Collider2D[20]; 
        int count = Physics2D.OverlapCircle(transform.position, debugRange, filter, colliders);

        for (int i = 0; i < count; i++)
        {
            // Additional debug to check if enemies are being detected
            Debug.Log("Detected enemy: " + colliders[i].transform.name);
        }
    }

    private bool CheckTargetIsInRange()
    {
        return target != null && Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        if (target == null)
            return;

        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }

    private void DebugWeaponVisibility(GameObject weapon)
    {
        SpriteRenderer spriteRenderer = weapon.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Debug.Log("SpriteRenderer enabled: " + spriteRenderer.enabled);
            spriteRenderer.enabled = true;  // Ensure it is enabled
        }
        else
        {
            Debug.LogError("Weapon has no SpriteRenderer attached!");
        }

        Debug.Log("Weapon Layer: " + LayerMask.LayerToName(weapon.layer)); // Check weapon's layer
    }

    private void ShowWeapon()
    {
        if (weaponObject != null)
        {
            weaponObject.SetActive(true);  
            DebugWeaponVisibility(weaponObject);  // Ensure it's visible
        }
    }

    private void HideWeapon()
    {
        if (weaponObject != null)
        {
            weaponObject.SetActive(false);  
        }
    }
}
