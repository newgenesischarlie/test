using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgrade : MonoBehaviour
{
    [SerializeField] private int upgradeCost = 100;
    [SerializeField] private float damageMultiplier = 1.5f;
    [SerializeField] private float rangeMultiplier = 1.2f;
    [SerializeField] private float fireRateMultiplier = 1.3f;

    [Header("Sell")]
    [Range(0, 1)]
    [SerializeField] private float sellPert;

    // Public properties for accessing values externally, if needed
    public float SellPerc { get; private set; }
    public int UpgradeCost { get; private set; }
    public int Level { get; private set; }

    private Turret turret;
    private GameManager gameManager;

    // Unity Start method (should be capitalized)
    private void Start()
    {
        turret = GetComponent<Turret>();
        gameManager = FindObjectOfType<GameManager>();

        // Initialize upgrade cost, sell percentage, and level
        UpgradeCost = upgradeCost;
        SellPerc = sellPert;
        Level = 1;
    }

    // Method to upgrade the turret if the player has enough coins
    public void UpgradeTurret()
    {
        // Check if we have enough currency - implement your own check here
        // since GameManager doesn't have SpendCurrency
        if (CanAffordUpgrade())
        {
            // Upgrade turret stats
            if (turret != null)
            {
                UpgradeTurretStats();
            }

            // Update visuals or effects
            // ...
        }
    }

    private bool CanAffordUpgrade()
    {
        // Implement your own currency check here
        // For example:
        if (gameManager != null)
        {
            // Assuming GameManager has a Currency property
            return gameManager.Currency >= upgradeCost;
        }
        return false;
    }

    private void UpgradeTurretStats()
    {
        // Implement the upgrade logic directly here
        if (turret != null)
        {
            // Update fireRate and range as floats
            turret.fireRate *= fireRateMultiplier;
            turret.range *= rangeMultiplier;
            
            // For damage, convert float to int using RoundToInt
            turret.damage = Mathf.RoundToInt(turret.damage * damageMultiplier);
        }
    }

    // Get the sell value for the turret (how much player gets back)
    public int GetSellValue()
    {
        int sellValue = Mathf.RoundToInt(UpgradeCost * SellPerc);
        return sellValue;
    }

    // Private method to handle the upgrade costs and level increment
    private void UpdateUpgrade()
    {
        // Remove the coins for the upgrade
        CurrencySystem.Instance.RemoveCoins(UpgradeCost);

        // Increment the upgrade cost for the next upgrade
        UpgradeCost += upgradeCost;

        // Increase the turret's upgrade level
        Level++;
    }
}

