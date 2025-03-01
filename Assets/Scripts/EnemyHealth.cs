using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public static Action<Enemy> OnEnemyKilled;
    public static Action<Enemy> OnEnemyHit;

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;
    private ObjectPooler objectPooler;

    public event System.Action OnDeath;

    private void Awake()
    {
        ResetHealth();
        objectPooler = FindObjectOfType<ObjectPooler>();
    }

    private void OnEnable()
    {
        isDead = false;
        ResetHealth();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead || currentHealth <= 0) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    public void TakeDamageFloat(float damageAmount)
    {
        int intDamage = (int)damageAmount;
        TakeDamage(intDamage);
    }

    private void HandleDeath()
    {
        if (isDead) return;
        isDead = true;
        
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }

    public bool IsAlive()
    {
        return currentHealth > 0 && !isDead;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    [SerializeField] private GameObject healthBarPrefab; // Health bar prefab
    [SerializeField] private Transform barPosition; // Position where the health bar should appear
    [SerializeField] private float maxHealthBar = 10f; // Max health

    private Image _healthBar; // UI Image component for health bar
    private Enemy _enemy; // Enemy script reference

    private void Start()
    {
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
                currentHealth / maxHealthBar, Time.deltaTime * 10f);
        }

        // Output the current health for debugging purposes
        Debug.Log("Current Health: " + currentHealth);
    }

    internal void DealDamage(float damage)
    {
        // Ensure that we don't deal damage if the health bar or enemy is not initialized
        if (_enemy == null || _healthBar == null) return;

        // Decrease current health by damage amount
        currentHealth -= damage;

        // Clamp the health value to make sure it doesn't go below 0
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealthBar);

        // If health reaches 0, invoke the "OnEnemyKilled" event
        if (currentHealth <= 0f)
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
            objectPooler.RemoveFromActiveEnemies(_enemy.gameObject); // Method to remove it from pooler
        }

        // Deactivate the enemy rather than destroying it
        gameObject.SetActive(false); // Deactivate enemy GameObject

        // Optionally, you can reset the enemy health when reactivated
        ResetHealth();
    }

    // Call this when health reaches 0
    private void Die()
    {
        OnDeath?.Invoke();
    }

    private void CalculateDamage(float damageAmount)
    {
        int intDamage = Mathf.RoundToInt(damageAmount); // Explicit conversion
        TakeDamage(intDamage);
    }
}
