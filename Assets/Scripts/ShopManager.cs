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
    [SerializeField] private WeaponData[] availableWeapons;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private Transform weaponButtonsContainer;
    [SerializeField] private Button weaponButtonPrefab;

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
        // Clear existing buttons
        foreach (Transform child in weaponButtonsContainer)
        {
            Destroy(child.gameObject);
        }

        // Create buttons for each weapon
        foreach (WeaponData weapon in availableWeapons)
        {
            CreateWeaponButton(weapon);
        }
    }

    private void CreateWeaponButton(WeaponData weapon)
    {
        Button newButton = Instantiate(weaponButtonPrefab, weaponButtonsContainer);
        
        // Set button icon
        Image buttonImage = newButton.GetComponent<Image>();
        if (buttonImage != null && weapon.icon != null)
        {
            buttonImage.sprite = weapon.icon;
        }

        // Set cost text
        Text buttonText = newButton.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = weapon.cost.ToString();
        }

        // Add weapon name as tooltip or label if you have a Text component for it
        Text nameText = newButton.transform.Find("NameText")?.GetComponent<Text>();
        if (nameText != null)
        {
            nameText.text = weapon.name;
        }

        // Add click listener
        newButton.onClick.AddListener(() => TryPurchaseWeapon(weapon));
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
