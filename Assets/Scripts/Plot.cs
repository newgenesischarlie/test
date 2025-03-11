using UnityEngine;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour
{
    [SerializeField] private Transform weaponPosition;
    
    private GameObject currentWeaponObj;
    private Weapon currentWeapon;
    private bool isOccupied = false;
    
    [Header("Optional")]
    [SerializeField] private int plotIndex; // For identifying specific plots

    private void Start()
    {
        // Initialize plot
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        WeaponShop shop = FindObjectOfType<WeaponShop>();
        if (shop != null)
        {
            shop.SelectPlot(this);
        }
    }

    public GameObject PlaceWeapon(GameObject weaponPrefab)
    {
        if (isOccupied || weaponPrefab == null)
            return null;

        Vector3 position = weaponPosition != null ? 
            weaponPosition.position : transform.position;
        
        currentWeaponObj = Instantiate(weaponPrefab, position, Quaternion.identity);
        currentWeapon = currentWeaponObj.GetComponent<Weapon>();
        isOccupied = true;
        
        return currentWeaponObj;
    }

    public bool IsOccupied()
    {
        return isOccupied;
    }

    public void RemoveWeapon()
    {
        if (currentWeaponObj != null)
        {
            Destroy(currentWeaponObj);
            currentWeaponObj = null;
            currentWeapon = null;
        }
        isOccupied = false;
    }

    // For identifying this plot
    public int GetPlotIndex()
    {
        return plotIndex;
    }

    // Optional: To visualize the clickable area in the editor, make sure the plot has a collider
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f); // Adjust radius to fit your plot size
    }
}
