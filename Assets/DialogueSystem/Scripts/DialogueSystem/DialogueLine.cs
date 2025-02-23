using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        private TMP_Text textHolder;

        [Header("Text Options")]
        [SerializeField] private string input;
        [SerializeField] private Color textColor;
        [SerializeField] private TMP_FontAsset textFont;  // Use TMP_FontAsset instead of Font

        [Header("Time parameters")]
        [SerializeField] private float delay;
        [SerializeField] private float delayBetweenLines;

        [Header("Sound")]
        [SerializeField] private AudioClip sound;

        //[Header("Character Image")]
        // [SerializeField] private Sprite characterSprite;
        // [SerializeField] private Image imageHolder;

        private void Awake()
        {
            textHolder = GetComponent<TMP_Text>();
            textHolder.text = "";

            // If you want to use character images
            // imageHolder.sprite = characterSprite;
            // imageHolder.preserveAspect = true;
        }

        private void Start()
        {
            StartCoroutine(WriteText(input, textHolder, textColor, textFont, delay, sound, delayBetweenLines));
        }

        // Coroutine to handle writing text with typing effect
        private IEnumerator WriteText(string text, TMP_Text textObject, Color color, TMP_FontAsset font, float delay, AudioClip soundEffect, float delayBetweenLines)
        {
            textObject.color = color;
            textObject.font = font; // Assign TMP_FontAsset here

            for (int i = 0; i <= text.Length; i++)
            {
                textObject.text = text.Substring(0, i);
                if (soundEffect != null)
                {
                    AudioSource.PlayClipAtPoint(soundEffect, Camera.main.transform.position);
                }
                yield return new WaitForSeconds(delay);
            }

            // Wait for a delay between lines (optional, if you want a pause after the text is fully written)
            yield return new WaitForSeconds(delayBetweenLines);
        }
    }
}