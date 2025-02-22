using UnityEngine;
using UnityEngine.UI;
using System;

public class Node : MonoBehaviour
{
    public static Action<Node> OnNodeSelected;
    public static Action OnTurretSold;

    [SerializeField] private GameObject attackRangeSprite;
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
            }
            else
            {
                Debug.Log("Not enough coins to upgrade the turret!");
            }
        }
    }


}
