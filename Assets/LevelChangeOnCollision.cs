using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class LevelChangeOnCollision : MonoBehaviour
{
    // SceneAsset allows selecting a scene directly in the Unity Editor
    public SceneAsset levelScene; // Reference to the scene you want to load

    // The player's Collider2D
    public Collider2D playerCollider;

    // The UI image or button collider that triggers the scene change
    public Collider2D levelImageCollider;

    // This method will be triggered when the player first enters the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player is colliding with the level image (level 1 button or image)
        if (other == levelImageCollider)
        {
            Debug.Log("Player entered the trigger area"); // Debug message to confirm collision

            // If a scene is selected in the Inspector, load that scene
            if (levelScene != null)
            {
                // Get the scene name from the SceneAsset
                string sceneName = levelScene.name;

                // Load the level (scene) by name
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError("No scene selected in the inspector.");
            }
        }
    }
}
