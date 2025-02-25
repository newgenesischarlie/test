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

    private void Start()
    {
        CreateHealthBar(); // Create health bar in UI
        CurrentHealth = initialHealth; // Set current health to initial value
        _enemy = GetComponent<Enemy>(); // Get the Enemy component
    }

    private void Update()
    {
        // Test condition: When the "P" key is pressed, deal damage
        if (Input.GetKeyDown(KeyCode.P))
        {
            DealdDamage(5f); // Deal 5 damage when 'P' is pressed
        }

        // Smoothly update the health bar's fill amount
        _healthBar.fillAmount = Mathf.Lerp(_healthBar.fillAmount,
           CurrentHealth / maxHealth, Time.deltaTime * 10f);

        // Output the current health for debugging purposes
        Debug.Log("Current Health: " + CurrentHealth);
    }

    internal void DealDamage(float damage)
    {
        throw new NotImplementedException();
    }

    private void CreateHealthBar()
    {
        // Instantiate the health bar UI element and set its parent to the specified bar position
        GameObject healthBarObject = Instantiate(healthBarPrefab, barPosition.position, Quaternion.identity);
        _healthBar = healthBarObject.GetComponentInChildren<Image>(); // Get the Image component for the health bar
    }

    private void DealdDamage(float damageAmount)
    {
        // Decrease current health by damage amount
        CurrentHealth -= damageAmount;

        // Clamp the health value to make sure it doesn't go below 0
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);

        // If health reaches 0, invoke the "OnEnemyKilled" event
        if (CurrentHealth <= 0f)
        {
            OnEnemyKilled?.Invoke(_enemy);
            Destroy(gameObject); // Destroy enemy object when killed
        }

        // Trigger "OnEnemyHit" event when damage is dealt
        OnEnemyHit?.Invoke(_enemy);
    }

    // Resets the enemy health to initial value (useful for respawn or reset scenarios)
    internal void ResetHealth()
    {
        CurrentHealth = initialHealth;
        _healthBar.fillAmount = 1f; // Reset health bar fill amount
    }
}
