using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextSceneButton : MonoBehaviour
{
    // Reference to the button, can be set in the Unity inspector
    public Button nextButton;

    void Start()
    {
        // Ensure the button is not null and add the listener to the click event
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(LoadNextScene);
        }
    }

    // This method loads the next scene in the build settings
    void LoadNextScene()
    {
        // Get the current scene's index, then load the next scene by incrementing the index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene exists in the build settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No next scene found in the build settings.");
        }
    }
}
