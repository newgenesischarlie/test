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

    public Turret Turret { get; set; }
    private float _rangeSize;
    private Vector3 _rangeOriginalSize;

    private void Start()
    {
        _rangeSize = attackRangeSprite.GetComponent<SpriteRenderer>().bounds.size.y;
        _rangeOriginalSize = attackRangeSprite.transform.localScale;
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
          // CurrencySystem.Instance.AddCoins(Turret.TurretUpgrade.GetSellValue());
            Destroy(Turret.gameObject);
            Turret = null;
            attackRangeSprite.SetActive(false);
            OnTurretSold?.Invoke();
            CloseTurretInfo();
        }
    }

    private void ShowTurretInfo()
    {
        // Assuming the turret has a 'TurretUpgrade' class with properties like 'Damage' and 'Level'
        if (Turret != null)
        {
            turretInfoPanel.SetActive(true); // Show the info panel

            // Set the text values for turret damage and level
          //  turretDamageText.text = "Damage: " + Turret.TurretUpgrade.Damage.ToString();
          //  turretLevelText.text = "Level: " + Turret.TurretUpgrade.Level.ToString();
        }
    }

    private void CloseTurretInfo()
    {
        turretInfoPanel.SetActive(false); // Hide the info panel when the turret is sold
    }
}
