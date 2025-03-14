using UnityEngine;
using TMPro;

public class EnemyFX : MonoBehaviour
{
    [SerializeField] private Transform textDamageSpawnPosition;

    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    public void EnemyHit(Enemy enemy, float damage)
    {
        // Check if the hit is for this enemy
        if (_enemy == enemy)
        {
            // Get a new damage text instance from the pool
            GameObject newInstance = DamageTextManager.Instance.Pooler.GetInstanceFromPool();

            // Ensure the damage text instance is valid
            if (newInstance != null)
            {
                // Get the TextMeshProUGUI component from the damage text instance
                TextMeshProUGUI damageText = newInstance.GetComponent<DamageText>().DmgText;

                // Set the damage value to the text
                damageText.text = damage.ToString();

                // Position the damage text at the specified spawn position
                newInstance.transform.SetParent(textDamageSpawnPosition);
                newInstance.transform.position = textDamageSpawnPosition.position;

                // Activate the damage text to display it
                newInstance.SetActive(true);
            }
        }
    }

    private void OnEnable()
    {
        // Subscribe to the OnEnemyHit event
        Projectile.OnEnemyHit += EnemyHit;
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnEnemyHit event
        Projectile.OnEnemyHit -= EnemyHit;
    }
}
