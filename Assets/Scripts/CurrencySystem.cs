using UnityEngine;
using UnityEngine.UI; // Import for Unity's built-in UI components like Image and Text
using TMPro; // Import for TextMeshPro if you're using that for text display

public class CurrencySystem : MonoBehaviour
{
    [SerializeField] private int coinTest;
    private string CURRENCY_SAVE_KEY = "MYGAME_CURRENCY";
    public int TotalCoins { get; private set; }
    public static CurrencySystem Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private Sprite coinSprite;
    [SerializeField] private Image coinImage; // This is the Image component for the coin UI
    [SerializeField] private Text coinDisplayText; // For Unity's built-in Text component
    [SerializeField] private TextMeshProUGUI coinDisplayTextTMP; // For TextMeshPro users
    [SerializeField] private Image uiBackground; // For a background image, if any

    // Declare an event for when the coins change
    public event System.Action OnCoinsChanged;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        transform.parent = null;  // Ensure the CurrencySystem is not a child of another object
        DontDestroyOnLoad(gameObject);  // Keep it alive across scenes
    }

    private void Start()
    {
        PlayerPrefs.DeleteKey(CURRENCY_SAVE_KEY); // Optional: Remove saved coin value
        LoadCoins();
        UpdateCoinDisplay();
        UpdateCoinSprite();
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
        OnCoinsChanged?.Invoke();  // Trigger the event when coins change
    }

    public void RemoveCoins(int amount)
    {
        if (TotalCoins >= amount)
        {
            TotalCoins -= amount;
            PlayerPrefs.SetInt(CURRENCY_SAVE_KEY, TotalCoins);
            PlayerPrefs.Save();
            OnCoinsChanged?.Invoke();  // Trigger the event when coins change
        }
    }

    private void UpdateCoinDisplay()
    {
        if (coinDisplayText != null)
        {
            coinDisplayText.text = "Coins: " + TotalCoins.ToString();
        }
        else if (coinDisplayTextTMP != null)
        {
            coinDisplayTextTMP.text = "Coins: " + TotalCoins.ToString();
        }
    }

    private void UpdateCoinSprite()
    {
        if (coinImage != null && coinSprite != null)
        {
            coinImage.sprite = coinSprite;
        }
    }
}

