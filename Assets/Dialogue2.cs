using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Dialogue2 : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public AudioClip[] soundEffects;
    public float textSpeed;
    public float characterMoveSpeed = 0.05f;
    public Transform[] characterSprites;
    public float moveAmount = 0.1f;

    public Transform appearanceSprite; // The sprite that will appear after a specific line
    public Transform characterToMove; // The character that will move towards the sprite
    public float moveSpeed = 2f; // Speed at which the character moves towards the sprite
    public int spriteAppearsAfterLine = 3; // Line after which the sprite will appear

    private int index;
    private bool isTyping;
    private bool spriteAppeared;
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

        textComponent.text = string.Empty;
        PreloadAudio();
        StartDialogue();
    }

    void PreloadAudio()
    {
        if (audioSource != null && soundEffects.Length > 0 && soundEffects[0] != null)
        {
            audioSource.clip = soundEffects[0];
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
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;

        if (audioSource != null && soundEffects.Length > index && soundEffects[index] != null)
        {
            audioSource.clip = soundEffects[index];
            audioSource.loop = true;
            audioSource.Play();
        }

        if (index < characterSprites.Length)
        {
            StartCoroutine(ContinuousMoveCharacterSprite(characterSprites[index])); // Start continuous movement for the sprite
        }

        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        if (audioSource != null)
        {
            audioSource.loop = false;
            audioSource.Stop();
        }

        // Check if the sprite should appear after this line
        if (index == spriteAppearsAfterLine)
        {
            ShowAppearanceSprite();
        }

        isTyping = false;
    }

    void ShowAppearanceSprite()
    {
        if (appearanceSprite != null)
        {
            appearanceSprite.gameObject.SetActive(true); // Show the sprite
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            StartCharacterMovement(); // Start moving the character when all lines are finished
        }
    }

    void StartCharacterMovement()
    {
        if (appearanceSprite != null && characterToMove != null)
        {
            StartCoroutine(MoveCharacterTowardsSprite(characterToMove, appearanceSprite));
        }
    }

    IEnumerator MoveCharacterTowardsSprite(Transform character, Transform target)
    {
        while (Vector3.Distance(character.position, target.position) > 0.1f) // When not yet close enough
        {
            character.position = Vector3.MoveTowards(character.position, target.position, moveSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Hide both the character and the sprite after collision
        character.gameObject.SetActive(false);
        target.gameObject.SetActive(false);
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

    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
