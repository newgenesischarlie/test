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
    Debug.Log("Plot selected: " + selectedPlot.GetPlotIndex()); // Debug message to confirm plot selection
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

    if (currencySystem != null)
    {
        Debug.Log("Current Coins: " + currencySystem.TotalCoins);
        if (currencySystem.TotalCoins >= weapon.cost)
        {
            currencySystem.RemoveCoins(weapon.cost);
            Debug.Log("Coins removed, placing weapon...");
            selectedPlot.PlaceWeapon(weapon.weaponPrefab); // Place the weapon on the plot
            Debug.Log("Weapon placed: " + weapon.name);
            CloseShop();
        }
        else
        {
            Debug.LogWarning("Not enough coins to purchase " + weapon.name);
        }
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
