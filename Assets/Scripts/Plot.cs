using UnityEngine;
using UnityEngine.UI;

public class PlotScript : MonoBehaviour
{
    public int plotCost = 10;  // Cost of the plot
    private bool isOwned = false;  // Whether the plot is bought or not

    // Reference to the weapon selection menu UI
    [SerializeField] private GameObject weaponMenuUI;  // Reference to the weapon selection menu
    private GameObject selectedWeapon;  // The selected weapon prefab to be placed on the plot

    private void OnMouseDown()
    {
        if (isOwned)
        {
            return;  // If the plot is already owned, do nothing
        }

        if (CurrencySystem.Instance.TotalCoins >= plotCost)
        {
            // Deduct the cost of the plot
            CurrencySystem.Instance.RemoveCoins(plotCost);
            isOwned = true;
            OpenWeaponSelectionMenu();  // Open the weapon selection menu
        }
        else
        {
            Debug.Log("Not enough coins to buy this plot.");
        }
    }

    // Open the weapon selection menu
    private void OpenWeaponSelectionMenu()
    {
        weaponMenuUI.SetActive(true);  // Show the weapon selection menu
        weaponMenuUI.GetComponent<WeaponSelection>().SetCurrentPlot(this);  // Pass the plot reference to the selection menu
    }

    // Place the weapon on the plot
    public void PlaceWeapon(GameObject weaponPrefab)
    {
        selectedWeapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        weaponMenuUI.SetActive(false);  // Close the weapon selection menu
    }

    public bool IsOwned()
    {
        return isOwned;
    }
}

