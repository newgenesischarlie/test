using System.Collections.Generic;
using UnityEngine;

public class WeaponDatabase : MonoBehaviour
{
    [System.Serializable]
    public class WeaponData
    {
        public string name; // The weapon name as a string (e.g., "1", "2", "3")
        public GameObject weaponPrefab;
        // Removed individual sprite for each weapon
    }

    [Header("Weapon List")]
    public List<WeaponData> weapons; // List to hold all the weapons

    // Universal sprite for all weapons
    public Sprite universalWeaponSprite; // Shared sprite for all weapons

    // Get a weapon by name
    public WeaponData GetWeaponByName(string weaponName)
    {
        foreach (var weapon in weapons)
        {
            if (weapon.name == weaponName)
            {
                return weapon;
            }
        }
        return null; // Return null if weapon not found
    }
}
