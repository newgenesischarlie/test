using UnityEngine;

public class Plot : MonoBehaviour
{
    [SerializeField] private ShopManager shopManager; // Reference to the ShopManager

    private void OnMouseDown()
    {
        // Ensure the shopManager is assigned
        if (shopManager != null)
        {
            Debug.Log("Plot clicked! Opening the shop...");
            shopManager.OpenShop(this); // Open the shop when the plot is clicked
        }
        else
        {
            Debug.LogError("ShopManager is not assigned!");
        }
    }

    // Optional: To visualize the clickable area in the editor, make sure the plot has a collider
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f); // Adjust radius to fit your plot size
    }
}
