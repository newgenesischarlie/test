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
    private bool isWeaponVisible = false;  // Track weapon visibility status
    private float hideWeaponDelay = 1f;  // Delay time before hiding the weapon
    private float hideWeaponTimer = 0f;  // Timer for weapon hiding

    private void Start()
    {
        // Ensure weapon is hidden at the start
        if (weaponObject != null)
        {
            weaponObject.SetActive(false);
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

        // Check if the target is out of range, but don’t hide the weapon immediately
        if (!CheckTargetIsInRange())
        {
            Debug.Log("Target out of range, searching for new target.");
            target = null;

            // Start the coroutine to hide weapon if the GameObject is active
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(HideWeaponAfterDelay());  // Start coroutine if active
            }
            else
            {
                // If GameObject is inactive, use a timer approach
                if (hideWeaponTimer <= 0f)
                {
                    hideWeaponTimer = hideWeaponDelay;  // Reset timer to delay
                }
                else
                {
                    hideWeaponTimer -= Time.deltaTime;  // Decrease the timer
                }

                // Hide weapon after delay
                if (hideWeaponTimer <= 0f)
                {
                    HideWeapon();
                }
            }
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
            if (target == null) // Assign the first found enemy as the target
            {
                target = colliders[i].transform;
                ShowWeapon(); // Show weapon once a target is acquired
            }
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
        if (weaponObject != null && !isWeaponVisible)
        {
            weaponObject.SetActive(true);  // Show the weapon
            isWeaponVisible = true;  // Mark the weapon as visible
            DebugWeaponVisibility(weaponObject);  // Ensure it's visible
        }
    }

    private void HideWeapon()
    {
        if (weaponObject != null && isWeaponVisible)
        {
            weaponObject.SetActive(false);  // Hide the weapon
            isWeaponVisible = false;  // Mark the weapon as hidden
            Debug.Log("Weapon is now hidden: " + weaponObject.activeSelf);
        }
    }

    private IEnumerator HideWeaponAfterDelay()
    {
        yield return new WaitForSeconds(1f);  // Wait for 1 second before hiding the weapon
        HideWeapon();
    }
}
