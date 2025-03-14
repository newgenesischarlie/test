using UnityEngine;
using TMPro;  // For using TextMeshPro if you want to display text damage

public class EnemyFX : MonoBehaviour
{
    [SerializeField] private Transform textDamageSpawnPosition;  // Where to spawn the damage effect
    [SerializeField] private GameObject damageEffectPrefab;      // Prefab for the damage effect (e.g., text or sprite)
    
    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();

        // Ensure the damage effect prefab starts inactive
        if (damageEffectPrefab != null)
        {
            damageEffectPrefab.SetActive(false);
        }
    }

    // Call this method when the enemy takes damage
    public void ShowDamageEffect(int damageAmount)
    {
        if (damageEffectPrefab != null && textDamageSpawnPosition != null)
        {
            // Activate the damage effect and set its position
            GameObject damageEffect = Instantiate(damageEffectPrefab, textDamageSpawnPosition.position, Quaternion.identity);
            damageEffect.SetActive(true);  // Make the effect visible

            // If the effect is a TextMeshPro object, set the damage text
            TextMeshPro textMeshPro = damageEffect.GetComponent<TextMeshPro>();
            if (textMeshPro != null)
            {
                textMeshPro.text = damageAmount.ToString(); // Display the damage amount
            }

            // Optionally, you can add any additional effects (like animation or a fade-out effect) here

            // Destroy the effect after a brief period (e.g., 1 second)
            Destroy(damageEffect, 1f);
        }
    }

    // Example of how to call this method when the enemy takes damage
    public void OnEnemyHit(int damageAmount)
    {
        ShowDamageEffect(damageAmount); // Show damage effect
        // Handle other enemy health-related logic (e.g., reduce health)
    }
}
