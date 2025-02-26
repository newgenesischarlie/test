using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencySystem : MonoBehaviour
{
    [SerializeField] private int coinTest;  // Default coins if PlayerPrefs is not set
    private string CURRENCY_SAVE_KEY = "MYGAME_CURRENCY";
    public int TotalCoins { get; private set; }
    public static CurrencySystem Instance { get; private set; }  // Singleton reference

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
    }
    public void RemoveCoins(int amount)
    {
        if (TotalCoins >= amount)
        {
            TotalCoins -= amount;
            PlayerPrefs.DeleteKey(CURRENCY_SAVE_KEY);  // Debug step; you can remove this later.
            LoadCoins();  // Load coin data from PlayerPrefs
        }
    }

    private void AddCoins(Enemy enemy)
    {
        // Add coins when an enemy is killed
         //AddCoins(enemy.DeathCoinReward);
    }

    private void OnEnable()
    {
        EnemyHealth.OnEnemyKilled += AddCoins;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyKilled -= AddCoins;
    }
}