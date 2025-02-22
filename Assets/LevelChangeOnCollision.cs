using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangeOnCollision : MonoBehaviour
{
    // Use a string for the scene name instead of SceneAsset
    public string levelSceneName;
    public Collider2D levelImageCollider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered by: " + other.name);  // Check which object is triggering

        if (other == levelImageCollider)
        {
            Debug.Log("Player collided with level image!");

            // Check if the scene name is not empty or null
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
}
