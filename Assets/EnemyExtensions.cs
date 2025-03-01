using UnityEngine;

// This should be a static class if it contains extension methods
public static class EnemyExtensions
{
    // Extension methods need 'this' keyword
    public static void ResetEnemyState(this Enemy enemy)
    {
        if (enemy == null) return;
        
        enemy.ResetHealth();
        enemy.ResetEnemy(); // Now accessible
    }
}
