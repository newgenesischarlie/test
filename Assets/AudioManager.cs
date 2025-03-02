using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class AudioSourceData
    {
        public string name;
        public AudioSource source;
        public AudioClip defaultClip;
    }

    [Header("Audio Sources")]
    [SerializeField] private List<AudioSourceData> audioSources = new List<AudioSourceData>();

    // Define AudioClip variables for background music and gameover sound
    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusicClip;
    [SerializeField] private AudioClip gameOverClip;

    private Dictionary<string, AudioSource> audioSourceDict = new Dictionary<string, AudioSource>();

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioSources()
    {
        audioSourceDict.Clear();
        foreach (var audioData in audioSources)
        {
            if (audioData.source != null && !string.IsNullOrEmpty(audioData.name))
            {
                audioSourceDict[audioData.name] = audioData.source;
                if (audioData.defaultClip != null)
                {
                    audioData.source.clip = audioData.defaultClip;
                }
            }
        }
    }

    /// <summary>
    /// Play a sound when the scene starts (for example, background music).
    /// </summary>
    public void PlaySceneStartAudio()
    {
        if (!isGameOver)  // Ensure the gameover clip isn't playing
        {
            PlayAudio("BackgroundMusic", backgroundMusicClip);  // Play the background music
        }
    }

    /// <summary>
    /// Play the gameover sound.
    /// </summary>
    public void PlayGameOverAudio()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            StopAllAudio();  // Stop any currently playing audio
            PlayAudio("GameOverMusic", gameOverClip);  // Play the game over sound
        }
    }

    /// <summary>
    /// Play audio on a specific audio source.
    /// </summary>
    /// <param name="sourceName">The audio source name</param>
    /// <param name="clip">The clip to play</param>
    public void PlayAudio(string sourceName, AudioClip clip)
    {
        if (!audioSourceDict.ContainsKey(sourceName))
        {
            Debug.LogWarning($"Audio source '{sourceName}' not found!");
            return;
        }

        var targetSource = audioSourceDict[sourceName];
        targetSource.clip = clip;
        targetSource.Play();
    }

    /// <summary>
    /// Stop all audio sources.
    /// </summary>
    public void StopAllAudio()
    {
        foreach (var source in audioSourceDict.Values)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }
}
