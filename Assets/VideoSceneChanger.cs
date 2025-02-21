using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class VideoSceneChanger : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer
    public UnityEngine.SceneManagement.SceneAsset nextScene; // Reference to the next scene

    void Start()
    {
        // Ensure the VideoPlayer is assigned
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // Add an event to listen when the video finishes
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    // This method is called when the video finishes
    void OnVideoFinished(VideoPlayer vp)
    {
        // Start loading the scene asynchronously
        StartCoroutine(LoadSceneAsync());
    }

    // Coroutine to load the scene asynchronously
    IEnumerator LoadSceneAsync()
    {
        // Get the scene's name
        string sceneName = nextScene.name;

        // Start the asynchronous loading of the scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Prevent the scene from activating immediately (optional)
        asyncLoad.allowSceneActivation = false;

        // You can display a loading bar or something else here (optional)
        while (!asyncLoad.isDone)
        {
            // You can check the progress or show a loading screen
            Debug.Log("Loading Progress: " + asyncLoad.progress * 100 + "%");

            // When the load is almost complete, activate the scene
            if (asyncLoad.progress >= 0.9f)
            {
                // Wait until you're ready to activate the scene
                asyncLoad.allowSceneActivation = true;
            }

            yield return null; // Wait for the next frame
        }
    }
}
