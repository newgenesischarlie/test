using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public static Action<Enemy> OnEnemyKilled;
    public static Action<Enemy> OnEnemyHit;

    [SerializeField] private GameObject healthBarPrefab; // Health bar prefab
    [SerializeField] private Transform barPosition; // Position where the health bar should appear
    [SerializeField] private float initialHealth = 10f; // Starting health
    [SerializeField] private float maxHealth = 10f; // Max health

    public float CurrentHealth { get; set; }

    private Image _healthBar; // UI Image component for health bar
    private Enemy _enemy; // Enemy script reference
    private ObjectPooler _objectPooler; // Reference to the ObjectPooler

    private void Start()
    {
        // Ensure ObjectPooler is assigned
        _objectPooler = FindObjectOfType<ObjectPooler>();

        // Check if necessary components are assigned
        if (healthBarPrefab == null)
        {
            Debug.LogError("Health Bar Prefab is not assigned in the Inspector.");
            return; // Stop execution if prefab is missing
        }

        if (barPosition == null)
        {
            Debug.LogError("Bar Position is not assigned in the Inspector.");
            return; // Stop execution if bar position is missing
        }

        _enemy = GetComponent<Enemy>(); // Get the Enemy component

        if (_enemy == null)
        {
            Debug.LogError("Enemy component is missing on this GameObject.");
            return; // Stop execution if no Enemy component is found
        }

        CreateHealthBar(); // Create health bar in UI
        CurrentHealth = initialHealth; // Set current health to initial value
    }

    private void Update()
    {
        // Test condition: When the "P" key is pressed, deal damage
        if (Input.GetKeyDown(KeyCode.P))
        {
            DealDamage(5f); // Deal 5 damage when 'P' is pressed
        }

        // Smoothly update the health bar's fill amount
        if (_healthBar != null)
        {
            _healthBar.fillAmount = Mathf.Lerp(_healthBar.fillAmount,
                CurrentHealth / maxHealth, Time.deltaTime * 10f);
        }

        // Output the current health for debugging purposes
        Debug.Log("Current Health: " + CurrentHealth);
    }

    internal void DealDamage(float damage)
    {
        // Ensure that we don't deal damage if the health bar or enemy is not initialized
        if (_enemy == null || _healthBar == null)
        {
            Debug.LogError("Enemy or health bar is null in DealDamage!");
            return;
        }

        // Decrease current health by damage amount
        CurrentHealth -= damage;

        // Clamp the health value to make sure it doesn't go below 0
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);

        // If health reaches 0, invoke the "OnEnemyKilled" event
        if (CurrentHealth <= 0f)
        {
            Debug.Log("Enemy killed: " + gameObject.name);
            OnEnemyKilled?.Invoke(_enemy); // Invoke the OnEnemyKilled event
            KillEnemy(); // Kill the enemy by deactivating it
        }

        // Trigger "OnEnemyHit" event when damage is dealt
        OnEnemyHit?.Invoke(_enemy);
    }

    private void CreateHealthBar()
    {
        // Instantiate the health bar UI element and set its parent to the specified bar position
        GameObject healthBarObject = Instantiate(healthBarPrefab, barPosition.position, Quaternion.identity);
        _healthBar = healthBarObject.GetComponentInChildren<Image>(); // Get the Image component for the health bar

        if (_healthBar == null)
        {
            Debug.LogError("Health bar prefab does not contain an Image component.");
        }
    }

    private void KillEnemy()
    {
        // Notify ObjectPooler to remove from the active pool
        if (_objectPooler != null)
        {
            Debug.Log("Returning enemy to pool: " + gameObject.name);
            _objectPooler.RemoveFromActiveEnemies(_enemy.gameObject); // Method to remove it from pooler
        }

        // Deactivate the enemy rather than destroying it
        gameObject.SetActive(false); // Deactivate enemy GameObject

        // Optionally, you can reset the enemy health when reactivated
        ResetHealth();
    }

    // Resets the enemy health to initial value (useful for respawn or reset scenarios)
    internal void ResetHealth()
    {
        CurrentHealth = initialHealth;
        if (_healthBar != null)
        {
            _healthBar.fillAmount = 1f; // Reset health bar fill amount
        }
    }

    // Called when the enemy is deactivated, unsubscribe from events
    private void OnDisable()
    {
        OnEnemyKilled -= HandleEnemyKilled;
        OnEnemyHit -= HandleEnemyHit;
    }

    // Called when the enemy is reactivated, subscribe to events
    private void OnEnable()
    {
        // Subscribe to events when the enemy is reactivated
        OnEnemyKilled += HandleEnemyKilled;
        OnEnemyHit += HandleEnemyHit;
    }

    private void HandleEnemyKilled(Enemy enemy)
    {
        if (enemy == _enemy)
        {
            Debug.Log("Handling enemy kill: " + enemy.gameObject.name);
            // You can handle any additional logic here, like resetting states
        }
    }

    private void HandleEnemyHit(Enemy enemy)
    {
        if (enemy == _enemy)
        {
            Debug.Log("Enemy hit: " + enemy.gameObject.name);
            // Handle enemy hit logic here
        }
    }
}
