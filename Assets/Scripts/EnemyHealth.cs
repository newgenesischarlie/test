using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public static Action<Enemy> OnEnemyKilled;
    public static Action<Enemy> OnEnemyHit;

    private bool isDestroyed = false;
    private int hitPoints = 100; // Set initial health points as needed

    public event System.Action OnDeath;

    private ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    public void TakeDamage(int dmg)
    {
        // Reduce health by damage
        hitPoints -= dmg;

        // Check if health reaches 0 or below and if not already destroyed
        if (hitPoints <= 0 && !isDestroyed)
        {
            // Invoke the Spawner's onDestroy event if it's not null
            if (Spawner.onDestroy != null)
            {
                Spawner.onDestroy.Invoke();
            }

            // Set the destroyed flag to true
            isDestroyed = true;

            // Trigger the "OnEnemyKilled" event (can be subscribed to by other systems)
            OnEnemyKilled?.Invoke(GetComponent<Enemy>());

            // Optionally, trigger the "OnEnemyHit" event when damage is taken (even if the enemy dies)
            OnEnemyHit?.Invoke(GetComponent<Enemy>());

            // Destroy the enemy GameObject
            Destroy(gameObject);

            // Call the Die method when health reaches 0
            Die();
        }
        else
        {
            // If the enemy is still alive, invoke the "OnEnemyHit" event
            OnEnemyHit?.Invoke(GetComponent<Enemy>());
        }
    }

    [SerializeField] private GameObject healthBarPrefab; // Health bar prefab
    [SerializeField] private Transform barPosition; // Position where the health bar should appear
    [SerializeField] private float initialHealth = 10f; // Starting health
    [SerializeField] private float maxHealth = 10f; // Max health

    public float CurrentHealth { get; set; }

    private Image _healthBar; // UI Image component for health bar
    private Enemy _enemy; // Enemy script reference

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
        if (_enemy == null || _healthBar == null) return;

        // Decrease current health by damage amount
        CurrentHealth -= damage;

        // Clamp the health value to make sure it doesn't go below 0
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);

        // If health reaches 0, invoke the "OnEnemyKilled" event
        if (CurrentHealth <= 0f)
        {
            OnEnemyKilled?.Invoke(_enemy);
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
        if (objectPooler != null)
        {
            objectPooler.RemoveFromActiveEnemies(gameObject); // Method to remove it from pooler
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

    // Call this when health reaches 0
    public void Die()
    {
        if (objectPooler != null)
        {
            objectPooler.RemoveFromActiveEnemies(gameObject);
        }

        OnDeath?.Invoke();
    }
}
