using UnityEngine;
public class CurrencySystem : MonoBehaviour
{
    public static CurrencySystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }

        DontDestroyOnLoad(gameObject); // Persist across scenes
    }

    // AddCoins now accepts an Enemy parameter
    public void AddCoins(Enemy enemy)
    {
        int coinsToAdd = CalculateCoinsFromEnemy(enemy);  // You can define how to calculate coins here
        TotalCoins += coinsToAdd;

        // Update the UI and save the new coin total
        if (coinCountText != null)
        {
            coinCountText.text = "Coins: " + TotalCoins;
        }

        PlayerPrefs.SetInt(CURRENCY_SAVE_KEY, TotalCoins);
        PlayerPrefs.Save();

        // Instantiate coin images if necessary
        for (int i = 0; i < coinsToAdd; i++)
        {
            Image newCoin = Instantiate(coinImagePrefab, coinPanel);
            newCoin.gameObject.SetActive(true); // Make sure coin is visible
        }
    }

    private int CalculateCoinsFromEnemy(Enemy enemy)
    {
        // Example: You can base coins on enemy health, type, or other logic
        return 10;  // For example, always give 10 coins for each enemy
    }
}
