using UnityEngine;
using UnityEngine.UI; // For Unity's built-in Text component
using TMPro; // For TextMeshPro if you prefer that

public class CoinDisplay : MonoBehaviour
{
    // Reference to the Text or TextMeshProUGUI component to display the coins
    [SerializeField] private Text coinDisplayText; // For Unity's built-in Text component
    [SerializeField] private TextMeshProUGUI coinDisplayTextTMP; // For TextMeshPro users

    private void Start()
    {
        // Ensure that we are starting with the correct coin display
        UpdateCoinDisplay();
    }

    private void OnEnable()
    {
        // Subscribe to the event when coins are updated
        CurrencySystem.Instance.OnCoinsChanged += UpdateCoinDisplay;
    }

    private void OnDisable()
    {
        // Unsubscribe when the object is disabled or destroyed
        CurrencySystem.Instance.OnCoinsChanged -= UpdateCoinDisplay;
    }

    // Update the coin display UI
    private void UpdateCoinDisplay()
    {
        if (coinDisplayText != null)
        {
            coinDisplayText.text = "Coins: " + CurrencySystem.Instance.TotalCoins.ToString();  // Update text
        }
        else if (coinDisplayTextTMP != null)
        {
            coinDisplayTextTMP.text = "Coins: " + CurrencySystem.Instance.TotalCoins.ToString();  // For TextMeshPro
        }
    }
}

