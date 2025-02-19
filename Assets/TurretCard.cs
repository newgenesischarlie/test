using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCard : MonoBehaviour
{
    public static Action<TurretSettings> OnPlaceTurret;

    [SerializeField] private Image turretImage;
    [SerializeField] private TextMeshProUGUI turretCost;
    public TurretSettings TurretLoaded { get; set; }
    public void SetupTurretButton(TurretSettings turretSettings)
    {
    TurretLoaded = turretSettings;
    TurretImage.sprite = turretSettings.TurretShopSprite;
    TurretCost.text = turretSettings.TurretShopCost.ToString();

    }
    public void PlaceTurret()
    {
    if(CurrencySystem.Instance.TotalCoins >= TurretLoaded.TurretShopCost)
    {
        CurrencySystem.Instance.RemoveCoins(TurretLoaded.TurretShopCost);
        UIManger.Instance.CloseTurretShopPanel();
        OnPlaceTurret?.Invoke(TurretLoaded);
    }
    }
}
