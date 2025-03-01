using UnityEngine;
using System.Collections.Generic;

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
    
    private Dictionary<string, AudioSource> audioSourceDict = new Dictionary<string, AudioSource>();

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
    /// Play audio on a specific audio source, stopping any other sources.
    /// </summary>
    /// <param name="sourceName">Name of the audio source to use</param>
    /// <param name="clip">The audio clip to play</param>
    public void PlayAudio(string sourceName, AudioClip clip)
    {
        if (!audioSourceDict.ContainsKey(sourceName))
        {
            Debug.LogWarning($"Audio source '{sourceName}' not found!");
            return;
        }

        // Stop all other audio sources
        foreach (var source in audioSourceDict.Values)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }

        // Play the new audio clip
        var targetSource = audioSourceDict[sourceName];
        targetSource.clip = clip;
        targetSource.Play();
    }

    /// <summary>
    /// Play audio without stopping other sources.
    /// </summary>
    public void PlayAudioOverlap(string sourceName, AudioClip clip)
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

    /// <summary>
    /// Stop a specific audio source.
    /// </summary>
    public void StopAudio(string sourceName)
    {
        if (audioSourceDict.ContainsKey(sourceName) && audioSourceDict[sourceName].isPlaying)
        {
            audioSourceDict[sourceName].Stop();
        }
    }

    /// <summary>
    /// Set volume for a specific audio source.
    /// </summary>
    public void SetVolume(string sourceName, float volume)
    {
        if (audioSourceDict.ContainsKey(sourceName))
        {
            audioSourceDict[sourceName].volume = Mathf.Clamp01(volume);
        }
    }

    /// <summary>
    /// Check if any audio is currently playing.
    /// </summary>
    public bool IsAnyAudioPlaying()
    {
        foreach (var source in audioSourceDict.Values)
        {
            if (source.isPlaying)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Check if specific audio source is playing.
    /// </summary>
    public bool IsAudioPlaying(string sourceName)
    {
        return audioSourceDict.ContainsKey(sourceName) && audioSourceDict[sourceName].isPlaying;
    }
}
