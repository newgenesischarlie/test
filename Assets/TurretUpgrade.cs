using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgrade : MonoBehaviour
{
    [SerializeField] private int upgradeInitialCost;
    [SerializeField] private int upgradeCostIncremental;
    [SerializeField] private float damageIncremental;
    [SerializeField] private float delayReduce;

    [Header("Sell")]
    [Range(0, 1)]
    [SerializeField] private float sellPert;

    // Public properties for accessing values externally, if needed
    public float SellPerc { get; private set; }
    public int UpgradeCost { get; private set; }
    public int Level { get; private set; }

    private TurretProjectile _turretProjectile;

    // Unity Start method (should be capitalized)
    private void Start()
    {
        _turretProjectile = GetComponent<TurretProjectile>();

        // Initialize upgrade cost, sell percentage, and level
        UpgradeCost = upgradeInitialCost;
        SellPerc = sellPert;
        Level = 1;
    }

    // Method to upgrade the turret if the player has enough coins
   // public void UpgradeTurret()
  //  {
        // Check if the player has enough coins
      //  if (CurrencySystem.Instance.TotalCoins >= UpgradeCost)
       // {
            // Upgrade turret attributes
         //   _turretProjectile.Damage += damageIncremental;
         //   _turretProjectile.DelayPerShot -= delayReduce;

            // Update cost and level after upgrading
         //   UpdateUpgrade();
       // }
      //  else
       // {
            // Optionally, you could add some feedback here, e.g., a sound or message to indicate insufficient coins
        //    Debug.Log("Not enough coins to upgrade!");
       // }
   // }

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
      //  CurrencySystem.Instance.RemoveCoins(UpgradeCost);

        // Increment the upgrade cost for the next upgrade
        UpgradeCost += upgradeCostIncremental;

        // Increase the turret's upgrade level
        Level++;
    }
}

