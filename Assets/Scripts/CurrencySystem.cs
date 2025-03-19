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

    [Header("Coin Generation")]
    [SerializeField] private int coinsPerInterval = 5; // How many coins to generate each time
    [SerializeField] private float coinGenerationInterval = 5f; // Interval in seconds

    // Declare an event for when the coins change
    public event System.Action OnCoinsChanged;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("CurrencySystem instance set.");
        }
        else
        {
            Debug.LogWarning("Multiple CurrencySystem instances detected!");
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

        // Start the coin generation
        InvokeRepeating("GenerateCoins", 0f, coinGenerationInterval); // Start generating coins immediately

        // Add listener for coin change
        OnCoinsChanged += UpdateCoinDisplay;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        OnCoinsChanged -= UpdateCoinDisplay;
    }

    private void LoadCoins()
    {
        TotalCoins = PlayerPrefs.GetInt(CURRENCY_SAVE_KEY, coinTest);
        Debug.Log("Loaded Coins: " + TotalCoins); // Debugging coin loading
    }

    // Function to generate coins every few seconds
    private void GenerateCoins()
    {
        AddCoins(coinsPerInterval);
    }

    public void AddCoins(int amount)
    {
        TotalCoins += amount;
        PlayerPrefs.SetInt(CURRENCY_SAVE_KEY, TotalCoins);
        PlayerPrefs.Save();
        Debug.Log("Added " + amount + " coins. Total Coins: " + TotalCoins); // Debug message for adding coins
        OnCoinsChanged?.Invoke();  // Trigger the event when coins change
        UpdateCoinDisplay(); // Update the UI display
    }

    public void RemoveCoins(int amount)
    {
        if (TotalCoins >= amount)
        {
            TotalCoins -= amount;
            PlayerPrefs.SetInt(CURRENCY_SAVE_KEY, TotalCoins);
            PlayerPrefs.Save();
            Debug.Log("Removed " + amount + " coins. Total Coins: " + TotalCoins); // Debug message for removing coins
            OnCoinsChanged?.Invoke();  // Trigger the event when coins change
            UpdateCoinDisplay(); // Update the UI display
        }
        else
        {
            Debug.Log("Not enough coins to remove. Current Coins: " + TotalCoins);
        }
    }

    private void UpdateCoinDisplay()
    {
        // Update the UI display for coins
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
