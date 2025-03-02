using UnityEngine;

public class Plot : MonoBehaviour
{
    [SerializeField] private ShopManager shopManager; // Reference to the ShopManager

    private void OnMouseDown()
    {
        // Open the shop UI when the player clicks on the plot
        shopManager.OpenShop(this);
    }
}
