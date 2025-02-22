using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChangeOnCollision : MonoBehaviour
{
    // The player's Collider2D
    public Collider2D playerCollider;

    // The UI image or button collider that triggers the scene change
    public Collider2D levelImageCollider;

    // The scene name to load when collision happens (e.g., Level 1)
    public string levelName = "Level 1";

    // This is where the collision detection happens
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is colliding with the UI image (level 1 button or image)
        if (collision.collider == levelImageCollider)
        {
            // Load the level (scene) by name
            SceneManager.LoadScene(levelName);
        }
    }
}
