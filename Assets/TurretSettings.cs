using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSettings : MonoBehaviour
{
    [CreateAssetMenu(fileName = "TurretShopSetting")]
    public class TurretSetting : ScriptableObject
    {
        public GameObject TurretPrefab;
public int TurretShopCost;
        public Sprite TurretShopSprite;
    }
}
