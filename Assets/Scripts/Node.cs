using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;  // If you're using TextMeshPro for text elements

public class Node : MonoBehaviour
{
    public static Action<Node> OnNodeSelected;
    public static Action OnTurretSold;

    [SerializeField] private GameObject attackRangeSprite;
    [SerializeField] private GameObject turretInfoPanel;  // Reference to the info panel
    [SerializeField] private TextMeshProUGUI turretNameText;  // Reference to the TextMeshProUGUI for turret name
    [SerializeField] private TextMeshProUGUI turretCostText;  // Reference to the TextMeshProUGUI for turret cost
    [SerializeField] private TextMeshProUGUI turretDescriptionText;  // Reference to the description of the turret

    public Turret Turret { get; set; }
    private float _rangeSize;
    private Vector3 _rangeOriginalSize;

    private void Start()
    {
        _rangeSize = attackRangeSprite.GetComponent<SpriteRenderer>().bounds.size.y;
        _rangeOriginalSize = attackRangeSprite.transform.localScale;
        turretInfoPanel.SetActive(false);  // Hide the turret info panel initially
    }

    public void SetTurret(Turret turret)
    {
        Turret = turret;
    }

    public bool IsEmpty()
    {
        return Turret == null;
    }

    public void CloseAttackRangeSprite()
    {
        attackRangeSprite.SetActive(false);
    }

    public void SelectTurret()
    {
        OnNodeSelected?.Invoke(this);
        if (!IsEmpty())
        {
            ShowTurretInfo();  // Call the method to show turret info
        }
    }

    public void SellTurret()
    {
        if (!IsEmpty())
        {
            TurretUpgrade turretUpgrade = Turret.GetComponent<TurretUpgrade>();
            CurrencySystem.Instance.AddCoins(turretUpgrade.GetSellValue());
            Destroy(Turret.gameObject);
            Turret = null;
            attackRangeSprite.SetActive(false);
            OnTurretSold?.Invoke();
        }
    }

    public void UpgradeTurret()
    {
        if (!IsEmpty())
        {
            TurretUpgrade turretUpgrade = Turret.GetComponent<TurretUpgrade>();

            if (CurrencySystem.Instance.TotalCoins >= turretUpgrade.UpgradeCost)
            {
                CurrencySystem.Instance.RemoveCoins(turretUpgrade.UpgradeCost);
                turretUpgrade.UpgradeTurret();
            }
            else
            {
                Debug.Log("Not enough coins to upgrade the turret!");
            }
        }
    }

    // Show turret information on a UI panel
    public void ShowTurretInfo()  // Make this method public
    {
        turretInfoPanel.SetActive(true);  // Show the turret info panel

        // Update the text fields with turret information
        turretNameText.text = Turret.name;  // Assuming Turret has a name
        turretCostText.text = "Cost: " + Turret.GetComponent<TurretUpgrade>().UpgradeCost.ToString();  // Example for cost
        //ßturretDescriptionText.text = Turret.GetComponent<TurretUpgrade>().GetDescription();  // Assuming a method to get the description
    }

    // Close the turret info panel when you are done
    public void CloseTurretInfo()
    {
        turretInfoPanel.SetActive(false);
    }

    public void OnButtonClick_ShowTurretInfo()
    {
        ShowTurretInfo();
    }

}
