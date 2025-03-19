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
        // Check if the UI is being interacted with, and ignore clicks on the UI
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Debug.Log("Plot clicked! Plot Index: " + plotIndex); // Debug message to confirm plot selection

        // Ensure ShopManager is correctly referenced
        ShopManager shop = FindObjectOfType<ShopManager>(); // Correct the reference
        if (shop != null)
        {
            shop.SelectPlot(this); // Pass the current plot to the shop
        }
    }

    public GameObject PlaceWeapon(GameObject weaponPrefab)
    {
        if (isOccupied || weaponPrefab == null)
        {
            Debug.LogWarning("Plot is already occupied or weaponPrefab is null");
            return null;
        }

        Vector3 position = weaponPosition != null ? weaponPosition.position : transform.position;
        currentWeaponObj = Instantiate(weaponPrefab, position, Quaternion.identity);
        currentWeapon = currentWeaponObj.GetComponent<Weapon>();
        isOccupied = true;

        Debug.Log("Weapon placed at position: " + position); // Debug message to confirm weapon placement
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
