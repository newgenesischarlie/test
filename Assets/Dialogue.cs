using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines; // The lines of dialogue.
    public string[] characterNames; // Array for character names corresponding to each line.
    public AudioClip[] soundEffects; // Sound effects for each line.
    public float textSpeed; // Speed at which text is typed.

    private int index;
    private bool isTyping; // Flag to check if typing is in progress
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Assuming the AudioSource component is on the same GameObject.

        // Debugging: Check if AudioSource exists
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource component is missing from this GameObject.");
        }

        // Debugging: Ensure TextMeshPro component is attached
        if (textComponent == null)
        {
            Debug.LogError("TextMeshProUGUI component is not assigned!");
            return;
        }

        // Debugging: Make sure lines and characterNames are populated
        if (lines.Length == 0 || characterNames.Length == 0)
        {
            Debug.LogError("Dialogue lines or character names are not assigned properly!");
            return;
        }

        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        // Only allow click to skip if typing is not in progress
        if (Input.GetMouseButtonDown(0) && !isTyping)
        {
            // If the text is fully typed out, move to the next line
            if (textComponent.text == characterNames[index] + ": " + lines[index])
            {
                Debug.Log("Line fully typed. Proceeding to next line.");
                NextLine();
            }
            else
            {
                Debug.Log("Skipping typing of current line.");
                // Finish typing immediately
                StopAllCoroutines();
                textComponent.text = characterNames[index] + ": " + lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;

        // Debugging: Ensure dialogue and character name arrays are correct
        Debug.Log("Starting dialogue. First line: " + lines[index]);
        textComponent.text = characterNames[index] + ": "; // Set the character name at the start.
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true; // Mark typing as in progress

        // Play sound effect if available for the line and audioSource is not null
        if (audioSource != null && soundEffects.Length > index && soundEffects[index] != null)
        {
            Debug.Log("Playing sound for line: " + index);
            audioSource.PlayOneShot(soundEffects[index]);
        }

        // Display the dialogue with typing effect
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text = characterNames[index] + ": " + textComponent.text.Substring(characterNames[index].Length + 2); // Update the name part only.
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false; // Mark typing as complete
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = characterNames[index] + ": "; // Set the new character name
            Debug.Log("Proceeding to next line: " + lines[index]);
            StartCoroutine(TypeLine()); // Start typing the next line
        }
        else
        {
            // Load next scene after all dialogues are finished
            Debug.Log("All dialogue lines finished. Loading next scene.");
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        // Assuming the next scene is the next build scene in the order.
        // You can replace this with a specific scene name or index.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
