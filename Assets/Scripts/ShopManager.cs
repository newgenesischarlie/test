using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private WeaponDatabase weaponDatabase; // Reference to the weapon database
    private Plot selectedPlot;
    private CurrencySystem currencySystem;

    [Header("Shop Settings")]
    [SerializeField] private GameObject shopUI;
    [SerializeField] private Button weaponButton; // Button to show weapon cost

    // Global cost for all weapons
    public int globalWeaponCost = 50;  // Set this value to the desired cost for all weapons

    private WeaponDatabase.WeaponData selectedWeapon;

    private void Start()
    {
        weaponDatabase = FindObjectOfType<WeaponDatabase>();
        currencySystem = FindObjectOfType<CurrencySystem>();

        // Set the button label with the global cost
        SetWeaponButtonCost(globalWeaponCost);

        // Initialize the shop
        InitializeShop();

        // Hide shop UI at start
        if (shopUI != null)
        {
            shopUI.SetActive(false);
        }
    }

    private void InitializeShop()
    {
        if (weaponButton != null)
        {
            // Set up the weapon button to handle weapon selection
            weaponButton.onClick.AddListener(OnWeaponButtonClick);
        }
    }

    public void SelectPlot(Plot plot, string weaponName)
    {
        selectedPlot = plot;
        selectedWeapon = weaponDatabase.GetWeaponByName(weaponName); // Get weapon by name

        // Debugging: Check the name being passed
        Debug.Log("Weapon name passed to SelectPlot: " + weaponName);

        // Check if selectedWeapon is null
        if (selectedWeapon == null)
        {
            Debug.LogError("Weapon with name " + weaponName + " not found in WeaponDatabase.");
            return;
        }

        Debug.Log("Plot selected: " + selectedPlot.GetPlotIndex() + " Weapon selected: " + selectedWeapon.name);

        // Set the global cost on the button
        SetWeaponButtonCost(globalWeaponCost);

        if (shopUI != null)
        {
            shopUI.SetActive(true); // Show the shop UI
        }
    }

    private void OnWeaponButtonClick()
    {
        if (selectedPlot == null || selectedWeapon == null)
        {
            Debug.LogWarning("No plot or weapon selected");
            return;
        }

        // Check if the player has enough coins to purchase the weapon
        if (currencySystem.TotalCoins >= globalWeaponCost)
        {
            currencySystem.RemoveCoins(globalWeaponCost); // Deduct the global cost from player's coins
            selectedPlot.PlaceWeapon(selectedWeapon.weaponPrefab); // Place weapon on the plot
            Debug.Log("Weapon placed: " + selectedWeapon.name);
            CloseShop();
        }
        else
        {
            Debug.LogWarning("Not enough coins to purchase " + selectedWeapon.name);
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

    // Update the cost text on the weapon button
    private void SetWeaponButtonCost(int cost)
    {
        Text buttonText = weaponButton.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = "Cost: " + cost.ToString(); // Set cost directly on the button text
        }
    }
}
