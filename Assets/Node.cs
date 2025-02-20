using UnityEngine;
using UnityEngine.UI;
using System;

public class Node : MonoBehaviour
{
    public static Action<Node> OnNodeSelected;
    public static Action OnTurretSold;

    [SerializeField] private GameObject attackRangeSprite;
    [SerializeField] private GameObject turretInfoPanel; // Reference to the UI panel displaying turret info
    [SerializeField] private Text turretDamageText; // Reference to a Text component for displaying turret damage
    [SerializeField] private Text turretLevelText;  // Reference to a Text component for displaying turret level
    [SerializeField] private Button upgradeButton;  // Button to trigger turret upgrade

    public Turret Turret { get; set; }
    private float _rangeSize;
    private Vector3 _rangeOriginalSize;

    private void Start()
    {
        _rangeSize = attackRangeSprite.GetComponent<SpriteRenderer>().bounds.size.y;
        _rangeOriginalSize = attackRangeSprite.transform.localScale;

        // Set the upgrade button listener
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(UpgradeTurret);
        }
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
            ShowTurretInfo();
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
            CloseTurretInfo();
        }
    }

    // Upgrade the turret and update UI
    public void UpgradeTurret()
    {
        if (!IsEmpty())
        {
            TurretUpgrade turretUpgrade = Turret.GetComponent<TurretUpgrade>();

            if (CurrencySystem.Instance.TotalCoins >= turretUpgrade.UpgradeCost)
            {
                // Deduct the coins for upgrading
                CurrencySystem.Instance.RemoveCoins(turretUpgrade.UpgradeCost);

                // Call the UpgradeTurret method from TurretUpgrade script
                turretUpgrade.UpgradeTurret();

                // Update the UI to reflect the new turret stats
                ShowTurretInfo();
            }
            else
            {
                Debug.Log("Not enough coins to upgrade the turret!");
            }
        }
    }

    private void ShowTurretInfo()
    {
        if (Turret != null)
        {
            TurretUpgrade turretUpgrade = Turret.GetComponent<TurretUpgrade>();

            if (turretUpgrade != null)
            {
                turretInfoPanel.SetActive(true); // Show the info panel

                // Set the text values for turret damage and level from TurretUpgrade
                turretLevelText.text = "Level: " + turretUpgrade.Level.ToString();
            }
        }
    }

    private void CloseTurretInfo()
    {
        turretInfoPanel.SetActive(false); // Hide the info panel when the turret is sold
    }
}
