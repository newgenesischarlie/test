using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangeOnCollision : MonoBehaviour
{
    public string levelSceneName;  // Name of the scene to load
    public Collider2D levelImageCollider;  // Reference to the level image collider (must be a trigger)

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered by: " + other.name);

        // Ensure only the player with the right tag triggers the event
        if (other.CompareTag("Player"))  // Assuming the Player has a "Player" tag
        {
            Debug.Log("Player collided with level image!");

            // Proceed with scene change
            if (!string.IsNullOrEmpty(levelSceneName))
            {
                Debug.Log("Loading scene: " + levelSceneName);
                SceneManager.LoadScene(levelSceneName);
            }
            else
            {
                Debug.LogError("No scene name selected in the inspector.");
            }
        }
    }

    // Optional: Draw the trigger collider in the scene view for debugging
    private void OnDrawGizmos()
    {
        if (levelImageCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(levelImageCollider.bounds.center, levelImageCollider.bounds.size);
        }
    }
}
