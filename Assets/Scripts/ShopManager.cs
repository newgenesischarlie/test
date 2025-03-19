using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private WeaponDatabase weaponDatabase;
    private Plot selectedPlot;
    private CurrencySystem currencySystem;

    [Header("Shop Settings")]
    [SerializeField] private GameObject shopUI;
    [SerializeField] private Button weaponButton;

    public int globalWeaponCost = 50;

    private WeaponDatabase.WeaponData selectedWeapon;

    private void Start()
    {
        weaponDatabase = FindObjectOfType<WeaponDatabase>();
        currencySystem = FindObjectOfType<CurrencySystem>();

        SetWeaponButtonCost(globalWeaponCost);
        InitializeShop();

        if (shopUI != null)
        {
            shopUI.SetActive(false);
        }
    }

    private void InitializeShop()
    {
        if (weaponButton != null)
        {
            weaponButton.onClick.AddListener(OnWeaponButtonClick);
        }
    }

    public void SelectPlot(Plot plot, string weaponName)
    {
        selectedPlot = plot;
        selectedWeapon = weaponDatabase.GetWeaponByName(weaponName);

        if (selectedWeapon == null)
        {
            Debug.LogError("Weapon with name " + weaponName + " not found in WeaponDatabase.");
            return;
        }

        SetWeaponButtonCost(globalWeaponCost);

        if (shopUI != null)
        {
            shopUI.SetActive(true);
        }
    }

    private void OnWeaponButtonClick()
    {
        if (selectedPlot == null || selectedWeapon == null)
        {
            Debug.LogWarning("No plot or weapon selected");
            return;
        }

        if (currencySystem.TotalCoins >= globalWeaponCost)
        {
            currencySystem.RemoveCoins(globalWeaponCost);
            selectedPlot.PlaceWeapon(selectedWeapon.weaponPrefab);
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

    private void SetWeaponButtonCost(int cost)
    {
        Text buttonText = weaponButton.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = "Cost: " + cost.ToString();
        }
    }
}
