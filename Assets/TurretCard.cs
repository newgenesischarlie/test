using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;  // For Image

public class TurretCard : MonoBehaviour
{
    public static Action<TurretSetting> OnPlaceTurret;

    [SerializeField] private Image turretImage;  // Image to display turret sprite
    [SerializeField] private TextMeshProUGUI turretCost;  // TextMeshPro for the turret cost
    public TurretSetting TurretLoaded { get; set; }  // The TurretSetting that is loaded into this card

    // Set up the turret card's display information
    public void SetupTurretButton(TurretSetting turretSetting)
    {
        TurretLoaded = turretSetting;

        // Set the turret image and cost text
        turretImage.sprite = turretSetting.TurretShopSprite;
        turretCost.text = turretSetting.TurretShopCost.ToString();
    }

    // Method to place the turret, checking if the player has enough coins
    public void PlaceTurret()
    {
        if (CurrencySystem.Instance.TotalCoins >= TurretLoaded.TurretShopCost)
        {
            // Deduct the cost from the player's coins
            CurrencySystem.Instance.RemoveCoins(TurretLoaded.TurretShopCost);

            // Close the turret shop panel (assuming UIManager is a singleton)
            UIManager.Instance.CloseTurretShopPanel();

            // Trigger the event to place the turret
            OnPlaceTurret?.Invoke(TurretLoaded);
        }
        else
        {
            // Optionally, add some feedback for the player if they can't afford the turret
            Debug.Log("Not enough coins to place the turret!");
        }
    }
}
