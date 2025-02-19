using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes : MonoBehaviour
{
    public static Action<Node> OnNodeSelected;
    public static Action OnTurretSold;

    [SerializedField] private GameObject attackRangeSpriite;
    Public Turret Turret { get; set; }
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
    public void SellTurret()
    {
        if (!IsEmpty())
        {
            CurrencySystem.Instance.AddCoins(TurretUpgrade.GetSellValue());
            Destroy(Turret.gameObject);
            Turret = null;
            attackRangeSprite.SetActive(false);
            OnTurretSold?.Invoke();
        }


    }
}