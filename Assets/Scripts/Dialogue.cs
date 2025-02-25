using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public AudioClip[] soundEffects;
    public float textSpeed; // Speed for text typing
    public float characterMoveSpeed = 0.05f; // Speed for character sprite movement (up and down)
    public Transform[] characterSprites; // Array of character sprites that will move per line
    public float moveAmount = 0.1f; // Amount to move the sprite (up/down)

    private int index;
    private bool isTyping;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource component is missing from this GameObject.");
        }

        if (textComponent == null)
        {
            Debug.LogError("TextMeshProUGUI component is not assigned!");
            return;
        }

        if (lines.Length == 0)
        {
            Debug.LogError("Dialogue lines are not assigned properly!");
            return;
        }

        textComponent.text = string.Empty; // Ensure the text is empty at the start
        PreloadAudio(); // Preload the audio clip for the first line
        StartDialogue();
    }

    void PreloadAudio()
    {
        if (audioSource != null && soundEffects.Length > 0 && soundEffects[0] != null)
        {
            audioSource.clip = soundEffects[0]; // Preload the first sound effect
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isTyping)
        {
            if (textComponent.text != lines[index])
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
            else
            {
                NextLine();
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        textComponent.text = string.Empty; // Clear any previous text
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;

        // Play and loop the sound effect for the line, if available
        if (audioSource != null && soundEffects.Length > index && soundEffects[index] != null)
        {
            audioSource.clip = soundEffects[index];
            audioSource.loop = true; // Loop the sound effect
            audioSource.Play();
        }

        // Move the appropriate character sprite based on the line's index
        if (index < characterSprites.Length)
        {
            StartCoroutine(ContinuousMoveCharacterSprite(characterSprites[index])); // Start continuous movement for the sprite
        }

        // Display the line letter by letter
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c; // Append the character to the text
            yield return new WaitForSeconds(textSpeed);
        }

        // Stop the sound loop immediately after the line is fully typed
        if (audioSource != null)
        {
            audioSource.loop = false; // Stop the looping sound effect
            audioSource.Stop(); // Immediately stop the sound
        }

        isTyping = false;
    }

    IEnumerator ContinuousMoveCharacterSprite(Transform characterSprite)
    {
        // Continuously move the character's sprite up and down for each letter typed
        Vector3 originalPosition = characterSprite.position;
        Vector3 upPosition = originalPosition + Vector3.up * moveAmount;
        Vector3 downPosition = originalPosition;

        while (isTyping)
        {
            // Move the sprite up
            characterSprite.position = upPosition;
            yield return new WaitForSeconds(characterMoveSpeed); // Delay based on characterMoveSpeed

            // Move the sprite back down
            characterSprite.position = downPosition;
            yield return new WaitForSeconds(characterMoveSpeed); // Delay based on characterMoveSpeed
        }

        // Ensure the sprite ends back at the original position after typing is complete
        characterSprite.position = originalPosition;
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty; // Clear the previous line
            StartCoroutine(TypeLine());
        }
        else
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
