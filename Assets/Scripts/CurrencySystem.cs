using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Import for Unity's built-in Text component
using TMPro;  // Import for TextMeshPro if needed

public class CurrencySystem : MonoBehaviour
{
    [SerializeField] private int coinTest;  // Default coins if PlayerPrefs is not set
    private string CURRENCY_SAVE_KEY = "MYGAME_CURRENCY";
    public int TotalCoins { get; private set; }
    public static CurrencySystem Instance { get; private set; }  // Singleton reference

    // Expose the coin sprite, text, and background in the Inspector
    [Header("UI Elements")]
    [SerializeField] private Sprite coinSprite;  // Coin sprite (e.g., a coin image)
    [SerializeField] private Image coinImage;    // UI Image to display the coin sprite
    [SerializeField] private Text coinDisplayText;  // Reference to the Unity Text component (or TextMeshPro)
    [SerializeField] private TextMeshProUGUI coinDisplayTextTMP; // Reference to TextMeshProUGUI, for TextMeshPro users
    [SerializeField] private Image uiBackground;  // Reference to the UI background panel or container

    private void Awake()
    {
        // Ensure that only one instance of CurrencySystem exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instance
        }

        DontDestroyOnLoad(gameObject);  // Keep this object across scenes
    }

    private void Start()
    {
        PlayerPrefs.DeleteKey(CURRENCY_SAVE_KEY);
        LoadCoins();
        UpdateCoinDisplay();  // Ensure the coin display is updated at the start
        UpdateCoinSprite();  // Set the coin sprite in the UI
    }

    private void LoadCoins()
    {
        TotalCoins = PlayerPrefs.GetInt(CURRENCY_SAVE_KEY, coinTest);
    }

    public void AddCoins(int amount)
    {
        TotalCoins += amount;
        PlayerPrefs.SetInt(CURRENCY_SAVE_KEY, TotalCoins);
        PlayerPrefs.Save();
        UpdateCoinDisplay();  // Update the coin display after adding coins
    }

    public void RemoveCoins(int amount)
    {
        if (TotalCoins >= amount)
        {
            TotalCoins -= amount;
            PlayerPrefs.SetInt(CURRENCY_SAVE_KEY, TotalCoins);
            PlayerPrefs.Save();
            UpdateCoinDisplay();  // Update the coin display after removing coins
        }
    }

    private void AddCoins(Enemy enemy)
    {
        // Add coins when an enemy is killed
        // AddCoins(enemy.DeathCoinReward);
    }

    private void OnEnable()
    {
        EnemyHealth.OnEnemyKilled += AddCoins;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyKilled -= AddCoins;
    }

    // Method to update the UI Text element with the current coin amount
    private void UpdateCoinDisplay()
    {
        if (coinDisplayText != null)
        {
            coinDisplayText.text = "Coins: " + TotalCoins.ToString();  // Update the text
        }
        else if (coinDisplayTextTMP != null)
        {
            coinDisplayTextTMP.text = "Coins: " + TotalCoins.ToString();  // For TextMeshPro
        }
    }

    // Method to update the coin sprite (if assigned)
    private void UpdateCoinSprite()
    {
        if (coinImage != null && coinSprite != null)
        {
            coinImage.sprite = coinSprite;  // Update the sprite of the coin image
        }
    }
}




