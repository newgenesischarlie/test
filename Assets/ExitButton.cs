using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    // Reference to the button, can be set in the Unity inspector
    public Button exitButton;

    void Start()
    {
        // Ensure the button is not null and add the listener to the click event
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitGame);
        }
    }

    // This method exits the game
    void ExitGame()
    {
        // If we're in the editor, stop playing
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // If we're in a built version of the game, quit the application
            Application.Quit();
        #endif
    }
}
