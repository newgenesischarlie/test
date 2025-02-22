using UnityEngine;
using UnityEngine.SceneManagement;

public class Level0 : MonoBehaviour
{
    private Collider2D levelImageCollider;
    private GameObject levelScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entering trigger: " + other.name);  // Check which object is colliding

        // Check if the player is colliding with the level image
        if (other == levelImageCollider)
        {
            Debug.Log("Player collided with level image!");  // Confirm collision with the level image

            if (levelScene != null)
            {
                string sceneName = levelScene.name;
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError("No scene selected in the inspector.");
            }
        }
    }
}
