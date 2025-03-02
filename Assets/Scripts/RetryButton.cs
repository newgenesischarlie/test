using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RetryButton : MonoBehaviour
{
    // Reference to the button, can be set in the Unity inspector
    public Button retryButton;

    void Start()
    {
        // Ensure the button is not null and add the listener to the click event
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(ReloadScene);
        }
    }

    // This method reloads the current scene
    void ReloadScene()
    {
        // Get the current scene's name and reload it
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
}
