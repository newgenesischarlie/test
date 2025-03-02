using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopUI; // The UI for the shop panel
    [SerializeField] private Button[] weaponButtons; // Buttons for buying weapons
    [SerializeField] private int[] weaponPrices; // Prices for the weapons
    [SerializeField] private GameObject[] weaponPrefabs; // Prefabs for the weapons (turrets)

    private Plot currentPlot;

    private void Start()
    {
        // Initially hide the shop UI
        shopUI.SetActive(false);

        // Set up weapon button listeners
        for (int i = 0; i < weaponButtons.Length; i++)
        {
            int index = i;
            weaponButtons[i].onClick.AddListener(() => BuyWeapon(index));
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

            // Place the weapon (turret) at the plot's position
            Instantiate(weaponPrefabs[weaponIndex], currentPlot.transform.position, Quaternion.identity);

            // Close the shop UI after purchase
            shopUI.SetActive(false);
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
