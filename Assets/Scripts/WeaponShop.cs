using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponShop : MonoBehaviour
{
    [Header("Weapon Types")]
    [SerializeField] private WeaponType[] weaponTypes;
    
    [Header("UI References")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Button[] weaponButtons; // One button per weapon type
    [SerializeField] private Text[] costTexts;
    [SerializeField] private Image[] weaponIcons;
    
    private Plot selectedPlot;
    private CurrencySystem currencySystem;
    
    private void Start()
    {
        currencySystem = FindObjectOfType<CurrencySystem>();
        SetupShopButtons();
        CloseShop();
    }
    
    private void SetupShopButtons()
    {
        // Set up each button with its weapon type
        for (int i = 0; i < Mathf.Min(weaponButtons.Length, weaponTypes.Length); i++)
        {
            WeaponType weaponType = weaponTypes[i];
            
            // Set cost text
            if (costTexts.Length > i && costTexts[i] != null)
                costTexts[i].text = weaponType.cost.ToString();
                
            // Set weapon icon
            if (weaponIcons.Length > i && weaponIcons[i] != null && weaponType.weaponIcon != null)
                weaponIcons[i].sprite = weaponType.weaponIcon;
                
            // Set click handler
            int index = i; // Capture index for lambda
            if (weaponButtons[i] != null)
                weaponButtons[i].onClick.AddListener(() => PurchaseWeapon(index));
        }
    }
    
    public void SelectPlot(Plot plot)
    {
        if (plot.IsOccupied())
        {
            // Optionally show upgrade menu instead
            return;
        }
        
        selectedPlot = plot;
        OpenShop();
    }
    
    private void OpenShop()
    {
        if (shopPanel != null)
            shopPanel.SetActive(true);
    }
    
    public void CloseShop()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);
        
        selectedPlot = null;
    }
    
    private void PurchaseWeapon(int weaponTypeIndex)
    {
        if (selectedPlot == null || weaponTypeIndex < 0 || weaponTypeIndex >= weaponTypes.Length)
            return;
            
        WeaponType selectedType = weaponTypes[weaponTypeIndex];
        
        // Check if player has enough currency
        if (currencySystem != null && currencySystem.TotalCoins >= selectedType.cost)
        {
            // Purchase successful
            currencySystem.RemoveCoins(selectedType.cost);
            
            // Place the weapon
            GameObject weaponObj = selectedPlot.PlaceWeapon(selectedType.weaponPrefab);
            
            // Configure the weapon
            if (weaponObj != null)
            {
                Weapon weapon = weaponObj.GetComponent<Weapon>();
                if (weapon != null)
                {
                    weapon.range = selectedType.range;
                    weapon.fireRate = selectedType.fireRate;
                    weapon.damage = selectedType.damage;
                    
                    // Activate the weapon
                    weaponObj.SetActive(true);
                }
            }
            
            CloseShop();
        }
        else
        {
            Debug.Log("Not enough coins!");
            // Optionally show a message to the player
        }
    }
} 