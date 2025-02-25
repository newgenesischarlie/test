using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTurretSetting", menuName = "Turret/Setting", order = 1)]
public class TurretSetting : ScriptableObject  // Removed the inner class and made it public.
{
    public GameObject TurretPrefab;       // The turret's prefab.
    public int TurretShopCost;            // The cost of the turret in the shop.
    public Sprite TurretShopSprite;       // The sprite for the turret in the shop.
}
