using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponType", menuName = "Tower Defense/Weapon Type")]
public class WeaponType : ScriptableObject
{
    [Header("Weapon Info")]
    public string weaponName;
    public Sprite weaponIcon;
    public GameObject weaponPrefab;
    public GameObject projectilePrefab;
    
    [Header("Stats")]
    public int cost;
    public float range = 5f;
    public float fireRate = 1f;
    public int damage = 10;
    
    [Header("Upgrade Costs")]
    public int damageUpgradeCost = 50;
    public int rangeUpgradeCost = 75;
    public int fireRateUpgradeCost = 100;
} 