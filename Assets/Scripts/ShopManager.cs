
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopUI; // The UI for the shop panel
  //  [SerializeField] private Button[] weaponButtons; // Buttons for buying weapons
    [SerializeField] private int[] weaponPrices; // Prices for the weapons
    [SerializeField] private GameObject[] weaponPrefabs; // Prefabs for the weapons (turrets)
    [SerializeField] private GameObject weaponContainer; // Container to hold all weapons
    private Plot currentPlot; // Reference to the plot where the weapon will be placed


    private void Start()
    {
        // Initially hide the shop UI
       // shopUI.SetActive(false);

        // Set up weapon button listeners
      //  for (int i = 0; i < weaponButtons.Length; i++)
      //  {
      //      int index = i;
      //      weaponButtons[i].onClick.AddListener(() => BuyWeapon(index));
      //  }
    }

    // Call this method when a plot is clicked
 //   public void OpenShop(Plot plot)
 //   {
 //       currentPlot = plot;
 //       shopUI.SetActive(true);
 //   }

    // Method to buy a weapon
   private void BuyWeapon(int weaponIndex)
{
    if (weaponIndex < 0 || weaponIndex >= weaponPrefabs.Length)
    {
        Debug.LogError("Weapon index is out of range.");
        return; // Exit if index is invalid
    }

    if (weaponPrefabs[weaponIndex] == null)
    {
        Debug.LogError("Weapon prefab is missing or invalid for weapon index " + weaponIndex);
        return; // Exit if the weapon prefab is not assigned or invalid
    }

    if (CurrencySystem.Instance.TotalCoins >= weaponPrices[weaponIndex])
    {
        CurrencySystem.Instance.RemoveCoins(weaponPrices[weaponIndex]);

        // Place the weapon (turret) at the plot's position
        GameObject weaponInstance = Instantiate(weaponPrefabs[weaponIndex], currentPlot.transform.position, Quaternion.identity);

        // Set the parent of the weapon to the WeaponContainer (not the plot)
        weaponInstance.transform.SetParent(weaponContainer.transform); 

        // Ensure the weapon is visible and active
        weaponInstance.SetActive(true); // Make sure the weapon is active after instantiation

        // Close the shop UI after purchase
        shopUI.SetActive(false);

        Debug.Log("Weapon bought and placed at plot!");
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
