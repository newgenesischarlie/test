using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopUI; // The UI for the shop panel
    [SerializeField] private Button[] weaponButtons; // Buttons for buying weapons
    [SerializeField] private int[] weaponPrices; // Prices for the weapons
    [SerializeField] private GameObject[] weaponPrefabs; // Prefabs for the weapons (turrets)
    private Plot currentPlot; // Reference to the plot where the weapon will be placed

    private void Start()
    {
        // Initially hide the shop UI and ensure all weapons are inactive
        shopUI.SetActive(false);
        DeactivateAllWeapons();

        // Set up weapon button listeners
        for (int i = 0; i < weaponButtons.Length; i++)
        {
            int index = i;
            weaponButtons[i].onClick.AddListener(() => BuyWeapon(index));
        }
    }

    // Method to deactivate all weapons at the start
    private void DeactivateAllWeapons()
    {
        foreach (GameObject weapon in weaponPrefabs)
        {
            weapon.SetActive(false); // Ensure all weapons are deactivated
        }
    }

    // Call this method when a plot is clicked
    public void OpenShop(Plot plot)
    {
        currentPlot = plot;
        shopUI.SetActive(true);
    }

    // Method to buy a weapon
   private void BuyWeapon(int weaponIndex)
{
    if (CurrencySystem.Instance.TotalCoins >= weaponPrices[weaponIndex])
    {
        CurrencySystem.Instance.RemoveCoins(weaponPrices[weaponIndex]);

        // Check if the weapon prefab is valid
        if (weaponPrefabs[weaponIndex] != null)
        {
            // Instantiate the weapon prefab at the plot's position
            GameObject weapon = Instantiate(weaponPrefabs[weaponIndex], currentPlot.transform.position, Quaternion.identity);

            // Deactivate the weapon initially
            weapon.SetActive(false);

            // After buying, activate the weapon
            weapon.SetActive(true);

            // Optionally, if the weapon has specific logic tied to the plot, we can attach it to the plot here.
            // e.g., currentPlot.SetWeapon(weapon);

            // Close the shop UI after purchase
            shopUI.SetActive(false);
        }
        else
        {
            Debug.LogError("Weapon prefab is missing or invalid for weapon index " + weaponIndex);
        }
    }
    else
    {
        Debug.Log("Not enough coins to buy this weapon.");
    }
}


    // Close the shop UI manually if needed
    public void CloseShop()
    {
        shopUI.SetActive(false);
    }
}
