using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class WeaponData
    {
        public string name;
        public GameObject weaponPrefab;
        public int cost;
        public Sprite icon;
    }

    [Header("Shop Settings")]
    [SerializeField] private WeaponData weapon; // Only one weapon now
    [SerializeField] private GameObject shopUI;
    [SerializeField] private Button weaponButton; // Only one button now

    private Plot selectedPlot;
    private CurrencySystem currencySystem;

    private void Start()
    {
        currencySystem = FindObjectOfType<CurrencySystem>();
        InitializeShop();

        // Hide shop UI at start
        if (shopUI != null)
        {
            shopUI.SetActive(false);
        }
    }

    private void InitializeShop()
    {
        // Set up the weapon button
        if (weaponButton != null)
        {
            // Set button icon
            Image buttonImage = weaponButton.GetComponent<Image>();
            if (buttonImage != null && weapon.icon != null)
            {
                buttonImage.sprite = weapon.icon;
            }

            // Set cost text
            Text buttonText = weaponButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = weapon.cost.ToString();
            }

            // Add click listener
            weaponButton.onClick.AddListener(() => TryPurchaseWeapon(weapon));
        }
    }

    public void SelectPlot(Plot plot)
    {
        selectedPlot = plot;
        if (shopUI != null)
        {
            shopUI.SetActive(true);
        }
    }

    private void TryPurchaseWeapon(WeaponData weapon)
    {
        if (selectedPlot == null || selectedPlot.IsOccupied())
        {
            Debug.LogWarning("No plot selected or plot is occupied");
            return;
        }

        if (currencySystem != null && currencySystem.TotalCoins >= weapon.cost)
        {
            // Purchase successful
            currencySystem.RemoveCoins(weapon.cost);
            selectedPlot.PlaceWeapon(weapon.weaponPrefab);
            CloseShop();
        }
        else
        {
            Debug.Log("Not enough coins to purchase " + weapon.name);
            // Optionally show a message to the player
        }
    }

    public void CloseShop()
    {
        if (shopUI != null)
        {
            shopUI.SetActive(false);
        }
        selectedPlot = null;
    }
}
